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
    public class ParkSqlDALTest
    {
        public static string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";

        //public ParkSqlDALTest(string dbconnectionString)
        //{
        //    connectionString = dbconnectionString;
        //}

        [TestMethod]
        public void GetAllParks()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                ParkSqlDALTest.InsertFakePark(0, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 42, "An amusing campsite");

                ParkSqlDAL testClass = new ParkSqlDAL(connection);

                List<Park> park = testClass.GetAllParks();

                Assert.AreEqual(4, park.Count);
            }
        }

        public static int InsertFakePark(int Id, string Name, DateTime establishDate, string Location, int area, int visitors, string description)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO park VALUES (@name, @locaton, @establishDate, @area, @visitors, @description)", conn);
                
                
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@establishDate", "1/1/1990");
                cmd.Parameters.AddWithValue("@locaton", Location);
                cmd.Parameters.AddWithValue("@area", area);
                cmd.Parameters.AddWithValue("@visitors", visitors);
                cmd.Parameters.AddWithValue("@description", description);

                
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(park_id) FROM park", conn);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
        }
    }
}
