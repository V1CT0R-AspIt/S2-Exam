﻿using System;
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

		public List<Booker> GetAllBookers()
		{
			// Make the list that the method returns:
			List<Booker> bookers = new();

			// Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			// Make the SQl query:
			string sql = "SELECT * FROM Bookers";

			// Make the command object:
			SqlCommand command = new(sql, connection);

			// Execute query and save the returned data in a variable:
			SqlDataReader reader = command.ExecuteReader();

			// Convert reader data to C# objects. For each row in the reader:
			while (reader.Read())
			{
				int id = (int)reader[0];
				string name = (string)reader[1];
				string mail = (string)reader[2];

				// Assign all the retrieved values to the booker object:
				Booker b = new()
				{
					ID = id,
					Name = name,
					Mail = mail
				};

				// Add the retrieved booker to the list of bookers:
				bookers.Add(b);
			}

			// Return the list of bookers:
			return bookers;
		}

		public List<Pitches> GetAllPitches()
		{
			// Make the list that the method returns:
			List<Pitches> pitches = new();

			// Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			// Make the SQl query:
			string sql = "SELECT * FROM Pitches";

			// Make the command object:
			SqlCommand command = new(sql, connection);

			// Execute query and save the returned data in a variable:
			SqlDataReader reader = command.ExecuteReader();

			// Convert reader data to C# objects. For each row in the reader:
			while (reader.Read())
			{
				int id = (int)reader[0];
				int number = (int)reader[1];

				// Assign all the retrieved values to the booker object:
				Pitches p = new()
				{
					ID = id,
					Number = number
				};

				// Add the retrieved booker to the list of pitches:
				pitches.Add(p);
			}

			// Return the list of pitches:
			return pitches;
		}

		public List<Bookings> GetAllBookings()
		{
			// First, get all the contact infos, because they are aggregated from bookers and pitches:
			List<Booker> bookers = GetAllBookers();
			List<Pitches> pitches = GetAllPitches();

			// Make the list that the method returns:
			List<Bookings> bookings = new();

			// Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			// Make the SQl query:
			string sql = "SELECT * FROM Bookings";

			// Make the command object:
			SqlCommand command = new(sql, connection);

			// Execute query and save the returned data in a variable:
			SqlDataReader reader = command.ExecuteReader();

			// Convert reader data to C# objects. For each row in the reader:
			while (reader.Read())
			{
				int id = (int)reader[0];
				DateTime start = (DateTime)reader[1];
				DateTime end = (DateTime)reader[2];

				// This is the pitches foreign key:
				int pitch_FK = (int)reader[3];
				// This is the bookers foreign key:
				int booker_FK = (int)reader[4];

				// The aggregated pitches and bookers object. Initialize to null:
				Pitches pitch = null;
				Booker booker = null;

				// Loop through all the pitches objects in the pitches list, that we got before from the database:
				for (int i = 0; i < pitches.Count; i++)
				{
					// If there is a match in the retrieved bookings row's FK value,
					if (pitch_FK == pitches[i].ID)
					{
						// then assign the object from the list, to the property on the bookings object, thereby making the OOP aggregation:
						pitch = pitches[i];

						// Break out of the loop, because there' no reason to continue:
						break;
					}
				}
				// Loop through all the booker objects in the booker list, that we got before from the database:
				for (int i = 0; i < bookers.Count; i++)
				{
					// If there is a match in the retrieved bookings row's FK value,
					if (booker_FK == bookers[i].ID)
					{
						// then assign the object from the list, to the property on the bookings object, thereby making the OOP aggregation:
						booker = bookers[i];

						// Break out of the loop, because there' no reason to continue:
						break;
					}
				}

				// Assign all the retrieved values to the bookings object:
				Bookings b = new()
				{
					ID = id,
					Start = start,
					End = end,
					PitchID = pitch_FK,
					BookerID = booker_FK
				};

				// Add the retrieved person to the list of bookings:
				bookings.Add(b);
			}

			// Return the list of bookings:
			return bookings;
		}
	}
	public class DataItem
	{
		public double Values { get; set; }
		public double MaxValue { get; set; }
	}
}
