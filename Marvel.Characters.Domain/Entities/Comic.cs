﻿namespace Marvel.Characters.Domain.Entities
{
    public class Comic
    {
        public int Id { get; set; }
        public string? ResourceURI { get; set; }
        public string? Name { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
