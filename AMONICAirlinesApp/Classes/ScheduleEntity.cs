using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMONICAirlinesApp.Classes
{
    public class ScheduleEntity
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Flight { get; set; }
        public int CabinPrice { get; set; }
        public int CabinTypeID { get; set; }
        public int Stops { get; set; }
    }
}
