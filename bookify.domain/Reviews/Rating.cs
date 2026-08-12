using bookify.domain.Abstractions;

namespace bookify.domain.Reviews
{
    public sealed record Rating
    {

        public static Error InValid => new Error("Rating.InValid", "Rating must be between 1 and 5");
        private Rating(int value) =>
        Value = value;
        public int Value { get; init; }

        public static Result<Rating> Create(int value)
        {
            if (value < 1 || value > 5)
            {
                return Result.Failure<Rating>(InValid);
            }
            return Result.Success(new Rating(value));
        }


    }
}
