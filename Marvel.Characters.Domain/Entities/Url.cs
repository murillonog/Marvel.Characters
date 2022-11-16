namespace Marvel.Characters.Domain.Entities
{
    public class Url
    {
        public Url(string uRL, string type, int characterId)
        {
            URL = uRL;
            Type = type;
            CharacterId = characterId;
        }

        public int Id { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
