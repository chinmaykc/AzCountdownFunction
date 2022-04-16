using System;
using System.Collections.Generic;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FuncCountdown.DTOs
{
    public class EventDetailsDto
    {
        [JsonProperty("eventName", Order = 1)]
        public string EventName { get; set; }

        [JsonProperty("eventDate", Order = 2)]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EventDTS { get; set; }

        [JsonProperty("totalDaysLeft", Order = 3)]
        public int Remaining_Days { get; set; }

        [JsonProperty("totalWorkingDaysLeft", Order = 4)]
        public int Remaining_WorkingDays { get; set; }

        [JsonProperty("totalWeekends", Order = 5)]
        public int Weekends { get; set; }

        [JsonProperty("numberOfHolidays", Order = 6)]
        public int UpcomingHolidays { get; set; }

        [JsonProperty("holidays", Order = 7)]
        public List<string> HolidayNameList { get; set; }

        [JsonProperty("today", Order = 8)]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CurrentDate { get; set; }
    }

    #region When using System.Text.Json;
    /// <summary>
    /// For Custom property name and order
    /// use -> JsonPropertyName("myName")
    /// use -> JsonPropertyOrder(1)
    /// </summary>

    //class CustomDateTimeConverter : JsonConverter<DateTime>
    //{
    //    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        return reader.GetDateTime();
    //    }

    //    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStringValue(value.ToString("dd-MMM-yyyy"));
    //    }
    //} 
    #endregion

    #region When using Newtonsoft.Json.Converters;
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "dd-MMM-yyyy";
        }
    }
    #endregion


    public record EventDetailsMinifiedDto(string EventName, string EventDate, int TotalDaysLeft, string Today);
}
