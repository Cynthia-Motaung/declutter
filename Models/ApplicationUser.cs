using Microsoft.AspNetCore.Identity;

namespace declutter.Models
{
    public class ApplicationUser :IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; } 
        

        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}
