using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Bookings
	{
		public int ID { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public int PitchID { get; set; }
		public int BookerID { get; set; }
	}
}
