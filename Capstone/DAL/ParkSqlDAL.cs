using System;
using Capstone.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    class ParkSqlDAL
    {
        private string connectionString;

        public ParkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Park> GetAllParks()
        {
            List<Park> output = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Park ORDER BY NAME ASC, conn");

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        output.Add(PopulatePark(reader));
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("An error occured while reading the data, please try again!" + ex.Message);
                throw;
            }

            return output;
        }
         
        public Park PopulatePark(SqlDataReader reader)
        {
            Park p = new Park();
            p.Id = Convert.ToInt32(reader["park_id"]);
            p.Name = Convert.ToString(reader["name"]);
            p.Location = Convert.ToString(reader["location"]);
            p.Establish_Date = Convert.ToDateTime(reader["establish_date"]);
            p.Area = Convert.ToInt32(reader["area"]);
            p.Visitors = Convert.ToInt32(reader["visitors"]);
            p.Description = Convert.ToString(reader["description"]);

            return p;

        }
        
    }
}
