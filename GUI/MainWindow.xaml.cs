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
		private Repository repo;

		private List<int> numberAvailable = new()
		{

		};

		public MainWindow()
		{
			InitializeComponent();
			WeatherService weatherService = new();
			string weather = weatherService.GetWeather();
			temp.Text = weather;
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
			List<Bookings> bookings = repo.GetAllBookings();
			List<Booker> bookers = repo.GetAllBookers();
			List<Pitches> pitches = repo.GetAllPitches();
			BookingGrid.ItemsSource = bookings;
			BookerGrid.ItemsSource = bookers;
			PitchGrid.ItemsSource = pitches;
		}

		private void AddBooking_Click(object sender, RoutedEventArgs e)
		{
			// Mock input data:

			// Make object to send to repository:

			// Call the repository:
		}
	}
}
