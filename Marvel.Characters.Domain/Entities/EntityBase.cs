namespace Marvel.Characters.Domain.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
