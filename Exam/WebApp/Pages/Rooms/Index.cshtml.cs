using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Room> Room { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchNumber { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchSize { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchNumber))
            {
                query = query.Where(r => r.RoomNumber.Contains(SearchNumber));
            }
            if (!string.IsNullOrWhiteSpace(SearchSize))
            {
                query = query.Where(r => r.Size.ToString() == SearchSize);
            }

            Room = await query.ToListAsync();
        }
    }
}
