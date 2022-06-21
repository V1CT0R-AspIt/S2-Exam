using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccess;
using Entities;
using Services;

namespace GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly Repository repo;

		public MainWindow()
		{
			InitializeComponent();
			try
			{
				//Initialize repo field:
				repo = new();
			}
			catch (Exception e)
			{

				MessageBox.Show($"Fejl under tilgang til data: {e.Message}", "Opstartsfejl", MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}
			WeatherService weatherService = new();
			string weather = weatherService.GetWeather();
			temp.Text = weather;
			List<int> pitchlist = repo.pitchList();
			SpotInput.ItemsSource = pitchlist;

			RefreshGridList();
		}
		private void RefreshGridList()
		{
			List<Bookings> bookings = repo.GetAllBookings();
			List<Booker> bookers = repo.GetAllBookers();
			List<Pitches> pitches = repo.GetAllPitches();
			BookingGrid.ItemsSource = null;
			BookerGrid.ItemsSource = null;
			PitchGrid.ItemsSource = null;
			BookingGrid.ItemsSource = bookings;
			BookerGrid.ItemsSource = bookers;
			PitchGrid.ItemsSource = pitches;
		}

		private void AddBooking_Click(object sender, RoutedEventArgs e)
		{
			// Mock input data:
			int pitchid = 1;
			int bookerid = 1;
			int bookingid = 1;
			int number = SpotInput.SelectedIndex;
			DateTime startdate = StartDate.SelectedDate.Value;
			DateTime enddate = EndDate.SelectedDate.Value;
			string name = NameInput.Text;
			string mail = MailInput.Text;

			// Gives an IDs to connect the right info together:
			List<Bookings> bookings = repo.GetAllBookings();
			List<Booker> bookers = repo.GetAllBookers();
			List<Pitches> pitches = repo.GetAllPitches();
			for (int i = 0; i < bookings.Count; i++)
			{
				if(pitchid == pitches[i].ID)
				{
					pitchid++;
				}
				if (bookerid == bookers[i].ID)
				{
					bookerid++;
				}
				if (bookingid == bookings[i].ID)
				{
					bookingid++;
				}
			}

			// Make object to send to repository:
			Pitches newpitch = new()
			{
				ID = pitchid,
				Number = number
			};
			Booker newbooker = new()
			{
				ID = bookerid,
				Name = name,
				Mail = mail
			};
			Bookings newbooking = new()
			{
				ID = bookingid,
				StartDate = startdate,
				EndDate = enddate,
				PitchID = pitchid,
				BookerID = bookerid
			};

			// Call the repository:
			repo.AddNewPitch(newpitch);
			repo.AddNewBooker(newbooker);
			repo.AddNewBooking(newbooking);

			RefreshGridList();
		}

	}
}
