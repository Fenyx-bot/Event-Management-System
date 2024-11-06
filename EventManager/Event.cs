using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventManager
{
    public class Event
    {
        static int _idCount;
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime Date { get; private set; }
        public string? Location { get; private set; }

        [JsonConstructor]
        public Event(int id, string name, string? description, DateTime date, string? location)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Date = date;
            this.Location = location;

            if(id > _idCount)
            {
                _idCount = id;
            }
        }

        public Event(string name, string? description, DateTime date, string? location)
        {
            this.ID = ++_idCount;
            this.Name = name;
            this.Description = description;
            this.Date = date;
            this.Location = location;
        }

    }
}


