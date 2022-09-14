using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Booking
	{
		private int id;
		public int ID
		{
			get
			{
				return id;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentException();
				}
				id = value;
			}
		}

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int PitchID { get; set; }
		public int BookerID { get; set; }

		private Pitch pitch;
		public Pitch Pitch
		{
			get
			{
				return pitch;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentException();
				}
				pitch = value;
			}
		}

		private Booker booker;
		public Booker Booker
		{
			get
			{
				return booker;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentException();
				}
				booker = value;
			}
		}
	}
}
