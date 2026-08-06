namespace bookify.domain.Abstractions
{
    public record Error(String Code, string Name)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "Value cannot be null.");
    }
}
