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
        public void CreateReservationTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                

                int id =ReservationSqlDALTests.InsertFakeReservation(17, "Greg", "4/15/2018", "4/18/2018", Convert.ToString(DateTime.Now));
                ReservationSqlDAL testClass = new ReservationSqlDAL(connection);

                List<Reservation> reservations = testClass.GetAllReservations("Seawall");

                bool containsReservation = reservations.Exists(r => r.ReservationId == id);
               
                Assert.IsTrue(containsReservation);

            }

    }
        public static int InsertFakeReservation(int SiteId, string ReservationName, string StartDate, string EndDate, string CreateDate)
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
