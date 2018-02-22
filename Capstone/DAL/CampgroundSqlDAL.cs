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
        
        public CampgroundSqlDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
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
                        output.Add(PopulateCampground(reader));
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

        public Campground PopulateCampground(SqlDataReader reader)
        {
            Campground c = new Campground();
            c.CampgroundId = (int)reader["campground_id"];
            c.ParkId = (int)reader["park_id"];
            c.Name = Convert.ToString(reader["name"]);
            //c.OpenFromMM = (int)reader["open_from_mm"];
            //c.OpenToMM = (int)reader["open_to_mm"];
            //c.DailyFee = (decimal)reader["daily_fee"];

            return c;
        }
    }
}