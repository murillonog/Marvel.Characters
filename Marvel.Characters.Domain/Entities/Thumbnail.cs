namespace Marvel.Characters.Domain.Entities
{
    public class Thumbnail
    {
        public Thumbnail(string path, string extension, int characterId)
        {
            Path = path;
            Extension = extension;
            CharacterId = characterId;
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
