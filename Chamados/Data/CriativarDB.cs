using System.Data.SqlClient;

namespace Data
{
    public class CriativarDB
    {
        private static SqlConnection con;

        private static void OpenConection()
        {
            con = new SqlConnection("Server=10.1.0.4;Database=CRIATIVAR;User Id=joao.colombo;Password=991240;");
            con.Open();
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