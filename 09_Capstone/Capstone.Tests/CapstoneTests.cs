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
        //private int blackwoodsSite1Id;
        //private int blackwoodsSite2Id;
        private int blackwoodsId;
        //private int seawallId;
        //private int nextReservationId;
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
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    blackwoodsId = Convert.ToInt32(reader["blackwoodsId"]);
                    //seawallId = Convert.ToInt32(reader["seawallId"]);
                    //blackwoodsSite1Id = Convert.ToInt32(reader["blackwoodsSite1Id"]);
                    //nextReservationId = Convert.ToInt32(reader["nextReservationId"]);
                    //site2Id = Convert.ToInt32(reader["site2Id"]);
                }
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
        [TestMethod]
        public void SiteDAOTest()
        {
            SiteSqlDAO siteSqlDAO = new SiteSqlDAO(connectionString);
            DateTime fromDate = new DateTime(2020, 7, 1);
            DateTime toDate = new DateTime(2020, 7, 5);
            IList<Site> sites = siteSqlDAO.GetAvailableSitesAcrossCampground(this.blackwoodsId, fromDate, toDate);
            Assert.AreEqual(2, sites.Count);
        }
        //[TestMethod]
        //public void ReservationDAOTest()
        //{
        //    ReservationSqlDAO reservationSqlDAO = new ReservationSqlDAO(connectionString);
        //    DateTime fromDate = new DateTime(2020, 7, 1);
        //    DateTime toDate = new DateTime(2020, 7, 5);
        //    int reservationId = reservationSqlDAO.ReserveSite(this.blackwoodsSite1Id, "Dwight Schrute", fromDate, toDate);
        //    Assert.AreEqual(nextReservationId, reservationId);
        //}
    }
}
