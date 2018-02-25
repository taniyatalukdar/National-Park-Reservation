using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Capstone.DAL;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlDALTests
    {
        public static string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";
       

        [TestMethod]
        public void CreateNewReservationTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {


                int id = ReservationSqlDALTests.InsertFakeReservation(17, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));

                ReservationSqlDAL testClass = new ReservationSqlDAL(connection);

                List<Reservation> reservations = testClass.GetReservationById("Seawall");

                bool containsReservation = reservations.Exists(r => r.ReservationId == id);

                Assert.IsTrue(containsReservation);
                Assert.AreEqual(14, reservations.Count);
            }
        }
        

        [TestMethod]
        public void SiteAvailableForReservationTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(0, parkId, "test", 10, 12, 20);
                int campSiteId = CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);
                int id = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));

                SiteSqlDAL testClass = new SiteSqlDAL(connection);

                List<Campsite> site = testClass.GetAvailableSitesInCampground(campgroundId, Convert.ToDateTime("2/20/2018"), Convert.ToDateTime("2/22/2018"));

                Assert.AreEqual(1, site.Count);
            }
        }
        [TestMethod]
        public void SiteAvailableForReservationSameEndDateAndStartDateTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(0, parkId, "test", 10, 12, 20);
                int campSiteId = CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);
                int id = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));
                int id2 = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Josh", Convert.ToDateTime("4/18/2018"), Convert.ToDateTime("4/20/2018"), Convert.ToString(DateTime.Now));
                SiteSqlDAL testClass = new SiteSqlDAL(connection);

                List<Campsite> site = testClass.GetAvailableSitesInCampground(campgroundId, Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"));

                Assert.AreEqual(0, site.Count);
            }
        }
        [TestMethod]
        public void SiteAvailableForReservationOverlappingDatesTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(0, parkId, "test", 10, 12, 20);
                int campSiteId = CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);
                int id = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));
                int id2 = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Josh", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/20/2018"), Convert.ToString(DateTime.Now));
                SiteSqlDAL testClass = new SiteSqlDAL(connection);

                List<Campsite> site = testClass.GetAvailableSitesInCampground(campgroundId, Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/20/2018"));

                Assert.AreEqual(0, site.Count);
            }
        }
        [TestMethod]
        public void SiteAvailableForReservationIntersectingDatesTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(0, parkId, "test", 10, 12, 20);
                int campSiteId = CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);
                int id = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/20/2018"), Convert.ToString(DateTime.Now));
                int id2 = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Josh", Convert.ToDateTime("4/16/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));
                SiteSqlDAL testClass = new SiteSqlDAL(connection);

                List<Campsite> site = testClass.GetAvailableSitesInCampground(campgroundId, Convert.ToDateTime("4/16/2018"), Convert.ToDateTime("4/18/2018"));

                Assert.AreEqual(0, site.Count);
            }
        }

        [TestMethod]
        public void SiteAvailableForReservationSameStartDateAndEndDateTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                int parkId = ParkSqlDALTest.InsertFakePark(10, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");
                int campgroundId = CampgroundSqlDALTest.InsertFakeCampground(0, parkId, "test", 10, 12, 20);
                int campSiteId = CampsiteSqlDALTests.InsertFakeCampsite(campgroundId, 98, 6, true, 50, true);
                int id = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Greg", Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"), Convert.ToString(DateTime.Now));
                int id2 = ReservationSqlDALTests.InsertFakeReservation(campSiteId, "Josh", Convert.ToDateTime("4/18/2018"), Convert.ToDateTime("4/20/2018"), Convert.ToString(DateTime.Now));
                SiteSqlDAL testClass = new SiteSqlDAL(connection);

                List<Campsite> site = testClass.GetAvailableSitesInCampground(campgroundId, Convert.ToDateTime("4/15/2018"), Convert.ToDateTime("4/18/2018"));

                Assert.AreEqual(0, site.Count);
            }
        }

        public static int InsertFakeReservation(int SiteId, string ReservationName, DateTime StartDate, DateTime EndDate, string CreateDate)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("insert into reservation Values(@siteId, @reservationName, @startDate, @endDate, @createDate)", conn);

                cmd.Parameters.AddWithValue("@siteId", SiteId);
                cmd.Parameters.AddWithValue("@reservationName", ReservationName);
                cmd.Parameters.AddWithValue("@startDate", StartDate);
                cmd.Parameters.AddWithValue("@endDate", EndDate);
                cmd.Parameters.AddWithValue("@createDate", CreateDate);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select max(reservation_id) from reservation", conn);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;

            }
        }


    }

}
