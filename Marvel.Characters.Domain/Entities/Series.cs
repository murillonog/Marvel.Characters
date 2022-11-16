namespace Marvel.Characters.Domain.Entities
{
    public class Series
    {
        public Series(string name, string resourceURI, int characterId)
        {
            Name = name;
            ResourceURI = resourceURI;
            CharacterId = characterId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ResourceURI { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
