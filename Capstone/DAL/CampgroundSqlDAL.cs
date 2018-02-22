using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;

        public CampgroundSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        List<Campground> output = new List<Campground>();
        // Returns a List that contains the names of all
        // Campgrounds in the selected park
        public List<Campground> GetAllCampgroundsInPark(string parkName)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM campground " +
                                                    "INNER JOIN park ON campground.park_id = park.park_id" +
                                                    " WHERE park.name = @parkName ORDER BY campground.name;", conn);
                    cmd.Parameters.AddWithValue("@parkName", parkName);
                    

                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        c.ParkId = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.Open_From_MM = Convert.ToInt32(reader["open_to_mm"]);
                        c.Open_To_MM = Convert.ToInt32(reader["open_to_mm"]);
                        c.Daily_Fee = Convert.ToInt32(reader["daily_fee"]);


                        output.Add(c);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred. " + ex.Message);
                throw;
            }

            return output;
        }

        
    }
}