using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameGroupApp.Data;
using GameGroupApp.Data.Models;

namespace GameGroupApp.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly GameGroupApp.Data.ApplicationDbContext _context;

        public IndexModel(GameGroupApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get;set; }
        public IList<PaymentTotal> Totals { get; set; }
        public ViewType ViewTypeSelected { get; set; }

        public async Task OnGetAsync(ViewType viewType = ViewType.Totals)
        {
            ViewTypeSelected = viewType;

            Record = await _context.Records
                .FromSqlRaw("SELECT * FROM dbo.Records")
                .Include(r => r.User).ToListAsync();

            switch (viewType)
            {
                case ViewType.Totals:
                    Totals = Record.GroupBy(r => r.UserId).Select(
                        pt => new PaymentTotal
                        {
                            UserId = pt.First().UserId,
                            UserName = pt.First().User.UserName,
                            Total = pt.Sum(c => c.Amount)
                        }
                        ).ToList();
                    break;
                case ViewType.All:
                    break;
                default:
                    break;
            }      

        }

        public async Task OnGetTotalsAsync()
        {
            await OnGetAsync();
        }

        

        public enum ViewType
        {
            All,
            Totals
        }
    }
}
