using System.Collections.Generic;

namespace Domain.Entities
{
    public class Usuario
    {
        public string NomeExibicao { get; set; }
        public string Login { get; set; }
        public int Codigo { get; set; }
        public string  Email { get; set; }
        public List<string> Grupos { get; set; }

    }
}