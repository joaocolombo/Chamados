using System.Data.SqlTypes;
using System.Security.Cryptography;
using Domain.Entities;

namespace Data.Repositories
{
    public static class AtendenteRepository
    {
        public static Atendente BuscarAtendente(string nome)
        {
            // buscar no sistema do cristano Gerenciador de Usuarios ==============================
            var nivel = "N1";
            if (nome.Contains("Joao") || nome.Contains("Everson") || nome.Contains("Osiel"))
            {
                nivel = "N2";
            }
            return new Atendente(){Nome = nome, Nivel = nivel};


        } 
    }
}