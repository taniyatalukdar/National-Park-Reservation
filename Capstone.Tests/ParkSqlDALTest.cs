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
        public string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";

        //public ParkSqlDALTest(string dbconnectionString)
        //{
        //    connectionString = dbconnectionString;
        //}

        [TestMethod]
        public void GetAllParks()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                ParkSqlDALTest.InsertFakePark(0, "test", (Convert.ToDateTime("1/15/1990")), "Cleveland", 1000, 13, "An amusing campsite");
            }
        }

        public static int InsertFakePark(int Id, string Name, DateTime establishDate, string Location, int area, int visitors, string description)
        {
            using (SqlConnection conn = new SqlConnection(@"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO park VALUES (@Id, @name, @establishDate, @locaton, @area, @visitors, @description", conn);
                
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@establishDate", Convert.ToDateTime("1/15/1990"));
                cmd.Parameters.AddWithValue("@locaton", Location);
                cmd.Parameters.AddWithValue("@area", area);
                cmd.Parameters.AddWithValue("@vistors", visitors);
                cmd.Parameters.AddWithValue("description", description);






                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT MAX(Id) FROM park", conn);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
        }
    }
}
