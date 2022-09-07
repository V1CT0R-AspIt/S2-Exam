using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Booking
	{
		public int ID { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int PitchID { get; set; }
		public int BookerID { get; set; }
		public Pitch Pitch { get; set; }
		public Booker Booker { get; set; }
	}
}
