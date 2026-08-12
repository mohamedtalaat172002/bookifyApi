using bookify.domain.Abstractions;

namespace bookify.domain.Reviews
{
    public static class ReviewErrors
    {
        public static Error NotCompleted => new Error("Review.NotCompleted", "The review is not completed.");
    }
}
