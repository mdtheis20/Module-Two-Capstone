using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO
    {
        private string connectionString;
        public SiteSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IList<Site> GetAvailableSites(int chosenCampground, DateTime chosenArrival, DateTime chosenDeparture)
        {
            List<Site> sites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string QUERY = @"SELECT DISTINCT site.site_id, campground.campground_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee
    FROM site
    JOIN campground ON site.campground_id = campground.campground_id
    LEFT JOIN reservation ON site.site_id = reservation.site_id
    WHERE campground.campground_id = @campground_id AND site.site_id NOT IN(SELECT site.site_id FROM site
    LEFT JOIN reservation ON site.site_id = reservation.site_id
    WHERE campground_id = @campground_id AND from_date < @to_date AND to_date > @from_date)";

                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    cmd.Parameters.AddWithValue("@campground_id", chosenCampground);
                    cmd.Parameters.AddWithValue("@to_date", chosenDeparture);
                    cmd.Parameters.AddWithValue("@from_date", chosenArrival);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site site = new Site();

                        site.Id = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.HasUtilities = Convert.ToBoolean(reader["utilities"]);
                        site.Cost = Convert.ToDecimal(reader["daily_fee"]);

                        sites.Add(site);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error returning the available sites " + ex.Message);
            }
            return sites;
        }
    }
}
