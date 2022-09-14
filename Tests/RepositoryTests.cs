namespace Tests
{
	public class RepositoryTests
	{
		[Fact]
		public void CorrectBookingCount()
		{
			// Arrange:
			Repository repo = new();
			int expextedNrOfBookings = 2;

			// Act:
			List<Booking> bookings = repo.GetAllBookings();

			// Assert:
			Assert.Equal(expextedNrOfBookings, bookings.Count);
		}

		[Fact]
		public void IsPitchStatusValid()
		{
			// Arrange:
			Pitch p;
			int id = 1;
			int number = 1;
			int actualID;
			int actualNumber;

			// Act:
			p = new(id, number);
			actualID = p.ID;
			actualNumber = p.Number;

			// Assert:
			Assert.Equal(actualID, id);
			Assert.Equal(actualNumber, number);
		}
	}
}