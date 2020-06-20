using Capstone.DAL;
using Capstone.Models;
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
        DateTime chosenArrival;
        DateTime chosenDeparture;

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
            this.menuOptions.Add("1", "Choose a Campground");
            this.menuOptions.Add("2", "Search for a Reservation");
            this.menuOptions.Add("3", "View all Reservations");
            this.menuOptions.Add("4", "Return to Previous Screen");
            this.quitKey = "4";
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
                case "1":
                    Console.Clear();
                    Campground chosenCampground = ChooseCampground();
                    if (chosenCampground == null)
                    {
                        return true;
                    }
                    IList<Site> availableSitesInCampground = siteSqlDAO.GetAvailableSitesAcrossCampground(chosenCampground.Id, chosenArrival, chosenDeparture);
                    if (availableSitesInCampground.Count == 0)
                    {
                        Console.WriteLine("There are no available sites matching your timeline. Please enter an alternate date range. ");
                        Pause("");
                        return true;
                    }
                    PrintAvailableSites(availableSitesInCampground, (chosenDeparture - chosenArrival).Days);
                    FinishReservation(availableSitesInCampground, chosenArrival, chosenDeparture);
                    return true;
                case "2":
                    Console.Clear();
                    GetArrivalAndDepartureDates();
                    IList<Site> availableSitesInPark = siteSqlDAO.GetAvailableSitesAcrossPark(park.Id, chosenArrival, chosenDeparture);
                    if (availableSitesInPark.Count == 0)
                    {
                        Console.WriteLine("There are no available sites matching your date range.");
                        Pause("");
                        return true;
                    }
                    PrintAvailableSites(availableSitesInPark, (chosenDeparture - chosenArrival).Days);
                    FinishReservation(availableSitesInPark, chosenArrival, chosenDeparture);
                    return true;
                case "3":
                    List<Reservation> reservations = reservationSqlDAO.ViewReservations(park.Id);
                    Console.WriteLine("{0,-4}{1,-8}{2,-40}{3,15}{4,15}", "Id","Site Id","Name","From Date","To Date");
                    foreach(Reservation reservation in reservations)
                    {
                        Console.WriteLine(reservation);
                    }
                    Pause("");
                    return true;
            }
            return true;
        }

        private void GetArrivalAndDepartureDates()
        {
            while (true)
            {
                chosenArrival = GetDate("What is the arrival date? (mm/dd/yyyy)");
                chosenDeparture = GetDate("What is the departure date? (mm/dd/yyyy)");
                if (chosenArrival >= chosenDeparture)
                {
                    Console.WriteLine("Departure date must be after arrival date.");
                    Pause("");
                    continue;
                }
                return;
            }
        }

        private Campground ChooseCampground()
        {
            Console.Clear();
            IList<Campground> campgrounds = campgroundSqlDAO.GetCampgrounds(park);
            PrintCampgrounds(campgrounds);
            Campground campground = GetChosenCampground(campgrounds);
            if (campground == null)
            {
                return null;
            }
            while (true)
            {
                GetArrivalAndDepartureDates();
                if (chosenArrival.Month < campground.OpeningMonth || chosenDeparture.Month > campground.ClosingMonth)
                {
                    Console.WriteLine("The campground is not open during those dates. ");
                    Pause("");
                    continue;
                }
                return campground;
            }
        }

        private Campground GetChosenCampground(IList<Campground> campgrounds)
        {
            while (true)
            {
                int chosenCampground = GetInteger("Which campground (enter 0 to cancel)? ");
                if (chosenCampground == 0)
                {
                    return null;
                }
                else
                {
                    foreach (Campground campground in campgrounds)
                    {
                        if (campground.Id == chosenCampground)
                        {
                            return campground;
                        }
                    }
                    Console.WriteLine("Please choose a campground from the list above.");
                    Pause("");
                }
            }
        }
        private void FinishReservation(IList<Site> availableSites, DateTime chosenArrival, DateTime chosenDeparture)
        {
            while (true)
            {
                int chosenSite = GetInteger("Which site should be reserved (enter 0 to cancel)? ");
                if (chosenSite == 0)
                {
                    return;
                }
                Dictionary<int, int> sitesToPrint = new Dictionary<int, int>();
                int i = 1;
                foreach (Site site in availableSites)
                {
                    sitesToPrint.Add(i, site.Id);
                    i++;
                }
                if (!sitesToPrint.ContainsKey(chosenSite))
                {
                    Console.WriteLine("Please choose a site from the list above.");
                    Pause("");
                    Console.Clear();
                    continue;
                }
                string chosenName = GetString("Which name should the reservation be made under? ");
                int confirmationId = reservationSqlDAO.ReserveSite(sitesToPrint[chosenSite], chosenName, chosenArrival, chosenDeparture);
                Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}");
                Console.ReadLine();
                break;
            }
            
        }

        private void PrintAvailableSites(IList<Site> availableSites, int lengthOfStay)
        {
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("{0,-15}{1,-10}{2,-15}{3,-15}{4,-15}{5,-10}{6,-10}", "Campground","Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost");
            int i = 1;
            foreach (Site site in availableSites)
            {
                Console.WriteLine($"{site.CampgroundName,-15}{i,-10}{site.MaxOccupancy,-15}{site.PrintIsAccessible,-15}{site.MaxRVLength,-15}{site.PrintHasUtilities,-10}{site.Cost * lengthOfStay:C2}");
                i++;
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
