namespace declutter.Models
{
    public class Entry
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ApplicationUser? Author { get; set; }

        public string AuthorId { get; set; } = string.Empty;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public string Slug => Title.ToLower().Replace(' ', '-');
    }
}
