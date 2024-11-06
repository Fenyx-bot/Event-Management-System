using System;

namespace EventManager
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Manager.LoadEvents(); //LOADING EVENTS FROM THE FILES
            while (true) //MAIN LOOP
            {
                MainMenu();
                Console.Clear();
            }
        }


        public static void MainMenu()
        {
            //TITLE AND CONTROLS
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("| Welcome to the Event Management System! |");

            //Checking for the closest event

            if(Manager.AllEvents.Count > 0)
            {
                var timeDiff = Manager.AllEvents[0].Date - DateTime.Now;
                string closestEventTime;
                if (timeDiff.Days > 30)
                {
                    closestEventTime = $"on {Manager.AllEvents[0].Date.Day}-{Manager.AllEvents[0].Date.Month}-{Manager.AllEvents[0].Date.Year}";
                }
                else
                {
                    closestEventTime = timeDiff.Days > 0 ? $"in {timeDiff.Days} {((timeDiff.Days == 1) ? "Day" : "Days")}" : $"Today";
                }

                Console.WriteLine($"| {Manager.AllEvents[0].Name} is {closestEventTime} |");
            }
            
            Console.WriteLine("-------------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("| Use \u001b[32m[UP]\u001b[36m and \u001b[32m[DOWN]\u001b[36m to navigate | Press \u001b[32m[ENTER]\u001b[36m to select |");
            Console.WriteLine("-------------------------------------------------------------");
            Console.ResetColor();


            string selectedCursor = " \u001b[32m-> ";
            bool enterPressed = false;


            Console.WriteLine($"{(Manager.choiceIndex == 0 ? selectedCursor : " .  ")}Create an Event\u001b[0m");
            Console.WriteLine($"{(Manager.choiceIndex == 1 ? selectedCursor : " .  ")}Get all events\u001b[0m");
            Console.WriteLine($"{(Manager.choiceIndex == 2 ? selectedCursor : " .  ")}Get an event by ID\u001b[0m");
            Console.WriteLine($"{(Manager.choiceIndex == 3 ? selectedCursor : " .  ")}Exit\u001b[0m");


            //CHECKING AND HANDELING INPUT
            var keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case System.ConsoleKey.UpArrow:
                    Manager.choiceIndex = (Manager.choiceIndex == 0 ? Manager.choiceIndex = 3 : Manager.choiceIndex - 1);
                    break;
                case System.ConsoleKey.DownArrow:
                    Manager.choiceIndex = (Manager.choiceIndex == 3 ? Manager.choiceIndex = 0 : Manager.choiceIndex + 1);
                    break;
                case System.ConsoleKey.Enter:
                    enterPressed = true;
                    break;
            }

            //HANDLING ENTER PRESS
            if (enterPressed)
            {
                switch (Manager.choiceIndex)
                {
                    case 0:
                        CreateEvent();
                        break;
                    case 1:
                        GetAllEvents();
                        break;
                    case 2:
                        GetEventByID();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }

            enterPressed = false;
        }

        public static void GetAllEvents()
        {
            Manager.choiceIndex = 0;

            Manager.pageIndex = 0;
            int totalPages = (int)MathF.Ceiling((float)Manager.AllEvents.Count / 3);


            while (true)
            {
                Console.Clear();

                //TITLE AND CONTROLS
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("------------------------------");
                Console.WriteLine("| Listing future events! |");
                Console.WriteLine($"| Page {Manager.pageIndex + 1} out of {((Manager.AllEvents.Count > 0) ? totalPages : 1)} |");
                Console.WriteLine("------------------------------");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("| Use \u001b[32m[UP]\u001b[36m and \u001b[32m[DOWN]\u001b[36m to navigate between events | Use \u001b[32m[LEFT]\u001b[36m and \u001b[32m[RIGHT]\u001b[36m to navigate between pages");
                Console.WriteLine("| Press \u001b[32m[A]\u001b[36m to see archived events | Press \u001b[32m[BACKSPACE]\u001b[36m to return");
                Console.WriteLine("| Press \u001b[32m[D]\u001b[36m to delete an event | Press \u001b[32m[U]\u001b[36m to update an event |");
                Console.WriteLine("----------------------------------------------------------------------------------------------------\n\u001b[0m~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ResetColor();

                //LISTING THE EVENTS

                int currentPageEvents = 0;

                if (Manager.AllEvents.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" ---------------------------------------------------------------------------------------------------");
                    Console.WriteLine($" | No Events Found |");
                    Console.WriteLine($" ---------------------------------------------------------------------------------------------------");
                    Console.ResetColor();
                }
                else
                {
                    int x = Manager.pageIndex * 3;
                    int y = (x + 3 > Manager.AllEvents.Count) ? Manager.AllEvents.Count : x + 3;

                    for (int i = Manager.pageIndex * 3; i < y; i++)
                    {
                        Event e = Manager.AllEvents[i];
                        Console.WriteLine($"{(Manager.choiceIndex == Manager.AllEvents.IndexOf(e) ? "\u001b[32m" : "")} ---------------------------------------------------------------------------------------------------");
                        Console.WriteLine($" | ID - {e.ID} | NAME - {e.Name} | DATE - {e.Date.Day}-{e.Date.Month}-{e.Date.Year} |\n | LOCATION - {e.Location} \n | DESCRIPTION - {e.Description}");
                        Console.WriteLine($" ---------------------------------------------------------------------------------------------------\u001b[0m");
                        currentPageEvents++; //CHECKING HOW MANY EVENTS WE HAVE IN A PAGE
                    }
                }

                //CHECKING AND HANDELING INPUT
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case System.ConsoleKey.UpArrow:
                        //CHECKING IF WE ARE AT THE FIRST EVENT OF THE PAGE AND WE STILL TRY TO GO UP
                        //THEN WE WILL JUMP TO THE LAST EVENT OF THE PAGE
                        //I USED [currentPageEvents] TO TELL HOW MANY EVENTS WE HAVE IN A PAGE
                        Manager.choiceIndex = (Manager.choiceIndex == Manager.pageIndex * 3 ? Manager.pageIndex * 3 + currentPageEvents - 1 : Manager.choiceIndex - 1);
                        break;
                    case System.ConsoleKey.DownArrow:
                        //CHECKING IF WE ARE AT THE LAST EVENT OF THE PAGE AND WE STILL TRY TO GO DOWN
                        //THEN WE WILL JUMP TO THE FIRST EVENT OF THE PAGE
                        //I USED [currentPageEvents] TO TELL HOW MANY EVENTS WE HAVE IN A PAGE
                        Manager.choiceIndex = (Manager.choiceIndex == Manager.pageIndex * 3 + currentPageEvents - 1 ? Manager.pageIndex * 3 : Manager.choiceIndex + 1);
                        break;
                    case System.ConsoleKey.RightArrow:
                        //CHECKING IF WE ARE AT THE LAST PAGE AND WE STILL TRY TO GO RIGHT
                        //THEN WE WILL JUMP TO THE FIRST PAGE
                        Manager.pageIndex = (Manager.pageIndex == totalPages - 1 ? 0 : Manager.pageIndex + 1);
                        Manager.choiceIndex = Manager.pageIndex * 3;
                        break;
                    case System.ConsoleKey.LeftArrow:
                        //CHECKING IF WE ARE AT THE FIRST PAGE AND WE STILL TRY TO GO LEFT
                        //THEN WE WILL JUMP TO THE LAST PAGE
                        Manager.pageIndex = (Manager.pageIndex == 0 ? totalPages - 1 : Manager.pageIndex - 1);
                        Manager.choiceIndex = Manager.pageIndex * 3;
                        break;
                    case System.ConsoleKey.D:
                        //DELETING THE CURRENTLY SELECTED EVENT
                        Manager.DeleteEvent(Manager.choiceIndex);
                        break;
                    case System.ConsoleKey.A:
                        //SWITCHING TO ARCHIVED EVENTS
                        Manager.choiceIndex = 0; GetArchivedEvents();
                        break;
                    case System.ConsoleKey.U:
                        //UPDATING THE CURRENTLY SELECTED EVENT

                        //CHECKING IF HAVE AN EVENT TO UPDATE
                        if (Manager.AllEvents.Count > Manager.choiceIndex)
                        {
                            UpdateEvent(Manager.AllEvents[Manager.choiceIndex]);
                        }

                        break;
                    case System.ConsoleKey.Backspace:
                        //GOING BACK TO THE MAIN MENU
                        Manager.choiceIndex = 0; return;
                }
            }
        }

        public static void GetArchivedEvents()
        {
            Manager.choiceIndex = 0;

            Manager.pageIndex = 0;
            int totalPages = (int)MathF.Ceiling((float)Manager.OldEvents.Count / 3);


            while (true)
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J"); //Clearing the console

                //TITLE AND CONTROLS
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("------------------------------");
                Console.WriteLine("| Listing Archived events! |");
                Console.WriteLine($"| Page {Manager.pageIndex + 1} out of {((Manager.OldEvents.Count > 0) ? totalPages : 1)} |");
                Console.WriteLine("------------------------------");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("| Use \u001b[32m[UP]\u001b[36m and \u001b[32m[DOWN]\u001b[36m to navigate between events | Use \u001b[32m[LEFT]\u001b[36m and \u001b[32m[RIGHT]\u001b[36m to navigate between pages");
                Console.WriteLine("| Press \u001b[32m[BACKSPACE]\u001b[36m to return | Press \u001b[32m[D]\u001b[36m to delete an event |");
                Console.WriteLine("----------------------------------------------------------------------------------------------------\n\u001b[0m~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ResetColor();

                //LISTING THE EVENTS
                int currentPageEvents = 0;

                if (Manager.OldEvents.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" ---------------------------------------------------------------------------------------------------");
                    Console.WriteLine($" | No Events Found |");
                    Console.WriteLine($" ---------------------------------------------------------------------------------------------------");
                    Console.ResetColor();
                }
                else
                {
                    int x = Manager.pageIndex * 3;
                    int y = (x + 3 > Manager.OldEvents.Count) ? Manager.OldEvents.Count : x + 3;



                    for (int i = Manager.pageIndex * 3; i < y; i++)
                    {
                        Event e = Manager.OldEvents[i];
                        Console.WriteLine($"{(Manager.choiceIndex == Manager.OldEvents.IndexOf(e) ? "\u001b[32m" : "")} ---------------------------------------------------------------------------------------------------");
                        Console.WriteLine($" | ID - {e.ID} | NAME - {e.Name} | DATE - {e.Date.Day}-{e.Date.Month}-{e.Date.Year} |\n | LOCATION - {e.Location} \n | DESCRIPTION - {e.Description}");
                        Console.WriteLine($" ---------------------------------------------------------------------------------------------------\u001b[0m");
                        currentPageEvents++; //CHECKING HOW MANY EVENTS WE HAVE IN A PAGE
                    }
                }

                //CHECKING AND HANDELING INPUT
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case System.ConsoleKey.UpArrow:
                        //CHECKING IF WE ARE AT THE FIRST EVENT OF THE PAGE AND WE STILL TRY TO GO UP
                        //THEN WE WILL JUMP TO THE LAST EVENT OF THE PAGE
                        //I USED [currentPageEvents] TO TELL HOW MANY EVENTS WE HAVE IN A PAGE
                        Manager.choiceIndex = (Manager.choiceIndex == Manager.pageIndex * 3 ? Manager.pageIndex * 3 + currentPageEvents - 1 : Manager.choiceIndex - 1);
                        break;
                    case System.ConsoleKey.DownArrow:
                        //CHECKING IF WE ARE AT THE LAST EVENT OF THE PAGE AND WE STILL TRY TO GO DOWN
                        //THEN WE WILL JUMP TO THE FIRST EVENT OF THE PAGE
                        //I USED [currentPageEvents] TO TELL HOW MANY EVENTS WE HAVE IN A PAGE
                        Manager.choiceIndex = (Manager.choiceIndex == Manager.pageIndex * 3 + currentPageEvents - 1 ? Manager.pageIndex * 3 : Manager.choiceIndex + 1);
                        break;
                    case System.ConsoleKey.RightArrow:
                        //CHECKING IF WE ARE AT THE LAST PAGE AND WE STILL TRY TO GO RIGHT
                        //THEN WE WILL JUMP TO THE FIRST PAGE
                        Manager.pageIndex = (Manager.pageIndex == totalPages - 1 ? 0 : Manager.pageIndex + 1);
                        Manager.choiceIndex = Manager.pageIndex * 3;
                        break;
                    case System.ConsoleKey.LeftArrow:
                        //CHECKING IF WE ARE AT THE FIRST PAGE AND WE STILL TRY TO GO LEFT
                        //THEN WE WILL JUMP TO THE LAST PAGE
                        Manager.pageIndex = (Manager.pageIndex == 0 ? totalPages - 1 : Manager.pageIndex - 1);
                        Manager.choiceIndex = Manager.pageIndex * 3;
                        break;
                    case System.ConsoleKey.D:
                        //DELETING THE CURRENTLY SELECTED EVENT
                        Manager.DeleteEvent(Manager.choiceIndex, true);
                        break;
                    //GOING BACK TO THE MAIN MENU
                    case System.ConsoleKey.Backspace:
                        Manager.choiceIndex = 0; return;
                }
            }
        }

        public static void GetEventByID()
        {
            //EVENT ID
            string id = "";
            while (true)
            {
                Console.Clear();

                //TIP
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("| TIP - You can write \u001b[32m[CANCEL]\u001b[36m anytime to go back! |");
                Console.WriteLine("----------------------------------------------------");

                //QUESTION
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("-------------------------------");
                Console.WriteLine("| Enter the ID of the event |");
                Console.WriteLine("-------------------------------");
                Console.ResetColor();


                Console.Write(">> ");
                id = Console.ReadLine() ?? "";

                //CHECKING IF USER WANTS TO CANCEL
                if (id.ToUpper() == "CANCEL")
                {
                    return;
                }

                //CHECKING IF THE ID IS VALID
                if (int.TryParse(id, out int result))
                {
                    if (Manager.AllEvents.Exists(x => x.ID == result))
                    {
                        Manager.choiceIndex = Manager.AllEvents.FindIndex(x => x.ID == result);
                        break;
                    }
                    else
                    {
                        //ERROR HANDLING
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" >> ERROR: ID not found. Press any key to try again... <<");
                        Console.ReadKey();
                    }
                }

            }

            while (true)
            {
                Console.Clear();

                //TITLE AND CONTROLS
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("------------------------------");
                Console.WriteLine("| Listing a specific event! |");
                Console.WriteLine("------------------------------");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("| Press \u001b[32m[D]\u001b[36m to delete an event | Press \u001b[32m[U]\u001b[36m to update an event |");
                                Console.WriteLine("Press \u001b[32m[BACKSPACE]\u001b[36m to return");
                Console.WriteLine("----------------------------------------------------------------------------------------------------\n\u001b[0m~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.ResetColor();

                Event e = Manager.AllEvents[Manager.choiceIndex];
                Console.WriteLine($"{(Manager.choiceIndex == Manager.AllEvents.IndexOf(e) ? "\u001b[32m" : "")} ---------------------------------------------------------------------------------------------------");
                Console.WriteLine($" | ID - {e.ID} | NAME - {e.Name} | DATE - {e.Date.Day}-{e.Date.Month}-{e.Date.Year} |\n | LOCATION - {e.Location} \n | DESCRIPTION - {e.Description}");
                Console.WriteLine($" ---------------------------------------------------------------------------------------------------\u001b[0m");

                //CHECKING AND HANDELING INPUT
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case System.ConsoleKey.D:
                        //DELETING THE EVENT
                        Manager.DeleteEvent(Manager.choiceIndex);
                        Manager.choiceIndex = 0;
                        return;
                    case System.ConsoleKey.U:
                        //UPDATING THE EVENT
                        UpdateEvent(e);
                        break;
                    //GOING BACK TO THE MAIN MENU
                    case System.ConsoleKey.Backspace:
                        Manager.choiceIndex = 0; return;
                }
            }
        }

        public static void UpdateEvent(Event e)
        {

            //EVENT NAME
            string name = "";
            Console.Clear();

            //TIP
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("| TIP - You can write \u001b[32m[CANCEL]\u001b[36m anytime to go back! |");
            Console.WriteLine(" | [LEAVE EMPTY IF YOU DON'T WANT TO CHANGE IT] | ");
            Console.WriteLine("----------------------------------------------------");

            //QUESTION
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-------------------------------");
            Console.WriteLine("| Enter the name of the event|");
            Console.WriteLine("-------------------------------");
            Console.ResetColor();


            Console.Write(">> ");
            name = Console.ReadLine() ?? "";


            //CHECKING IF USER WANTS TO CANCEL
            if (name.ToUpper() == "CANCEL")
            {
                return;
            }

            Console.Clear();

            //EVENT DESCRIPTION

            //TIP
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("| TIP - You can write \u001b[32m[CANCEL]\u001b[36m anytime to go back! |");
            Console.WriteLine(" | [LEAVE EMPTY IF YOU DON'T WANT TO CHANGE IT] | ");
            Console.WriteLine("----------------------------------------------------");

            //QUESTION
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("| Enter the Description of the event [OPTIONAL] |");
            Console.WriteLine("-------------------------------------------------");
            Console.ResetColor();


            Console.Write(">> ");
            string description = Console.ReadLine() ?? "";

            //CHECKING IF USER WANTS TO CANCEL
            if (description.ToUpper() == "CANCEL")
            {
                return;
            }

            //EVENT DATE

            DateTime? date = null;
            while (date == null)
            {
                Console.Clear();

                //TIP
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("| TIP - You can write \u001b[32m[CANCEL]\u001b[36m anytime to go back! |");
                Console.WriteLine(" | [LEAVE EMPTY IF YOU DON'T WANT TO CHANGE IT] | ");
                Console.WriteLine("----------------------------------------------------");

                //QUESTION
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("| Enter the Date of the event [DD/MM/YYYY] |");
                Console.WriteLine("--------------------------------------------");
                Console.ResetColor();

                Console.Write(">> ");
                var input = Console.ReadLine() ?? "";

                //CHECKING IF USER WANTS TO CANCEL
                if (input.ToUpper() == "CANCEL")
                {
                    return;
                }

                //CHECKING IF USER WANTS TO LEAVE IT EMPTY
                if(input.Length == 0)
                {
                    break;
                }

                try
                {
                    date = DateTime.Parse(input);
                }
                catch (Exception err)
                {
                    //ERROR HANDLING
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" >> ERROR: {err}. Press any key to try again... <<");
                    Console.ReadKey();
                    continue;
                }

                if (date < DateTime.Now)
                {
                    //ERROR HANDLING
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" >> ERROR: Date must be in the future. Press any key to try again... <<");
                    Console.ReadKey();
                    date = null;
                }

            }

            Console.Clear();

            //EVENT LOCATION

            //TIP
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("| TIP - You can write \u001b[32m[CANCEL]\u001b[36m anytime to go back! |");
            Console.WriteLine(" | [LEAVE EMPTY IF YOU DON'T WANT TO CHANGE IT] | ");
            Console.WriteLine("----------------------------------------------------");

            //QUESTION
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("| Enter the location of the event [OPTIONAL] |");
            Console.WriteLine("----------------------------------------------");
            Console.ResetColor();

            Console.Write(">> ");
            string location = Console.ReadLine() ?? "";

            //CHECKING IF USER WANTS TO CANCEL
            if (location.ToUpper() == "CANCEL")
            {
                return;
            }

            //CREATING EVENT

            var newEvent = new Event(e.ID, name.Length == 0 ? e.Name : name, description.Length == 0 ? e.Description : description, date == null ? e.Date : (DateTime)date, location.Length == 0 ? e.Location : location);

            Manager.UpdateEvent(newEvent);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"| Event {e.Name} updated successfully! |");
            Console.WriteLine("---------------------------------------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------");
            Console.WriteLine("| Press any key to continue... |");
            Console.WriteLine("--------------------------------");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void CreateEvent()
        {

            //EVENT NAME
            string name = "";
            while (name.Length == 0)
            {
                Console.Clear();

                //TIP
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("| TIP - You can write \u001b[32m[CANCEL]\u001b[36m anytime to go back! |");
                Console.WriteLine("----------------------------------------------------");

                //QUESTION
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("-------------------------------");
                Console.WriteLine("| Enter the name of the event |");
                Console.WriteLine("-------------------------------");
                Console.ResetColor();


                Console.Write(">> ");
                name = Console.ReadLine() ?? "";

                
            }
            //CHECKING IF USER WANTS TO CANCEL
            if ( name.ToUpper() == "CANCEL")
            {
                return;
            }

            Console.Clear();

            //EVENT DESCRIPTION

            //TIP
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"| TIP - You can leave \u001b[32m[OPTIONAL]\u001b[36m fields empty! |");
            Console.WriteLine("------------------------------------------------");

            //QUESTION
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("| Enter the Description of the event [OPTIONAL] |");
            Console.WriteLine("-------------------------------------------------");
            Console.ResetColor();


            Console.Write(">> ");
            string description = Console.ReadLine() ?? "";

            //CHECKING IF USER WANTS TO CANCEL
            if (description.ToUpper() == "CANCEL")
            {
                return;
            }

            //EVENT DATE

            DateTime? date = null;
            while (date == null)
            {
                Console.Clear();

                //TIP
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("| TIP - Set a wrong date? No Problem! you can edit it later! |");
                Console.WriteLine("--------------------------------------------------------------");

                //QUESTION
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("| Enter the Date of the event [DD/MM/YYYY] |");
                Console.WriteLine("--------------------------------------------");
                Console.ResetColor();

                Console.Write(">> ");
                var input = Console.ReadLine();

                //CHECK IF USER LEAVES IT EMPTY
                if (input == null || input.Trim().Length == 0)
                {
                    continue;
                }

                //CHECKING IF USER WANTS TO CANCEL
                if (input.ToUpper() == "CANCEL")
                {
                    return;
                }

                try
                {
                    date = DateTime.Parse(input);
                }
                catch (Exception e)
                {
                    //ERROR HANDLING
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" >> ERROR: Invalid Date Format. Press any key to try again... <<");
                    Console.ReadKey();
                    continue;
                }

                if (date < DateTime.Now)
                {
                    //ERROR HANDLING
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" >> ERROR: Date must be in the future. Press any key to try again... <<");
                    Console.ReadKey();
                    date = null;
                }

            }

            Console.Clear();

            //EVENT LOCATION

            //TIP
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------");
            Console.WriteLine("| TIP - Almost Done! |");
            Console.WriteLine("----------------------");

            //QUESTION
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("| Enter the location of the event [OPTIONAL] |");
            Console.WriteLine("----------------------------------------------");
            Console.ResetColor();

            Console.Write(">> ");
            string? location = Console.ReadLine();

            //CHECKING IF USER WANTS TO CANCEL
            if (location?.ToUpper() == "CANCEL")
            {
                return;
            }

            //CREATING EVENT
            Manager.CreateEvent(new Event(name, description, (DateTime)date, location));

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"| Event {name} created successfully! |");
            Console.WriteLine("---------------------------------------");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------");
            Console.WriteLine("| Press any key to continue... |");
            Console.WriteLine("--------------------------------");
            Console.ResetColor();
            Console.ReadKey();
        }

    }
}


