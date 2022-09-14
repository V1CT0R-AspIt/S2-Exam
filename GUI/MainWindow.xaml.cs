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
using System.Net.Mail;
using System.Reflection;

namespace GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//Used to access methods in the Repository
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


			List<Pitch> pitches = repo.GetAllPitches();
			PitchInput.ItemsSource = pitches;
			CheckAndSearchPitchInput.ItemsSource = pitches;

			RefreshGridList();
		}


		private void RefreshGridList()
		{
			List<Booking> bookings = repo.GetAllBookings();
			BookingGrid.ItemsSource = null;
			BookingGrid.ItemsSource = bookings;
		}


		#region AddNewBooking
		private bool AreDatesConsistent(DateTime startDate, DateTime endDate)
		{
			return startDate < endDate;
		}

		private bool IsPitchAvailable(DateTime startDate, DateTime endDate, Pitch pitch)
		{
			List<Booking> bookings = repo.GetAllBookings();

			for (int i = 0; i < bookings.Count; i++)
			{
				if ((startDate <= bookings[i].EndDate && startDate >= bookings[i].StartDate) || (endDate <= bookings[i].EndDate && endDate >= bookings[i].StartDate))
				{
					if (pitch.Number == bookings[i].Pitch.Number)
					{
						return false;
					}
				}
			}

			return true;
		}

		private bool IsEmailValid(string email)
		{
			var valid = true;

			try
			{
				var emailAddress = new MailAddress(email);
			}
			catch
			{
				valid = false;
			}
			return valid;
		}

		private bool IsEmailAvailable(string email)
		{
			List<Booking> bookings = repo.GetAllBookings();

			bool valid = true;

			for (int i = 0; i < bookings.Count; i++)
			{
				if (email.ToLower() == bookings[i].Booker.Email.ToLower())
				{
					valid = false;
				}
			}

			return valid;
		}

		private void AddBooking_Click(object sender, RoutedEventArgs e)
		{
			string name = NameInput.Text;
			string email = EmailInput.Text;
			int children = Int32.Parse(ChildrenQuantityInput.Text);
			int adults = Int32.Parse(AdultsQuantityInput.Text);
			DateTime startDate = StartDateInput.SelectedDate.Value;
			DateTime endDate = EndDateInput.SelectedDate.Value;
			Pitch pitch = (Pitch)PitchInput.SelectedItem;

			Booker booker = new()
			{
				Name = name,
				Email = email,
				Children = children,
				Adults = adults
			};

			Booking booking = new()
			{
				StartDate = startDate,
				EndDate = endDate,
				PitchID = pitch.ID,
				BookerID = booker.ID
			};

			if (AreDatesConsistent(booking.StartDate, booking.EndDate))
			{
				if (IsPitchAvailable(booking.StartDate, booking.EndDate, pitch))
				{
					if (IsEmailValid(booker.Email))
					{
						if (IsEmailAvailable(booker.Email))
						{
							repo.AddNewBooking(booker, booking);
						}
						else
						{
							MessageBox.Show($"Denne Mail er allerede I brug", "Brug en anden Mail eller Rediger din booking", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
				}
				else
				{
					MessageBox.Show($"Denne Plads er allerede I brug I denne Tids Periode", "Vælg en anden Plads eller Tids Periode", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}

			RefreshGridList();
		} 
		#endregion

		#region EditBooking
		Booking selectedBooking;

		private void BookingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (BookingGrid.SelectedItem != null)
			{
				selectedBooking = (Booking)BookingGrid.SelectedItem;

				NameInput.Text = selectedBooking.Booker.Name;
				EmailInput.Text = selectedBooking.Booker.Email;
				StartDateInput.SelectedDate = selectedBooking.StartDate;
				EndDateInput.SelectedDate = selectedBooking.EndDate;
				PitchInput.SelectedIndex = selectedBooking.Pitch.Number - 1;
				ChildrenQuantityInput.Text = selectedBooking.Booker.Children.ToString();
				AdultsQuantityInput.Text = selectedBooking.Booker.Adults.ToString();
			}
		}

		private bool IsEmailTheSame(string email)
		{
			bool valid = true;

			if (email.ToLower() != selectedBooking.Booker.Email.ToLower())
			{
				valid = false;
			}

			return valid;
		}

		private void EditBooking_Click(object sender, RoutedEventArgs e)
		{
			Pitch pitch = (Pitch)PitchInput.SelectedItem;

			Booker booker = selectedBooking.Booker;

			booker.Name = NameInput.Text;
			booker.Email = EmailInput.Text;
			booker.Children = Int32.Parse(ChildrenQuantityInput.Text);
			booker.Adults = Int32.Parse(AdultsQuantityInput.Text);

			Booking booking = selectedBooking;

			booking.StartDate = StartDateInput.SelectedDate.Value;
			booking.EndDate = EndDateInput.SelectedDate.Value;
			booking.PitchID = pitch.ID;
			booking.BookerID = booker.ID;
			booking.Pitch = pitch;
			booking.Booker = booker;

			if (AreDatesConsistent(booking.StartDate, booking.EndDate))
			{
				if (IsEmailValid(booker.Email))
				{
					if (IsEmailTheSame(booker.Email))
					{
						if (selectedBooking.StartDate == StartDateInput.SelectedDate &&
					selectedBooking.EndDate == EndDateInput.SelectedDate &&
					selectedBooking.Pitch.Number == PitchInput.SelectedIndex + 1)
						{
							repo.UpdateBooking(booker, booking);
						}
						else
						{
							if (IsPitchAvailable(booking.StartDate, booking.EndDate, booking.Pitch))
							{
								repo.UpdateBooking(booker, booking);
							}
							else
							{
								MessageBox.Show($"Denne Plads er allerede I brug I denne Tids Periode", "Vælg en anden Plads eller Tids Periode", MessageBoxButton.OK, MessageBoxImage.Error);
							}
						}
					}
					else
					{
						MessageBox.Show($"Denne Mail matcher ikke den Booking du prøver at endre", "Brug den samme Mail hvis du skal rediger dumbass", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}

			RefreshGridList();
		} 
		#endregion


		#region SearchAndCheck
		List<Booking> searchModeList = new();

		private void Search_Click(object sender, RoutedEventArgs e)
		{
			searchModeList.Clear();
			List<Booking> bookings = repo.GetAllBookings();
			if (CheckAndSearchPitchInput.SelectedIndex + 1 >= 0)
			{
				for (int i = 0; i < bookings.Count; i++)
				{
					if (CheckAndSearchPitchInput.SelectedIndex + 1 == bookings[i].Pitch.Number)
					{
						searchModeList.Add(bookings[i]);
					}
				}
			}
			if (searchModeList.Count == 0)
			{
				searchModeList = bookings;
			}
			BookingGrid.ItemsSource = null;
			BookingGrid.ItemsSource = searchModeList;
		}

		private void Check_Click(object sender, RoutedEventArgs e)
		{
			if (AreDatesConsistent(CheckStartDateInput.SelectedDate.Value, CheckEndDateInput.SelectedDate.Value))
			{
				if (CheckAndSearchPitchInput.SelectedIndex + 1 >= 0)
				{
					if (IsPitchAvailable(CheckStartDateInput.SelectedDate.Value, CheckEndDateInput.SelectedDate.Value, (Pitch)CheckAndSearchPitchInput.SelectedItem))
					{
						CheckPitchAnswer.Text = "Plads ledig";
					}
					else
					{
						CheckPitchAnswer.Text = "Plads booket!";
					}
				}
			}
		} 
		#endregion
	}
}