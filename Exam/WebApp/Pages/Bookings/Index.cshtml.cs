using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchStudent { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchRoom { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchStatus { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Student)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchStudent))
            {
                query = query.Where(b => b.Student.FullName.Contains(SearchStudent));
            }
            if (!string.IsNullOrWhiteSpace(SearchRoom))
            {
                query = query.Where(b => b.Room.RoomNumber.Contains(SearchRoom));
            }
            if (!string.IsNullOrWhiteSpace(SearchStatus))
            {
                query = query.Where(b => b.Status.ToString() == SearchStatus);
            }

            Booking = await query.ToListAsync();
        }
    }
}
