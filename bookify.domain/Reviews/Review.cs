using bookify.domain.Abstractions;
using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Reviews.Events;

namespace bookify.domain.Reviews
{
    public sealed class Review : Entity
    {
        public Review(Guid id, Guid userId, Guid apartmentId, Guid bookingId, Rating rating, Comment comment, DateTime createdAt)
            : base(id)
        {
            UserId = userId;
            ApartmentId = apartmentId;
            BookingId = bookingId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
        }

        public Guid UserId { get; private set; }
        public Guid ApartmentId { get; private set; }
        public Guid BookingId { get; private set; }
        public Rating Rating { get; private set; }
        public Comment Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static Result<Review> Create
            (Apartment apartment,
            Booking booking, Rating rating,
            Comment comment, DateTime createdAt)
        {
            if (booking.Status != BookingStatus.completed)
            {
                return Result.Failure<Review>(ReviewErrors.NotCompleted);
            }
            var review = new Review(Guid.NewGuid(), booking.UserId, booking.ApartmentId, booking.id, rating, comment, createdAt);

            review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.id));
            return Result.Success(review);

        }



    }
}
