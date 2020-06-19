using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    QUERY = @"SELECT MAX(reservation_id) FROM reservation";
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
    }
}
