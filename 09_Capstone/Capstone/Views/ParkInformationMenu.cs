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
            switch (choice)
            {
                case "1": // Do whatever option 1 is. You may prompt the user for more information
                          // (using the Helper methods), and then pass those values into some 
                          //business object to get something done.
                    Console.Clear();
                    PrintCampgrounds(campgroundSqlDAO.GetCampgrounds(park));
                    Console.WriteLine();
                    Console.WriteLine(@"Select a Command
    1) Search for Available Reservation
    2) Return to Previous Screen");
                    string input = Console.ReadLine();
                    if (input == "1")
                    {
                        ReservationProcess();
                    }
                    return true;    // Keep running the main menu
                case "2": // Do whatever option 2 is
                    ReservationProcess();
                    return true;    // Keep running the main menu
                //case "3":
                //    // Create and show the sub-menu
                //    return true;    // Keep running the main menu
            }
            return true;
        }

        private void ReservationProcess()
        {
            Console.Clear();
            PrintCampgrounds(campgroundSqlDAO.GetCampgrounds(park));
            MakeReservation();
            
        }

        private void MakeReservation()
        {
            int chosenCampground = GetInteger("Which campground (enter 0 to cancel)? ");
            if (chosenCampground == 0)
            {
                return;
            }
            DateTime chosenArrival = Convert.ToDateTime(GetString("What is the arrival date? (mm/dd/yyyy)"));
            DateTime chosenDeparture = Convert.ToDateTime(GetString("What is the departure date? (mm/dd/yyyy)"));
            if (chosenArrival >= chosenDeparture)
            {
                Console.WriteLine("Departure date must be after arrival date.");
                Pause("");
                return;
            }
            int lengthOfStay = (chosenDeparture - chosenArrival).Days;
            IList<Site> availableSites = siteSqlDAO.GetAvailableSites(chosenCampground, chosenArrival, chosenDeparture);
            PrintAvailableSites(availableSites, lengthOfStay);
            int chosenSite = GetInteger("Which site should be reserved (enter 0 to cancel)? ");
            string chosenName = GetString("Which name should the reservation be made under? ");
            int confirmationId = reservationSqlDAO.ReserveSite(chosenSite, chosenName, chosenArrival, chosenDeparture);
            Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}");
            Console.ReadLine();
        }

        private void PrintAvailableSites(IList<Site> availableSites, int lengthOfStay)
        {
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("{0,-10}{1,-15}{2,-15}{3,-15}{4,-10}{5,-10}", "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost");
            foreach(Site site in availableSites)
            {
                Console.WriteLine($"{site.SiteNumber,-10}{site.MaxOccupancy,-15}{site.IsAccessible,-15}{site.MaxRVLength,-15}{site.HasUtilities,-10}{site.Cost*lengthOfStay,-10}");
            }
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
