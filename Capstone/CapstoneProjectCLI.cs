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

        public Park selectedPark;
        //const string Command_GetAllParks = "1";
        //const string Command_Acadia = "2";
        //const string Command_Arches = "3";
        //const string Command_CuyahogaNationalValleyPark = "4";
        //const string Command_Quit = "Q";

        public void RunCLI()
        {
            try
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
                Park selectedPark = new Park(); 
                foreach (Park park in parks)
                {
                    if (userSelection == park.Id)
                    {
                        selectedPark = park;
                    }
                }

                Console.WriteLine(selectedPark.Name + " National Park");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("Location:".PadRight(20) + selectedPark.Location);
                Console.WriteLine("Established:".PadRight(20) + selectedPark.Establish_Date);
                Console.WriteLine("Area:".PadRight(20) + string.Format("{0:n0}", selectedPark.Area) + " sq km");
                Console.WriteLine("Annual Visitors:".PadRight(20) + string.Format("{0:n0}", selectedPark.Visitors));
                Console.WriteLine("\n" + selectedPark.Description + "\n");
                Console.WriteLine();
                Console.WriteLine();


                // Display Campgrounds for that park

                Console.WriteLine("Select a Command");
                Console.WriteLine("".PadRight(5) + "1) View Campgrounds");
                Console.WriteLine("".PadRight(5) + "2) Search for Reservation");
                Console.WriteLine("".PadRight(5) + "3) Return to Previous Screen");
                int parkMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");


                CampgroundSqlDAL campDAL = new CampgroundSqlDAL(connection);
                List<Campground> campgrounds = campDAL.GetAllCampgroundsInPark(selectedPark.Name);
                if (parkMenuChoice == 1)
                {
                    DisplayAllCampgroundsInPark(selectedPark.Name);
                    
                    Console.WriteLine("\n \n");
                    Console.WriteLine("1) Search for available reservation");
                    Console.WriteLine("2) Return to previous menu");
                    int campgroundMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");

                    parkMenuChoice = 2;
                    
                }
                if (parkMenuChoice == 2)
                {
                    

                    int campgroundIndex = 0;
                    string startDate = "";
                    string endDate = "";

                    DisplayAllCampgroundsInPark(selectedPark.Name);
                    
                    campgroundIndex = CLIHelper.GetInteger("\nWhich campground (enter 0 to cancel)?  ___ ") - 1;
                    startDate = CLIHelper.GetString("What is the arrival date?  dd/mm/yyyy ");
                    endDate = CLIHelper.GetString("What is the departure date?  dd/mm/yyyy ");

                    SiteSqlDAL siteDAL = new SiteSqlDAL(connection);
                    List<Campsite> availableCampsites = siteDAL.GetAvailableSitesInCampground(campgrounds[campgroundIndex].Name,Convert.ToDateTime(startDate),Convert.ToDateTime(endDate));

                    Console.Write("".PadRight(75, '_'));
                    Console.WriteLine("\nRESULTS MATCHING YOUR SEARCH CRITERIA");
                    Console.WriteLine("Available Camp Sites at " + selectedPark + " National Park, " + campgrounds[campgroundIndex].Name + " Campground");
                    Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(12) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15)
                        + "Utility".PadRight(10) + "Cost");
                    foreach (Campsite site in availableCampsites)
                    {
                        Console.WriteLine(site.Site_Number + " ".PadRight(10)+site.Max_Occupancy+" ".PadRight(10)+site.Accessible+" ".PadRight(10)+site.Max_Rv_Length+" ".PadRight(10)+site.Utilities+" ".PadRight(10)+ campgrounds[campgroundIndex].Daily_Fee);
                    }
                    Console.WriteLine("Which site to reserve? ");
                    int siteMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");
                    Console.WriteLine("What name should the reservation be under? ");
                    string reservationNameChoice = CLIHelper.GetString("\nSELECT:  ");

                    ReservationSqlDAL resDAL = new ReservationSqlDAL(connection);
                     int reservationId = resDAL.CreateNewReservation(reservationNameChoice, startDate, endDate, siteMenuChoice);
                    
                    Console.WriteLine($"The reservation has been made and the confirmation id is {reservationId}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error has occurred: " + ex.Message);
                throw;
            }
        }


        private void DisplayAllCampgroundsInPark(string parkName)
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connection);
            List<Campground> campground = dal.GetAllCampgroundsInPark(parkName);

            Console.WriteLine(parkName + " National Park");
            Console.WriteLine("");

            Console.WriteLine("  " + "Name".PadRight(25) + "Open from".PadRight(12) + "Open til".PadRight(12) + "Price");
            Console.WriteLine("_________________________________________________________________");
            for (int i = 0; i < campground.Count; i++)
            {
                Console.WriteLine((i + 1) + " " + campground[i].Name.PadRight(25) + "  " + campground[i].Open_From_MM + "  ".PadRight(10) + campground[i].Open_From_MM + "    ".PadRight(10) + campground[i].Daily_Fee);

            }
        }

        private void PrintParks()
        {
            Console.WriteLine("Please select one of the following option.");
            
            ParkSqlDAL dal = new ParkSqlDAL(connection);
            List<Park> parks = dal.GetAllParks();

            int count = 1;
            foreach (Park park in parks)
            {
                Console.WriteLine(count + " " + park.Name);
                count++;
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









