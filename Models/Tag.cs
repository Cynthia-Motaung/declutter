namespace declutter.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = nameof(Name).ToLower();
        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}
