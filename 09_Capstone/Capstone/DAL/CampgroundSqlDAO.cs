using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO
    {
        private string connectionString;
        
        Dictionary<int, string> months = new Dictionary<int, string>
        {
            { 1, "January" },
            { 2, "February" },
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" }
        };
        public CampgroundSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IList<Campground> GetCampgrounds(Park park)
        {
            List<Campground> campgrounds = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string QUERY = "SELECT * FROM campground WHERE park_id = @park_id";
                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    cmd.Parameters.AddWithValue("@park_id", park.Id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.Id = Convert.ToInt32(reader["campground_id"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpeningMonth = months[Convert.ToInt32(reader["open_from_mm"])];
                        campground.ClosingMonth = months[Convert.ToInt32(reader["open_to_mm"])];
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        campgrounds.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred returning the list of campgrounds " + ex.Message);
                throw;
            }
            return campgrounds;
        }
    }
}
