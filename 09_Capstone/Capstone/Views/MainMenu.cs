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
            switch (choice)
            {
                case "1": // Do whatever option 1 is. You may prompt the user for more information
                            // (using the Helper methods), and then pass those values into some 
                            //business object to get something done.
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
