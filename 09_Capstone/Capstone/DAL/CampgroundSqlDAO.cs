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
                    const string QUERY = "SELECT * FROM campground";
                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.Id = Convert.ToInt32(reader["campground_id"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpeningMonth = Convert.ToDateTime(reader["open_from_mm"]);
                        campground.ClosingMonth = Convert.ToDateTime(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        campgrounds.Add(campground);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred returning the list of parks " + ex.Message);
                throw;
            }
            return parks;
        }
    }
}
