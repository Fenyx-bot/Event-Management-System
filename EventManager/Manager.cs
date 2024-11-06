using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventManager
{
    static class Manager
    {
        // List of future events
        public static List<Event> AllEvents = new List<Event>();

        // List of archived events
        public static List<Event> OldEvents = new List<Event>();

        // Index of the selected event
        public static int choiceIndex = 0;

        // Index of the selected page
        public static int pageIndex = 1;


        public static void DeleteEvent(int index, bool isArchived = false) //Delete event from the list
        {

            try
            {
                if (isArchived)
                    OldEvents.RemoveAt(index);
                else
                    AllEvents.RemoveAt(index);
            } catch(ArgumentOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(">> ERROR: No event to delete. <<");
                Console.ResetColor();
            }

            SaveEvents();
        }

        public static void DeleteOldEvent(int index) //Delete event from the list
        {
            OldEvents.RemoveAt(index);
            SaveEvents();
        }

        public static void UpdateEvent(Event e)
        {
            for(int i = 0; i < AllEvents.Count; i++)
            {
                if (AllEvents[i].ID == e.ID)
                {
                    AllEvents[i] = e;
                    SaveEvents();
                    return;
                }
            }
        }

        public static void LoadEvents() //Loading events from the files and sorting them by date
        {
            string eventJsonData = "";
            string oldeventJsonData = "";

            try
            {
                eventJsonData = File.ReadAllText("EventsData.json");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found. Creating new file.");
                var x = File.Create("EventsData.json");
                x.Close();
                Thread.Sleep(1000);
                SaveEvents();
                eventJsonData = File.ReadAllText("EventsData.json");
            }

            try
            {
                oldeventJsonData = File.ReadAllText("OldEventsData.json");
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("File not found. Creating new file.");
                var x = File.Create("OldEventsData.json");
                x.Close();
                Thread.Sleep(1000);
                SaveEvents();
                oldeventJsonData = File.ReadAllText("OldEventsData.json");
            }

            List<Event> eventlist = JsonSerializer.Deserialize<List<Event>>(eventJsonData)!;

            if (eventlist.Count > 0)
            {
                AllEvents = eventlist.OrderBy(e => e.Date).ToList();
            }

            eventlist = JsonSerializer.Deserialize<List<Event>>(oldeventJsonData)!;

            if (eventlist.Count > 0)
            {
                OldEvents = eventlist.OrderBy(e => e.Date).ToList();
            }

            for(int i = 0; i < AllEvents.Count; i++)
            {
                if (AllEvents[i].Date < DateTime.Now)
                {
                    OldEvents.Add(AllEvents[i]);
                    AllEvents.Remove(AllEvents[i]);
                }
            }

            SaveEvents();

            Console.Clear();
        }

        public static void SaveEvents() // saving events to the files
        {
            if (AllEvents.Count > 0)
            {
                AllEvents = AllEvents.OrderBy(e => e.Date).ToList();

                Console.WriteLine("Events saved successfully.");
            }

            string eventJsonData = JsonSerializer.Serialize(AllEvents);
            try
            {
                File.WriteAllText("EventsData.json", eventJsonData);
            }catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">> ERROR: {e.Message} <<" );
                Console.ResetColor();
            }

            string oldEventJsonData = JsonSerializer.Serialize(OldEvents);
            try
            {
                File.WriteAllText("OldEventsData.json", oldEventJsonData);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($">> ERROR: {e.Message} <<");
                Console.ResetColor();
            }
        }

        public static void CreateEvent(Event newEvent) //Adding new event to the list
        {
            AllEvents.Add(newEvent);
            SaveEvents();
        }
    }
}
