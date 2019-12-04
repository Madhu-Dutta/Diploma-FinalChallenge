using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameGroupApp.Data;
using GameGroupApp.Data.Models;

namespace GameGroupApp.Pages.PaidUnpaid
{
    public class DetailsModel : PageModel
    {
        private readonly GameGroupApp.Data.ApplicationDbContext _context;

        public DetailsModel(GameGroupApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Record Record { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Record = await _context.Records
                .Include(r => r.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Record == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
