using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ParkSqlDAO
    {
        private string connectionString;

        public ParkSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IList<Park> GetParks()
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string QUERY = "SELECT * FROM park";
                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.Id = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);
                        parks.Add(park);
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
