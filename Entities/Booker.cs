using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Booker
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int Children { get; set; }
		public int Adults { get; set; }
	}
}
