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

        private string connectionString;

        public CapstoneProjectCLI(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }


        const string Command_GetAllParks = "1";
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

            ParkSqlDAL testClass = new ParkSqlDAL(connectionString);

            while (true)
            {
                string command = Console.ReadLine();
                Console.Clear();

                while(true)
                {
                    MainMenuText();
                    String userInput = Console.ReadLine();

                    if(userInput == "1")
                    {
                        testClass.GetAllParks();
                    }
                    if(userInput == "2")
                    {

                    }
                        
                }

               
                }
                PrintMenu();
            }

        private void MainMenuText()
        {
            throw new NotImplementedException();
        }

        private void PrintMenu()
        {
            Console.WriteLine("Select a park for further details");
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


