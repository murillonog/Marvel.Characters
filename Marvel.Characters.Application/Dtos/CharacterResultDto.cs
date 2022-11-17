using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Characters.Application.Dtos
{
    public class CharacterResultDto
    {
        public int? Page { get; set; }
        public int? Size { get; set; }
        public int? Total { get; set; }
        public int? Count { get; set; }
        public List<CharacterDto>? Results { get; set; }
    }
}
