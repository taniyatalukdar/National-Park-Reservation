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
        
        public List<Campsite> GetAllSitesInCampground(string campgroundName)
        {
            List<Campsite> output = new List<Campsite>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM site " +
                                                    "INNER JOIN campground ON site.campground_id = campground.campground_id" +
                                                    " WHERE campground.name = @campgroundName;", conn);
                    
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);

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
        
        public List<Campsite> GetAvailableSitesInCampground(string campgroundName, DateTime arrivalDate, DateTime departDate)
        {
            List<Campsite> output = new List<Campsite>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT  * FROM site " +
                                                    "INNER JOIN campground ON site.campground_id = campground.campground_id " +
                                                    "WHERE campground.name = @campgroundName AND site_id NOT IN " +
                                                    "(SELECT site_id FROM reservation WHERE " +
                                                    "from_date Between '03/15/2018' AND '10/25/2018' OR " +
                                                    "to_date Between '03/15/2018' AND '10/25/2018' OR " +
                                                    "(from_date Between '03/15/2018' AND '10/25/2018' AND to_date Between '03/15/2018' AND '10/25/2018') " +
                                                    "OR from_date< '03/15/2018' AND to_date > '10/25/2018');", conn);

                    
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);
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


        
        public List<Campsite> GetReservedSitesInCampground(string campgroundName, string arrivalDate, string departDate)
        {
            List<Campsite> output = new List<Campsite>();

            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM reservation " +
                                                    " INNER JOIN site ON reservation.site_id = site.site_id" +
                                                    " INNER JOIN campground ON site.campground_id = campground.campground_id " +
                                                    " WHERE campground.name = @campgroundName " +
                                                    " AND exists (SELECT * FROM reservation" +
                                                    " WHERE @arrivalDate NOT BETWEEN (select min(from_date) from reservation) " +
                                                    " AND (select max(to_date) from reservation));", conn);

                    
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);
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