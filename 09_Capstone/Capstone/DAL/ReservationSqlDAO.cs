using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO
    {
        private string connectionString;
        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int ReserveSite(int chosenSite, string chosenName, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string QUERY = @"INSERT reservation (site_id, name, from_date, to_date, create_date)
VALUES(@chosenSite, @chosenName, @fromDate, @toDate, (SELECT GETDATE()))";
                    SqlCommand cmd = new SqlCommand(QUERY, conn);
                    cmd.Parameters.AddWithValue("@chosenSite", chosenSite);
                    cmd.Parameters.AddWithValue("@chosenName", chosenName);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.ExecuteNonQuery();
                    // TODO 2: Use @@IDENTITY
                    QUERY = @"SELECT @@Identity FROM reservation";
                    cmd = new SqlCommand(QUERY, conn);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reserving the site." + ex.Message);
                throw;
            }
        }

        public List<Reservation> ViewReservations(int parkId)
        {
            List<Reservation> reservations = new List<Reservation>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                const string QUERY = @"SELECT * FROM reservation r
    JOIN site s ON r.site_id = s.site_id
    JOIN campground c ON s.campground_id = c.campground_id
    JOIN park p ON c.park_id = p.park_id
    WHERE p.park_id = @parkId AND from_date <= GETDATE()+30 ORDER BY r.from_date";
                SqlCommand cmd = new SqlCommand(QUERY, conn);
                cmd.Parameters.AddWithValue("@parkId", parkId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Reservation reservation = new Reservation
                    {
                        Id = Convert.ToInt32(reader["reservation_id"]),
                        SiteId = Convert.ToInt32(reader["site_id"]),
                        Name = Convert.ToString(reader["name"]),
                        FromDate = Convert.ToDateTime(reader["from_date"]),
                        ToDate = Convert.ToDateTime(reader["to_date"]),
                        CreateDate = Convert.ToDateTime(reader["create_date"])
                    };
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }
    }
}
