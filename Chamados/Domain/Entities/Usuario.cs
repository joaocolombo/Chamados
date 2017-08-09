using System.Collections.Generic;

namespace Domain.Entities
{
    public class Usuario
    {
        public string Nome { get; set; }
        public int Id { get; set; }
        public string  Email { get; set; }
        public List<string> Grupos { get; set; }

    }
}