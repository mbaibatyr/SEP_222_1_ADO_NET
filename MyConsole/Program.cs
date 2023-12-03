using System.Data.SqlClient;

namespace MyConsole
{
    internal class Program
    {
        static string conStr_1 = "Server=214-P;Database=testDB;Trusted_Connection=True;";
        static string conStr_2 = "Server=214-P;Database=testDB;User Id=user1;Password=1234;";

        static void TestConnection()
        {
            using (SqlConnection db = new SqlConnection(conStr_2))
            {
                db.Open();
                Console.WriteLine(db.State.ToString());
                db.Close();
                Console.WriteLine(db.State.ToString());
            }
        }

        static void getDate()
        {
            using (SqlConnection db = new SqlConnection(conStr_2))
            {
                db.Open();
                using (SqlCommand cmd = new SqlCommand("Select getdate()", db))
                {
                    var ob = cmd.ExecuteScalar();
                    if(ob != null)
                        Console.WriteLine(ob.ToString());
                    else
                        Console.WriteLine("Empty");
                }

                Console.WriteLine();
                db.Close();
            }
        }

        static void getCity()
        {
            using (SqlConnection db = new SqlConnection(conStr_2))
            {
                db.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT id, NAME FROM CITY", db))
                {
                   var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Console.WriteLine($"{dr[0].ToString()} {dr["NAME"].ToString()}");
                    }
                }                
                db.Close();
            }
        }

        static void Main(string[] args)
        {
            //TestConnection();
            //getDate();
            getCity();
        }
    }
}