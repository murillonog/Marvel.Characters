namespace Marvel.Characters.Domain.Entities
{
    public class Story
    {
        public Story( string name, string resourceURI, string type, int characterId)
        {
            Name = name;
            ResourceURI = resourceURI;
            Type = type;
            CharacterId = characterId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ResourceURI { get; set; }
        public string Type { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
