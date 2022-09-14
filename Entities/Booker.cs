using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Booker
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

		public string Name { get; set; }
		public string Email { get; set; }
		public int Children { get; set; }
		public int Adults { get; set; }
	}
}
