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
        
        public void RunCLI()
        {
            try
            {
                PrintHeader();
                PrintParks();
                
                Console.WriteLine("Please select an option");

                int userSelection = int.Parse(Console.ReadLine());

                if (userSelection == 0)
                {
                    Console.WriteLine("Thank you for using National Park Reservation Program!");
                    return;
                }
                while(userSelection > 3 || userSelection < 0)
                {
                    Console.WriteLine("Please try again.");
                    userSelection = int.Parse(Console.ReadLine());
                }

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
                PrintParkInfo(selectedPark);

                PrintCampgroundMenu();
                int parkMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");
                while(parkMenuChoice < 0 || parkMenuChoice > 3)
                {
                    Console.WriteLine("Invalid input");
                    parkMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");
                }
                
                CampgroundSqlDAL campDAL = new CampgroundSqlDAL(connection);
                List<Campground> campgrounds = campDAL.GetAllCampgroundsInPark(selectedPark.Name);
                if (parkMenuChoice == 1)
                {
                    DisplayAllCampgroundsInPark(selectedPark.Name);
                    
                    Console.WriteLine("\n \n");
                    Console.WriteLine("1) Search for available reservation");
                    Console.WriteLine("2) Return to previous menu");
                    int campgroundMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");

                    if(campgroundMenuChoice == 2)
                    {
                        Console.Clear();
                        RunCLI();
                    }
                    parkMenuChoice = 2;
                }
                if (parkMenuChoice == 2)
                {
                    int campgroundIndex = 0;
                    DateTime startDate;
                    DateTime endDate;
                    
                    DisplayAllCampgroundsInPark(selectedPark.Name);
                    
                    campgroundIndex = CLIHelper.GetInteger("\nWhich campground (enter 0 to cancel)?  ___ ") - 1;
                    while(campgroundIndex < -1 || campgroundIndex > 2)
                    {
                        Console.WriteLine("Invalid input");
                        DisplayAllCampgroundsInPark(selectedPark.Name);
                        campgroundIndex = CLIHelper.GetInteger("\nWhich campground (enter 0 to cancel)?  ___ ") - 1;
                    }
                    if(campgroundIndex == -1)
                    {
                        RunCLI();
                    }
                    startDate = CLIHelper.GetDateTime("What is the arrival date?  dd/mm/yyyy ");
                    endDate = CLIHelper.GetDateTime("What is the departure date?  dd/mm/yyyy ");
                    TimeSpan lengthOfStay = endDate.Subtract(startDate);
                    int stayLength = (int)lengthOfStay.TotalDays;

                    SiteSqlDAL siteDAL = new SiteSqlDAL(connection);
                    List<Campsite> availableCampsites = siteDAL.GetAvailableSitesInCampground(campgrounds[campgroundIndex].CampgroundId,Convert.ToDateTime(startDate),Convert.ToDateTime(endDate));

                    if(stayLength < 1)
                    {
                        Console.WriteLine("Invalid length of stay");
                        startDate = CLIHelper.GetDateTime("What is the arrival date?  dd/mm/yyyy ");
                        endDate = CLIHelper.GetDateTime("What is the departure date?  dd/mm/yyyy ");

                    }
                    Console.Write("".PadRight(75, '_'));
                    Console.WriteLine("\nRESULTS MATCHING YOUR SEARCH CRITERIA");
                    Console.WriteLine("Available Camp Sites at " + selectedPark + " National Park, " + campgrounds[campgroundIndex].Name + " Campground");
                    Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(12) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15)
                        + "Utility".PadRight(10) + "Cost");
                    if (availableCampsites.Count < 1)
                    {
                        Console.WriteLine("No available campsites");
                        RunCLI();
                    }

                    int count = 0;
                    foreach (Campsite site in availableCampsites)
                    {
                        Console.WriteLine(count+ site.Site_Number + " ".PadRight(10)+site.Max_Occupancy+" ".PadRight(10)+site.Accessible+" ".PadRight(10)+site.Max_Rv_Length+" ".PadRight(13)+site.Utilities+" ".PadRight(7)+ (campgrounds[campgroundIndex].Daily_Fee * stayLength));
                        count++;
                    }

                    Console.WriteLine("Which site to reserve? ");
                    int siteMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");
                    if(siteMenuChoice < 1 || siteMenuChoice > availableCampsites.Count)
                    {
                        Console.WriteLine("Invalid Choice");
                        siteMenuChoice = CLIHelper.GetInteger("\nSELECT:  ");
                    }
                   
                    Console.WriteLine("What name should the reservation be under? ");
                    string reservationNameChoice = CLIHelper.GetString("\nSELECT:  ");
                    
                    ReservationSqlDAL resDAL = new ReservationSqlDAL(connection);
                     int reservationId = resDAL.CreateNewReservation(siteMenuChoice, reservationNameChoice, startDate, endDate,Convert.ToString(DateTime.Now));

                    Console.WriteLine($"The reservation has been made and the confirmation id is {reservationId}");
                }
                
                
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error has occurred: " + ex.Message);
                throw;
            }
            catch(FormatException ex)
            {
                Console.WriteLine("Incorrect input: "+ ex.Message);
                RunCLI();
                
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
            Console.WriteLine("Please select one of the following option (press 0 to exit).");
            
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
        private void PrintParkInfo(Park selectedPark)
        {
            Console.WriteLine(selectedPark.Name + " National Park");
            Console.WriteLine("______________________________________________");
            Console.WriteLine("Location:".PadRight(20) + selectedPark.Location);
            Console.WriteLine("Established:".PadRight(20) + selectedPark.Establish_Date);
            Console.WriteLine("Area:".PadRight(20) + string.Format("{0:n0}", selectedPark.Area) + " sq km");
            Console.WriteLine("Annual Visitors:".PadRight(20) + string.Format("{0:n0}", selectedPark.Visitors));
            Console.WriteLine("\n" + selectedPark.Description + "\n");
            Console.WriteLine();
            Console.WriteLine();

        }
        private void PrintCampgroundMenu()
        {
            Console.WriteLine("Select a Command");
            Console.WriteLine("".PadRight(5) + "1) View Campgrounds");
            Console.WriteLine("".PadRight(5) + "2) Search for Reservation");
            Console.WriteLine("".PadRight(5) + "3) Return to Previous Screen");
        }
    }










}









