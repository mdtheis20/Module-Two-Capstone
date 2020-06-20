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
        public IList<Site> GetAvailableSitesAcrossCampground(int chosenCampgroundId, DateTime chosenArrival, DateTime chosenDeparture)
        {
            List<Site> sites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string QUERY = @"SELECT DISTINCT site.site_id, campground.name, campground.campground_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee
    FROM site
    JOIN campground ON site.campground_id = campground.campground_id
    LEFT JOIN reservation ON site.site_id = reservation.site_id
    WHERE campground.campground_id = @campground_id AND site.site_id NOT IN(SELECT site.site_id FROM site
    LEFT JOIN reservation ON site.site_id = reservation.site_id
    WHERE campground_id = @campground_id AND from_date < @to_date AND to_date > @from_date)";

                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    cmd.Parameters.AddWithValue("@campground_id", chosenCampgroundId);
                    cmd.Parameters.AddWithValue("@to_date", chosenDeparture);
                    cmd.Parameters.AddWithValue("@from_date", chosenArrival);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site site = ReadToSite(reader);

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

        private static Site ReadToSite(SqlDataReader reader)
        {
            return new Site
            {
                Id = Convert.ToInt32(reader["site_id"]),
                CampgroundName = Convert.ToString(reader["name"]),
                CampgroundId = Convert.ToInt32(reader["campground_id"]),
                SiteNumber = Convert.ToInt32(reader["site_number"]),
                MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                IsAccessible = Convert.ToBoolean(reader["accessible"]),
                MaxRVLength = Convert.ToInt32(reader["max_rv_length"]),
                HasUtilities = Convert.ToBoolean(reader["utilities"]),
                Cost = Convert.ToDecimal(reader["daily_fee"])
            };
        }

        public IList<Site> GetAvailableSitesAcrossPark(int chosenParkId, DateTime chosenArrival, DateTime chosenDeparture)
        {
            List<Site> sites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string QUERY = @"SELECT DISTINCT site.site_id, campground.name, campground.campground_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee
	FROM campground
	JOIN site ON campground.campground_id = site.campground_id
	LEFT JOIN reservation ON site.site_id = reservation.site_id
	WHERE park_id = @park_id AND site.site_id NOT IN (SELECT site.site_id FROM site
	LEFT JOIN reservation ON site.site_id = reservation.site_id
	WHERE park_id = @park_id AND from_date < @to_date AND to_date > @from_date)";

                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    cmd.Parameters.AddWithValue("@park_id", chosenParkId);
                    cmd.Parameters.AddWithValue("@to_date", chosenDeparture);
                    cmd.Parameters.AddWithValue("@from_date", chosenArrival);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site site = ReadToSite(reader);

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
