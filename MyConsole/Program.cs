using Npgsql;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace MyConsole
{
    internal class Program
    {
        static string conStr_1 = "Server=207-P;Database=testDB;Trusted_Connection=True;";
        static string conStr_2 = "Server=207-P;Database=testDB;User Id=user1;Password=1234;";
        static string conStr_3 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\байбатыровм.STEP\Documents\testDB.mdf;Integrated Security=True;Connect Timeout=30";
        static SqlConnection db2 = null;


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

        static void TestConnectionPG()
        {
            using (NpgsqlConnection db = new NpgsqlConnection(conStr_2))
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
                    if (ob != null)
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

        static void getCity2()
        {
            using (SqlConnection db = new SqlConnection(conStr_2))
            {
                db.Open();
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("SELECT id, NAME FROM CITY", db))
                {
                    dt.Load(cmd.ExecuteReader());
                    //var res = dt.Select("name = 'Астана'");
                    Console.WriteLine(dt.Rows.Count);
                    foreach (DataRow row in dt.Rows)
                    {
                        Console.WriteLine($"{row[0].ToString()} {row["NAME"].ToString()}");
                    }
                }
                db.Close();
            }
        }

        static void pGetCityByName(string name)
        {
            using (SqlConnection db = new SqlConnection(conStr_2))
            {
                db.Open();
                using (SqlCommand cmd = new SqlCommand("pGetCityByName", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("name", name);
                    var dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        Console.WriteLine("NO DATA");
                        return;
                    }
                    while (dr.Read())
                    {
                        Console.WriteLine($"{dr[0].ToString()} {dr["NAME"].ToString()}");
                    }
                }
                db.Close();
            }
        }

        static void pGetCityNameById(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr_2))
            {
                db.Open();
                using (SqlCommand cmd = new SqlCommand("pGetCityNameById", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.Add("name", SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["name"].Value != null)
                        Console.WriteLine(cmd.Parameters["name"].Value.ToString());
                }
                db.Close();
            }
        }

        static void pStudentReturnCntSymbolsInLastNameById(int id)
        {
            using (SqlCommand cmd = new SqlCommand("pStudentReturnCntSymbolsInLastNameById", db2))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.Add("count", SqlDbType.NVarChar, 1000).Direction = 
                    ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["count"].Value != null)
                    Console.WriteLine(cmd.Parameters["count"].Value.ToString());
            }


        }
        
        static void pGetStudentsAndFaculty()
        {
            using (SqlCommand cmd = new SqlCommand("pGetStudentsAndFaculty", db2))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Console.WriteLine(dr[1].ToString());
                }
                dr.NextResult();
                while (dr.Read())
                {
                    Console.WriteLine(dr[1].ToString());
                }
            }
        }
        static void Main(string[] args)
        {
            db2 = new SqlConnection(conStr_3);
            db2.Open();
            //pStudentReturnCntSymbolsInLastNameById(2);
            pGetStudentsAndFaculty();

            db2.Close();
            //TestConnection();
            //getDate();
            //getCity();
            //getCity2();
            //pGetCityByName("");
            //pGetCityNameById(2);
        }
    }
}

/*
 create proc pGetCityNameById -- 1
@id int,
@name nvarchar(1000) out
as
SELECT		
		@name = [name]
FROM  [testDB].[dbo].[City]
where id = @id

-------------------------
create proc [dbo].[pGetCityByName] --null
@name nvarchar(1000)
as
SELECT
		[id],
		[name]
FROM  [testDB].[dbo].[City]
where (@name is null or @name = '' or [name] = @name)
 
 */