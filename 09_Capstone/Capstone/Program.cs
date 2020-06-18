using Capstone.DAL;
using CLI;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");

            /********************************************************************
            // If you do not want to use CLIMenu, you can remove the following
            *********************************************************************/
            // Create any DAOs needed here, and then pass them into main menu...
            ParkSqlDAO parkSqlDAO = new ParkSqlDAO(connectionString);
            CampgroundSqlDAO campgroundSqlDAO = new CampgroundSqlDAO(connectionString);
            SiteSqlDAO siteSqlDAO = new SiteSqlDAO(connectionString);
            ReservationSqlDAO reservationSqlDAO = new ReservationSqlDAO(connectionString);


            MainMenu mainMenu = new MainMenu(parkSqlDAO, campgroundSqlDAO, siteSqlDAO, reservationSqlDAO);  // You'll probably be adding daos to the constructor

            // Run the menu.
            mainMenu.Run();
        }
    }
}
