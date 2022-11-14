namespace Marvel.Characters.Domain.Entities
{
    public class Story : EntityBase
    {
        public string? ResourceURI { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}
