using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string connectionString;

        public ReservationSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        
        public bool CreateNewReservation(Reservation newReservation)
        {
            bool reservationId = true;


            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO reservation (site_id, name, from_date, to_date) " +
                                                    "VALUES (@siteId, @reservationName, @arrivalDate, @departDate);", conn);

                    cmd.Parameters.AddWithValue("@siteId",newReservation.SiteId);
                    cmd.Parameters.AddWithValue("@reservationName", newReservation.Name);
                    cmd.Parameters.AddWithValue("@arrivalDate", newReservation.From_Date);
                    cmd.Parameters.AddWithValue("@departDate", newReservation.To_Date);

                    cmd.ExecuteNonQuery();

                    
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred. " + e.Message);
                reservationId = false;
            }

            return reservationId;
        }

        
        public List<Reservation> GetAllReservations(string CampgroundName)
        {
            List<Reservation> output = new List<Reservation>();

            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM reservation " +
                                                    "INNER JOIN site ON reservation.site_id = site.site_id," +
                                                    "INNER JOIN campground ON site.campground_id = campground.campground_id" +
                                                    " WHERE campground.name = @campgroundName;", conn);
                    
                    cmd.Parameters.AddWithValue("@campgroundName", CampgroundName);

                   
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output.Add(PopulateReservation(reader));
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

        public Reservation PopulateReservation(SqlDataReader reader)
        {
            Reservation r = new Reservation();
            r.ReservationId = (int)reader["reservation_id"];
            r.SiteId = (int)reader["site_id"];
            r.Name = (string)reader["name"];
            r.From_Date = Convert.ToDateTime(reader["from_date"]);
            r.To_Date = Convert.ToDateTime(reader["to_date"]);
            r.Create_Date = Convert.ToDateTime(reader["from_date"]);

            return r;
        }

    }
}