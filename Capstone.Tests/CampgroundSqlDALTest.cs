using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;
using Capstone.Models;
using Capstone.DAL;
using System.Data.SqlClient;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSqlDALTest
    {
      
       public static string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";


        [TestMethod]
        public void GetAllCampgroundsInPark()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                CampgroundSqlDALTest.InsertFakeCampground(0, 1, "test", 10, 12, 20);

                CampgroundSqlDAL testClass = new CampgroundSqlDAL(connection);

                List<Campground> campground = testClass.GetAllCampgroundsInPark("Acadia");

                Assert.AreEqual(4, campground.Count);
            }
        }

        public static int InsertFakeCampground(int CampgroundId, int ParkId, string Name, int OpenFromMm, int OpenToMm, int DailyFee )
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO campground VALUES (@park_id, @name, @openFromMm, @openToMm, @dailyFee)", conn);

                cmd.Parameters.AddWithValue("@park_id", ParkId);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@openFromMm", OpenFromMm);
                cmd.Parameters.AddWithValue("@openToMm", OpenToMm);
                cmd.Parameters.AddWithValue("@dailyFee", DailyFee);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(campground_id) FROM campground", conn);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
        }
    }
    
}
