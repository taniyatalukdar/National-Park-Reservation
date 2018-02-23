﻿using System;
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

        
        public int CreateNewReservation(string reservationName, string startDate, string endDate, int siteId)
        {
            int reservationId = 0;

            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO reservation (site_id, name, from_date, to_date) " +
                                                    "VALUES (@siteId, @reservationName, @arrivalDate, @departDate);", conn);

                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@reservationName", reservationName);
                    cmd.Parameters.AddWithValue("@arrivalDate", startDate);
                    cmd.Parameters.AddWithValue("@departDate", endDate);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT MAX(reservation_id) FROM reservation;", conn);
                    reservationId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred. " + e.Message);
                throw;
            }

            return reservationId;
        }

        
        public List<Reservation> GetAllReservations(string campgroundName)
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
                    
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);

                   
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