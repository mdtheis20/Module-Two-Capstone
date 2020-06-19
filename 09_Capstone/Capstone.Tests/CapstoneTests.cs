using System;
using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using Capstone.Models;
using System.Data.SqlClient;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class CapstoneTests
    {
        private string connectionString = "Server=.\\SQLExpress;Database=npcampground;Trusted_Connection=true;";

        public TransactionScope transaction;

        [TestInitialize]
        public void Setup()
        {
            transaction = new TransactionScope();
            string sqlText = File.ReadAllText("../../../Setup.sql");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                cmd.ExecuteNonQuery();
            }
            
        }
        [TestCleanup]
        public void Dispose()
        {
            transaction.Dispose();
        }
        [TestMethod]
        public void ParkDAOTest()
        {
            ParkSqlDAO parkSqlDAO = new ParkSqlDAO(connectionString);
            IList<Park> parks = parkSqlDAO.GetParks();
            Assert.AreEqual(2, parks.Count);
        }

        [TestMethod]
        public void CampgroundDAOTest()
        {
            ParkSqlDAO parkSqlDAO = new ParkSqlDAO(connectionString);
            IList<Park> parks = parkSqlDAO.GetParks();
            CampgroundSqlDAO campgroundSqlDAO = new CampgroundSqlDAO(connectionString);
            IList<Campground> campgrounds = campgroundSqlDAO.GetCampgrounds(parks[0]);
            Assert.AreEqual("Blackwoods", campgrounds[0].Name);
        }
    }
}
