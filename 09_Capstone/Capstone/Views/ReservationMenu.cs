using CLI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Views
{
    public class ReservationMenu : CLIMenu
    {
        // Store any private variables here....

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ReservationMenu(/** Variables may be passed in... ***/) :
            base("Reservation Menu")
        {
            // Store any values passed in....
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "Option 1");
            this.menuOptions.Add("2", "Do Option 2 and return to Main");
            this.menuOptions.Add("B", "Back to Main Menu");
            this.quitKey = "B";
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
                case "1": // Do whatever option 1 is
                    WriteError("Not yet implemented");
                    Pause("");
                    return true;
                case "2": // Do whatever option 2 is
                    WriteError("When this option is complete, we will exit this submenu by returning false from the ExecuteSelection method.");
                    Pause("");
                    return false;
            }
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Reservation Menu"));
            ResetColor();
        }

    }
}
