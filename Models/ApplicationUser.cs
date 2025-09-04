using Microsoft.AspNetCore.Identity;

namespace declutter.Models
{
    public class ApplicationUser :IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}
