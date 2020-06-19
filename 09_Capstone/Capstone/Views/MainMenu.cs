using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace CLI
{
    /// <summary>
    /// The top-level menu in our application
    /// </summary>
    public class MainMenu : CLIMenu
    {
        private ParkSqlDAO parkSqlDAO;
        private CampgroundSqlDAO campgroundSqlDAO;
        private SiteSqlDAO siteSqlDAO;
        private ReservationSqlDAO reservationSqlDAO;
        // You may want to store some private variables here.  YOu may want those passed in 
        // in the constructor of this menu

        /// <summary>
        /// Constructor adds items to the top-level menu. You will likely have parameters  passed in
        /// here...
        /// </summary>
        public MainMenu(ParkSqlDAO parkSqlDAO, CampgroundSqlDAO campgroundSqlDAO, SiteSqlDAO siteSqlDAO, ReservationSqlDAO reservationSqlDAO
            ) : base("Main Menu")
        {
            this.parkSqlDAO = parkSqlDAO;
            this.campgroundSqlDAO = campgroundSqlDAO;
            this.siteSqlDAO = siteSqlDAO;
            this.reservationSqlDAO = reservationSqlDAO;
        }

        protected override void SetMenuOptions()
        {
            // A Sample menu.  Build the dictionary here
            IList<Park> parks = parkSqlDAO.GetParks();
            int menuOption = 1;
            foreach (Park park in parks)
            {
                this.menuOptions.Add(menuOption.ToString(), park.Name);
                menuOption++;
            }
            this.menuOptions.Add("Q", "Quit program");
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            IList<Park> parks = parkSqlDAO.GetParks();
            Park park = parks[int.Parse(choice) - 1];
            ParkInformationMenu submenu = new ParkInformationMenu(park, parkSqlDAO, campgroundSqlDAO, siteSqlDAO, reservationSqlDAO);
            submenu.Run();
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Main Menu"));
            ResetColor();
        }
    }
}
