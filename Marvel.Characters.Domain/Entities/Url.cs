namespace Marvel.Characters.Domain.Entities
{
    public class Url
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? URL { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
