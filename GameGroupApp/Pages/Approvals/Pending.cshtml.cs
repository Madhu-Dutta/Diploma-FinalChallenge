using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameGroupApp.Data;
using GameGroupApp.Data.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GameGroupApp.Pages.Approvals
{
    public class PendingModel : PageModel
    {
     
        private readonly ApplicationDbContext _context;

        public PendingModel(ApplicationDbContext context)
        {
            _context = context;
        }

        //Make a List of Pending members
        public List<PendingMember> PendingMembers { get; set; }

        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public MemberStatus ChangeStatus { get; set; }

        public class PendingMember
        {
            public string UserId { get; set; }
            public string Email { get; set; }
        }

        //Get all Pending Members
        public async Task OnGetAsync()
        {
            //Get Pending Members
            var pendingClaims = _context.UserClaims
                 .Where(x =>
                     x.ClaimType == MemberClaims.MemberStatus
                     && x.ClaimValue == MemberStatus.Pending.ToString());
            //Get members
            var members = _context
                .UserClaims
                .Where(c =>
                    c.ClaimType == MemberClaims.Email);
            PendingMembers = await pendingClaims.Join(
                members,
                pc => pc.UserId,
                am => am.UserId,
                (user, email) => new PendingMember
                {
                    UserId = user.UserId,
                    Email = email.ClaimValue
                })
                .ToListAsync();
        }

        //Post members
        public async Task<IActionResult> OnPostChangeStatusAsync(string id, MemberStatus change)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ///Grab the id and status of the member and store it as claimValue
            var claim = await _context.UserClaims
                .Where(c =>
                    c.ClaimType == MemberClaims.MemberStatus
                    && c.UserId == id)
                .FirstOrDefaultAsync();

            claim.ClaimValue = change.ToString();

            await _context.SaveChangesAsync();
            return RedirectToPage("./Approve");
        }

    }
}

        