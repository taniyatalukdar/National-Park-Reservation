using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;


namespace Capstone.Tests
{
    [TestClass]
    public class CampsiteSqlDALTests
    {
        public static string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";

        [TestMethod]
        public void GetAvailableSitesInCampgroundtests()
        {
                    

    
    }
}
}
