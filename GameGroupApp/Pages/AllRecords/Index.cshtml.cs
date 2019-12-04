using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameGroupApp.Data;
using GameGroupApp.Data.Models;

namespace GameGroupApp.Pages.AllRecords
{
    public class IndexModel : PageModel
    {
        private readonly GameGroupApp.Data.ApplicationDbContext _context;

        public IndexModel(GameGroupApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get;set; }
        public ViewBy ViewBySetting { get; set; }

        public async Task OnGetAsync(ViewBy viewBy = ViewBy.Future)
        {
            ViewBySetting = viewBy;
            //Record = await _context.Records
            //    .Include(r => r.User).ToListAsync();

            switch (viewBy)
            {

                case ViewBy.Future:
                    Record = await _context.Records
                        .Where(g => g.Date > DateTime.Now)
                        .Include(g => g.User)
                        .ToListAsync();
                    break;
                case ViewBy.Past:
                    Record = await _context
                        .Records
                        .Where(g => g.Date < DateTime.Now)
                        .Include(g => g.User)
                        .ToListAsync();
                    break;
                default:
                    Record = await _context
                        .Records
                        .Include(g => g.User)
                        .ToListAsync();
                    break;
            }
        }
        public enum ViewBy
        {
            All,
            Future,
            Past
        }
    }
}
