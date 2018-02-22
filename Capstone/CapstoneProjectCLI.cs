using System;
using Capstone.Models;
using Capstone.DAL;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{


    public class CapstoneProjectCLI
    {

        public static string connection = @"Server=.\SqlExpress;Database=campground-tiny;Trusted_Connection=true";


        //const string Command_GetAllParks = "1";
        //const string Command_Acadia = "2";
        //const string Command_Arches = "3";
        //const string Command_CuyahogaNationalValleyPark = "4";
        //const string Command_Quit = "Q";

        public void RunCLI()
        {
            PrintHeader();
            PrintParks();

            // Prompt the user for a park
            Console.WriteLine("Please select an option");

            // Ask them to select one of the parks from the menu.
            int userSelection = int.Parse(Console.ReadLine());

            // Get the park name that goes with their selection
            ParkSqlDAL dal = new ParkSqlDAL(connection);
            List<Park> parks = dal.GetAllParks();
            Park selectedPark = parks[userSelection];

            // Display Information for the Park

            // Display Campgrounds for that park
            DisplayAllCampgroundsInPark(selectedPark.Name);



            //while (true)
            //{
                //Console.WriteLine("Please select an option");

                //string userInput = int.TryParse(Console.ReadLine(), out int park_id);

                

                //if (userInput == "1")
                //{
                //    GetAllCampgroundsInPark();
                //}
                //else if (userInput == "Q")
                //{
                //    break;
                //}
            //}



        }
        private void DisplayAllCampgroundsInPark(string parkName)
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connection);
            List<Campground> campgroundSites = dal.GetAllCampgroundsInPark(parkName);

            for (int i = 0; i < campgroundSites.Count; i++)
            {
                Console.WriteLine(i + " " + campgroundSites[i].Name);
            }
        }

        private void MainMenuText()
        {
            throw new NotImplementedException();

        }
        private void PrintParks()
        {
            Console.WriteLine("Please select one of the following option.");
            //Console.WriteLine("1.Acadia");
            //Console.WriteLine("2.Arches");
            //Console.WriteLine("3.Cuyahoga National Valley Park");
            ParkSqlDAL dal = new ParkSqlDAL(connection);
            List<Park> park = dal.GetAllParks();
            for(int i = 0; i < park.Count; i++)
            {
                Console.WriteLine(i+ " " + park[i].Name);
                
            }
        }
        private void PrintHeader()
        {


            Console.WriteLine(@"__    __              __      __                                __        _______                      __             ");
            Console.WriteLine(@"         /  \  /  |            /  |    /  |                              /  |      /       \                    /  |  ");
            Console.WriteLine(@"$$  \ $$ |  ______   _$$ |_   $$/   ______   _______    ______  $$ |      $$$$$$$  | ______    ______  $$ |   __   _______");
            Console.WriteLine(@"$$$  \$$ | /      \ / $$   |  /  | /      \ /       \  /      \ $$ |      $$ |__$$ |/      \  /      \ $$ |  /  | /       |");
            Console.WriteLine(@"$$$$  $$ | $$$$$$  |$$$$$$/   $$ |/$$$$$$  |$$$$$$$  | $$$$$$  |$$ |      $$    $$/ $$$$$$  |/$$$$$$  |$$ |_/$$/ /$$$$$$$/ ");
            Console.WriteLine(@"$$ $$ $$ | /    $$ |  $$ | __ $$ |$$ |  $$ |$$ |  $$ | /    $$ |$$ |      $$$$$$$/  /    $$ |$$ |  $$/ $$   $$<  $$      \ ");
            Console.WriteLine(@"$$ |$$$$ |/$$$$$$$ |  $$ |/  |$$ |$$ \__$$ |$$ |  $$ |/$$$$$$$ |$$ |      $$ |     /$$$$$$$ |$$ |      $$$$$$  \  $$$$$$  |");
            Console.WriteLine(@"$$ | $$$ |$$    $$ |  $$  $$/ $$ |$$    $$/ $$ |  $$ |$$    $$ |$$ |      $$ |     $$    $$ |$$ |      $$ | $$  |/     $$/ ");
            Console.WriteLine(@"$$/   $$/  $$$$$$$/    $$$$/  $$/  $$$$$$/  $$/   $$/  $$$$$$$/ $$/       $$/       $$$$$$$/ $$/       $$/   $$/ $$$$$$$/  ");
            Console.WriteLine();

        }
    }










}









