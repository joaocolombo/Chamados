using System;
using System.Data.SqlClient;

namespace Data
{
    public static class ChamadosDb
    {
        
        private static SqlConnection con;

        private static void OpenConection()
        {
            try
            {
                con = new SqlConnection("Server=10.1.0.4;Database=Chamados;User Id=joao.colombo;Password=991240; Max Pool Size=8000;");
                con.Open();
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public static SqlConnection Conecection()
        {
            OpenConection();
            
            return con;
        }


        public static void CloseConnection()
        {
            con.Close();
        }

        public static SqlDataReader DataReader(SqlCommand command)
        {
            OpenConection();
            SqlCommand cmd = command;
            command.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }


        public static void ExecuteQueries(string QuerySql)
        {
            OpenConection();
            SqlCommand cmd = new SqlCommand(QuerySql, con);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public static void ExecuteQueries(SqlCommand command)
        {
            OpenConection();
            SqlCommand cmd = command;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public static SqlDataReader DataReader(string QuerySql)
        {
            OpenConection();
            SqlCommand cmd = new SqlCommand(QuerySql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            CloseConnection();
            return dr;
        }

    }
}