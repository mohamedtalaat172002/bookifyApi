namespace bookify.domain.Bookings
{
    public record DateRange
    {
        private DateRange()
        {

        }
        public DateOnly Start { get; init; }
        public DateOnly End { get; init; }
        public int LengthInDays => End.DayNumber - Start.DayNumber;

        public static DateRange Create(DateOnly start, DateOnly end)
        {
            if (end < start)
            {
                throw new ArgumentException("End date cannot be earlier than start date.");
            }
            return new DateRange
            {
                Start = start,
                End = end
            };

        }
    }
}
