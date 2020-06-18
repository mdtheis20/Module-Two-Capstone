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
            //this.menuOptions.Add("1", parkSqlDAO.GetParks);
            //this.menuOptions.Add("2", "Ask the user for name");
            //this.menuOptions.Add("3", "Go to a sub-menu");
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
            Park park = GetChosenPark(parks, choice);
            Console.WriteLine(park);
            Console.WriteLine("");
            Console.WriteLine($@"Select a command
    1) View Campgrounds
    2) Search for Reservation
    3) Return to Previous Screen");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1": // Do whatever option 1 is. You may prompt the user for more information
                          // (using the Helper methods), and then pass those values into some 
                          //business object to get something done.
                    Console.Clear();
                    IList<Campground> campgrounds = campgroundSqlDAO.GetCampgrounds(park);
                    PrintCampgrounds(campgrounds);
                    Pause("");
                    int i1 = GetInteger("Enter the first integer: ");
                    int i2 = GetInteger("Enter the second integer: ");
                    Console.WriteLine($"{i1} + {i2} = {i1+i2}");
                    Pause("Press enter to continue");
                    return true;    // Keep running the main menu
                case "2": // Do whatever option 2 is
                    string name = GetString("What is your name?");
                    WriteError($"Not yet implemented, {name}.");
                    Pause("");
                    return true;    // Keep running the main menu
                case "3": 
                    // Create and show the sub-menu
                    SubMenu1 sm = new SubMenu1();
                    sm.Run();
                    return true;    // Keep running the main menu
            }
            return true;
        }

        private void PrintCampgrounds(IList<Campground> campgrounds)
        {
            Console.WriteLine("{0, -6}{1, -35}{2, -15}{3, -15}{4, 10}", " ", "Name", "Open", "Close", "Daily Fee");
            foreach(Campground campground in campgrounds)
            {
                Console.WriteLine($"#{campground.Id,-5}{campground.Name,-35}{campground.OpeningMonth,-15}{campground.ClosingMonth,-15}{campground.DailyFee:C2}");
            }
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
        protected Park GetChosenPark(IList<Park> parks, string choice)
        {
            foreach (Park park in parks)
            {
                if (park.Id == Convert.ToInt32(choice))
                {
                    return park;
                }
            }
            return null;
        }
    }
}
