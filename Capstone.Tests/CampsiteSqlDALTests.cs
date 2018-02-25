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
    public class CampsiteSqlDALTests
    {
        public static string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";

        [TestMethod]
        public void TestReservationUnderEmpty()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(10, parkId, "test", 10, 5, 100);
                int campSiteId = CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);
                int id = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));
                

                ReservationSqlDAL testClass = new ReservationSqlDAL(connection);
               
                List<Reservation> site = testClass.GetAllReservations("test");

                Assert.AreEqual(1, site.Count);
            }
        }
        [TestMethod]
        public void GetAllCampsiteTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(0, parkId, "test", 10, 12, 20);
                CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);

                SiteSqlDAL testClass = new SiteSqlDAL(connection);

                List<Campsite> site = testClass.GetAllCampSites(campgroundId);

                Assert.AreEqual(52, site.Count);
            }
        }
        

        public static int InsertFakeCampsite(int CampGroundId, int SiteNumber, int MaxOccupancy, bool Accessible, int MaxRvLength, bool Utilities)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO site VALUES ( @campGroundId, @siteNumber, @maxOccupancy, @accessible, @maxRvLength, @utilities)", conn);


               
                cmd.Parameters.AddWithValue("@campGroundId", CampGroundId);
                cmd.Parameters.AddWithValue("@siteNumber", SiteNumber);
                cmd.Parameters.AddWithValue("@maxOccupancy", MaxOccupancy);
                cmd.Parameters.AddWithValue("@accessible", Accessible);
                cmd.Parameters.AddWithValue("@maxRvLength", MaxRvLength);
                cmd.Parameters.AddWithValue("@utilities", Utilities);


                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(site_id) FROM site", conn);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
        }


    }
}
