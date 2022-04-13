using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncCountdown.DTOs
{
    public class EventDetailsEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int nUserID { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string szEventName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string szEventDate { get; set; }

        public DateTime dtCreatedDTS { get; set; }
        public DateTime dtLastUpdatedDTS { get; set; }
    }
}
