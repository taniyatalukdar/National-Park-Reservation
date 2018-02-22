using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
     public class CapstoneProjectCLI
    {
        
        const string Command_Acadia = "1";
        const string Command_Arches = "2";
        const string Command_CuyahogaNationalPark = "3";
        const string Command_Quit = "Q";
        //const string Command_ProjectList = "5";
        //const string Command_CreateDepartment = "6";
        //const string Command_UpdateDepartment = "7";
        //const string Command_CreateProject = "8";
        //const string Command_AssignEmployeeToProject = "9";
        //const string Command_RemoveEmployeeFromProject = "10";
        //const string Command_Quit = "q";

        public void RunCLI()
        {
            PrintHeader();
            PrintMenu();

            while (true)
            {
                string command = Console.ReadLine();
                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_Acadia:
                        Acadia();
                        break;

                    case Command_Arches:
                        Arches();
                        break;

                    case Command_CuyahogaNationalPark:
                        CuyahogaNationalPark();
                        break;

                    case Command_Quit:
                        Console.WriteLine("Thank you for using campground tiny");
                        break;

                    default:
                        Console.WriteLine("Invalid command, please try again!");
                        break;

                }
                PrintMenu();
            }
        }

        private void CuyahogaNationalPark()
        {
            throw new NotImplementedException();
        }

        private void Arches()
        {
            throw new NotImplementedException();
        }

        private void Acadia()
        {
            throw new NotImplementedException();
        }

        private void PrintMenu()
        {
            Console.WriteLine("Select a park for further details");
        }

        private void PrintHeader()
        {
            Console.WriteLine("View Park Interface");
        }


    }

    
}
