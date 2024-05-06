using System.Data;
using System.Data.SqlClient;
namespace Pronia.Helper
{
    public class HelpMethod
    {
        const string ConnectingPath = "Server=ZEGA;Database=Pronia;Trusted_Connection=True;Integrated Security=True;";
        public int Id { get; set; }

        public HelpMethod(int id)
        {
            Id = id;
        }



        public static DataTable GetDataQuery(string query)
        {
            using SqlConnection connecting = new SqlConnection(ConnectingPath);
            connecting.Open();
            using SqlDataAdapter adapter = new SqlDataAdapter(query, connecting);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public static int GetData(string query)
        {
            using SqlConnection connecting = new SqlConnection(ConnectingPath);
            connecting.Open();
            using SqlCommand cmd = new SqlCommand(query, connecting);
            return cmd.ExecuteNonQuery();
        }
    }
}
