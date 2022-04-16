using FuncCountdown.DTOs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuncCountdown.Utilities
{
    /// <summary>
    /// Reads events and holidays from Json insted of DB.
    /// TODO: Use DB later
    /// </summary>
    public class JsonDBContext
    {
        private const string DB_FILE_NAME = "EventRepoDB.json";
        private static readonly string _jsonDBFilePath;
        private readonly List<EventDetailsEntity> _events;
        private readonly SortedDictionary<DateTime, string> _holidays;

        static JsonDBContext()
        {
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _jsonDBFilePath = Path.GetFullPath(Path.Combine(binDirectory, $"..\\{DB_FILE_NAME}"));
        }
        public JsonDBContext()
        {
            JObject dbContext = JObject.Parse(File.ReadAllText(_jsonDBFilePath));
            var events = dbContext["Events"];
            _events = events.ToObject<List<EventDetailsEntity>>();

            var holidays = dbContext["Holidays"];
            _holidays = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<DateTime, string>>(holidays.ToString());

        }

        public List<EventDetailsEntity> UserEvents { get { return _events; } }
        public SortedDictionary<DateTime, string> Holidays { get { return _holidays; } }
    }
}
