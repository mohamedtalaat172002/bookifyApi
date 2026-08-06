namespace bookify.domain.Shared
{
    public record Money(decimal Amount, Currency Currency)
    {
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new ArgumentException("Currencies are not the same");

            return new Money(a.Amount + b.Amount, a.Currency);
        }
        public static Money Zero() => new Money(0, Currency.None);

        public bool IsZero() => this == Zero();
    }
}
