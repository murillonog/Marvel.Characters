namespace Marvel.Characters.Domain.Entities
{
    public class Character
    {       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Modified { get; set; }
        public string ResourceURI { get; set; }
        public bool Favorite { get; set; }
        public List<Url> Urls { get; set; }
        public Thumbnail? Thumbnail { get; set; }
        public List<Comic> Comics { get; set; }
        public List<Story> Stories { get; set; }
        public List<Event> Events { get; set; }
        public List<Series> Series { get; set; }

        public Character(string name, string description, string? modified, string resourceURI)
        {
            Name = name;
            Description = description;
            Modified = modified;
            ResourceURI = resourceURI;
            Urls = new List<Url>();
            Comics = new List<Comic>();
            Stories = new List<Story>();
            Events = new List<Event>();
            Series = new List<Series>();
        }
    }
}
