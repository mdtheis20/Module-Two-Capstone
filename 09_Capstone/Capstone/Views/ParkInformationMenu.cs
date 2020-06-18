using Capstone.DAL;
using Capstone.Models;
using Capstone.Views;
using System;
using System.Collections.Generic;

namespace CLI
{
    /// <summary>
    /// A sub-menu 
    /// </summary>
    public class ParkInformationMenu : CLIMenu
    {
        // Store any private variables here....
        Park park;
        ParkSqlDAO parkSqlDAO;
        CampgroundSqlDAO campgroundSqlDAO;
        SiteSqlDAO siteSqlDAO;
        ReservationSqlDAO reservationSqlDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ParkInformationMenu(Park park, ParkSqlDAO parkSqlDAO, CampgroundSqlDAO campgroundSqlDAO, SiteSqlDAO siteSqlDAO, ReservationSqlDAO reservationSqlDAO) :
            base("Park Information")
        {
            this.park = park;
            this.parkSqlDAO = parkSqlDAO;
            this.campgroundSqlDAO = campgroundSqlDAO;
            this.siteSqlDAO = siteSqlDAO;
            this.reservationSqlDAO = reservationSqlDAO;
            // Store any values passed in....
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "View Campgrounds");
            this.menuOptions.Add("2", "Search for Reservation");
            this.menuOptions.Add("3", "Return to Previous Screen");
            this.quitKey = "3";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            ReservationMenu reservationMenu = new ReservationMenu();
            switch (choice)
            {
                case "1": // Do whatever option 1 is. You may prompt the user for more information
                          // (using the Helper methods), and then pass those values into some 
                          //business object to get something done.
                    Console.Clear();
                    IList<Campground> campgrounds = campgroundSqlDAO.GetCampgrounds(park);
                    PrintCampgrounds(campgrounds);
                    Console.WriteLine();
                    Console.WriteLine(@"Select a Command
    1) Search for Available Reservation
    2) Return to Previous Screen");
                    string input = Console.ReadLine();
                    if (input == "1")
                    {
                        GetReservationInfo();
                    }
                    return true;    // Keep running the main menu
                case "2": // Do whatever option 2 is
                    GetReservationInfo();
                    return true;    // Keep running the main menu
                //case "3":
                //    // Create and show the sub-menu
                //    return true;    // Keep running the main menu
            }
            return true;
        }

        private void GetReservationInfo()
        {
            int chosenCampground = GetInteger("Which campground (enter 0 to cancel)? ");
            if (chosenCampground == 0)
            {
                return;
            }
            DateTime chosenArrival = Convert.ToDateTime(GetString("What is the arrival date? "));
            DateTime chosenDeparture = Convert.ToDateTime(GetString("What is the departure date? "));
            int lengthOfStay = (chosenDeparture - chosenArrival).Days;
            IList<Site> availableSites = siteSqlDAO.GetAvailableSites(chosenCampground, chosenArrival, chosenDeparture);
            PrintReservationInfo(availableSites, lengthOfStay);
        }

        private void PrintReservationInfo(IList<Site> availableSites, int lengthOfStay)
        {
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("{0,-10}{1,-10}{2,-15}{3,-15}{4,-10}{5,-10}", "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost");
            foreach(Site site in availableSites)
            {
                Console.WriteLine($"{site.SiteNumber,-10}{site.MaxOccupancy,-10}{site.IsAccessible,-15}{site.MaxRVLength,-15}{site.HasUtilities,-10}{site.Cost*lengthOfStay,-10}");
            }
            Pause("");
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            //Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Park Information"));
            ResetColor();
            Console.WriteLine(park);
        }

    }
}
