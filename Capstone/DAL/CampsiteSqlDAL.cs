using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string connectionString;

        public SiteSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }
        

        public List<Campsite> GetAvailableSitesInCampground(int campgroundId, DateTime arrivalDate, DateTime departDate)
        {
            List<Campsite> output = new List<Campsite>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();



                    SqlCommand cmd = new SqlCommand("select TOP 5 * " +
                                                    "from site " +
                                                    "JOIN campground ON site.campground_id = campground.campground_id " +
                                                    "JOIN park on park.park_id = campground.park_id " +
                                                    "where site.site_id not in " +
                                                    "    ((select reservation.site_id " +
                                                    "    from reservation " +
                                                    "    where " +
                                                    "        (@arrivalDate <= from_date and @departDate >= to_date) " +
                                                    "        or(@arrivalDate >= from_date and @arrivalDate < to_date) " +
                                                    "        or(@departDate > from_date and @departDate <= to_date))) " +
                                                   // "        and ((1 >= campground.open_from_mm and  " +  
                                                   // "        1 <= campground.open_to_mm) and " +
                                                   // "        (3 >= campground.open_from_mm and " +
                                                   // "        3 <= campground.open_to_mm)) " +
                                                    "        and site.campground_id = @campgroundId;", conn);


                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departDate", departDate);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Add(PopulateSite(reader));
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
        
        public Campsite PopulateSite(SqlDataReader reader)
        {
            Campsite s = new Campsite();
            s.CampSiteID = (int)reader["site_id"];
            s.CampgroundId = (int)reader["campground_id"];
            s.Site_Number = (int)reader["site_number"];
            s.Max_Occupancy = (int)reader["max_occupancy"];
            s.Accessible = (bool)reader["accessible"];
            s.Max_Rv_Length = (int)reader["max_rv_length"];
            s.Utilities = (bool)reader["utilities"];

            return s;
        }

    }
}