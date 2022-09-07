using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Entities;

namespace DataAccess
{
	public class Repository
	{
		private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = BookingSystem; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

		public Repository()
		{
			//Test if application can contact database server:
			SqlConnection connection = new(connectionString);
			connection.Open();
			connection.Close();
		}

		public List<Booking> GetAllBookings()
		{
			// Vi bruger den samme forbindelse til begge SELECT transaktions, så derfor har vi kun én sql streng:
			string sql = "SELECT * FROM Pitches; SELECT * FROM Bookers; SELECT * FROM Bookings;";

			// Lav en forbindelse til databasen, åbn den:
			SqlConnection connection = new(connectionString);
			connection.Open();

			// Lav et command object, der skal udføre transaktionen
			SqlCommand command = new(sql, connection);

			// Udfør transaktionen, og gem resultatet i en SqlDataReader:
			SqlDataReader reader = command.ExecuteReader();

			// Udtag Bookings data og lav om til C# objekter:
			List<Pitch> pitches = MakePitchesList(reader);

			reader.NextResult();

			List<Booker> bookers = MakeBookersList(reader);

			reader.NextResult();

			List<Booking> bookings = MakeBookingsList(reader, pitches, bookers);

			// returner listen der nu er fyldt med objekter:
			return bookings;
		}

		public List<Pitch> GetAllPitches()
		{
			List<Pitch> pitches = new();

			string sql = "SELECT * FROM Pitches;";

			SqlConnection connection = new(connectionString);
			connection.Open();

			SqlCommand command = new(sql, connection);

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				int id = (int)reader["ID"];
				int number = (int)reader["Number"];
				Pitch pitch = new()
				{
					ID = id,
					Number = number
				};
				pitches.Add(pitch);
			}

			return pitches;
		}

		public List<Booker> GetAllBookers()
		{
			List<Booker> bookers = new();

			string sql = "SELECT * FROM Bookers;";

			SqlConnection connection = new(connectionString);
			connection.Open();

			SqlCommand command = new(sql, connection);

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				int id = (int)reader["ID"];
				string name = (string)reader["Name"];
				string email = (string)reader["Email"];
				int children = (int)reader["Children"];
				int adults = (int)reader["Adults"];
				Booker booker = new()
				{
					ID = id,
					Name = name,
					Email = email,
					Children = children,
					Adults = adults
				};
				bookers.Add(booker);
			}

			return bookers;
		}

		private List<Booking> MakeBookingsList(SqlDataReader reader, List<Pitch> pitches, List<Booker> bookers)
		{
			List<Booking> bookings = new();

			while (reader.Read())
			{
				int id = (int)reader["ID"];
				DateTime startDate = (DateTime)reader["StartDate"];
				DateTime endDate = (DateTime)reader["EndDate"];
				int pitch_FK = (int)reader["PitchID"];
				int booker_FK = (int)reader["BookerID"];

				Pitch pitch = GetPitchForBooking(pitch_FK, pitches);

				Booker booker = GetBookerForBooking(booker_FK, bookers);

				Booking booking = new()
				{
					ID = id,
					StartDate = startDate,
					EndDate = endDate,
					Pitch = pitch,
					Booker = booker
				};
				bookings.Add(booking);
			}

			return bookings;
		}

		private List<Pitch> MakePitchesList(SqlDataReader reader)
		{
			List<Pitch> pitches = new();

			while (reader.Read())
			{
				int id = (int)reader["ID"];
				int number = (int)reader["Number"];
				Pitch pitch = new()
				{
					ID = id,
					Number = number
				};
				pitches.Add(pitch);
			}

			return pitches;
		}

		private List<Booker> MakeBookersList(SqlDataReader reader)
		{
			List<Booker> bookers = new();

			while (reader.Read())
			{
				int id = (int)reader["ID"];
				string name = (string)reader["Name"];
				string email = (string)reader["Email"];
				int children = (int)reader["Children"];
				int adults = (int)reader["Adults"];
				Booker booker = new()
				{
					ID = id,
					Name = name,
					Email = email,
					Children = children,
					Adults = adults
				};
				bookers.Add(booker);
			}

			return bookers;
		}

		private Pitch GetPitchForBooking(int pitch_FK, List<Pitch> pitches)
		{
			foreach (Pitch pitch in pitches)
			{
				if (pitch.ID == pitch_FK)
				{
					return pitch;
				}
			}
			return new();
		}

		private Booker GetBookerForBooking(int Booker_FK, List<Booker> bookers)
		{
			foreach (Booker booker in bookers)
			{
				if (booker.ID == Booker_FK)
				{
					return booker;
				}
			}
			return new();
		}


		private int GetBooker()
		{
			string sql = "SELECT TOP 1 * FROM Bookers ORDER BY ID DESC";

			SqlConnection connection = new(connectionString);
			connection.Open();

			SqlCommand command = new(sql, connection);

			SqlDataReader reader = command.ExecuteReader();

			int id = 0;
			while (reader.Read())
			{
				id = (int)reader["ID"];
			}

			return id;
		}

		private string MakeSqlDateTimeHappy(DateTime Date)
		{
			return Date.ToString("yyyy.MM.dd");
		}

		public void AddNewBooking(Booker booker, Booking booking)
		{
			string sql1 = $"INSERT INTO Bookers (Name, Email, Children, Adults) VALUES ('{booker.Name}', '{booker.Email}', {booker.Children}, {booker.Adults});";

			SqlConnection connection = new(connectionString);
			connection.Open();

			SqlCommand command1 = new(sql1, connection);

			command1.ExecuteNonQuery();

			string sql2 = $"INSERT INTO Bookings (StartDate, EndDate, PitchID, BookerID) VALUES ('{MakeSqlDateTimeHappy(booking.StartDate)}', '{MakeSqlDateTimeHappy(booking.EndDate)}', {booking.PitchID}, {GetBooker()});";

			SqlCommand command2 = new(sql2, connection);

			command2.ExecuteNonQuery();
		}

		public void UpdateBooking(Booker booker, Booking booking)
		{
			string sql1 = $"UPDATE Bookers SET Name = '{booker.Name}', Email = '{booker.Email}', Children = {booker.Children}, Adults = {booker.Adults} WHERE ID = {booker.ID};";

			SqlConnection connection = new(connectionString);
			connection.Open();

			SqlCommand command1 = new(sql1, connection);

			command1.ExecuteNonQuery();

			string sql2 = $"UPDATE Bookings SET StartDate = '{MakeSqlDateTimeHappy(booking.StartDate)}', EndDate = '{MakeSqlDateTimeHappy(booking.EndDate)}', PitchID = {booking.PitchID} WHERE ID = {booking.ID};";

			SqlCommand command2 = new(sql2, connection);

			command2.ExecuteNonQuery();
		}
	}
}