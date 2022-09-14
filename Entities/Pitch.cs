using System;

namespace Entities
{
	public class Pitch
	{
		public Pitch(int id, int number)
		{
			ID = id;
			Number = number;
		}
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

		private int number;
		public int Number
		{
			get
			{
				return number;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentException();
				}
				number = value;
			}
		}
	}
}
