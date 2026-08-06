namespace bookify.domain.Shared
{
    public record Currency
    {
        public string Code { get; init; }

        private Currency(string code) => Code = code;

        public static readonly Currency USD = new("USD");
        public static readonly Currency EGP = new("EGP");
        internal static readonly Currency None = new("");

        public static readonly IReadOnlyCollection<Currency> All =
            new[] { USD, EGP };

        public static Currency FromCode(string code)
        {
            return All.FirstOrDefault(x => x.Code == code) ??
                throw new ApplicationException("The Currency Is InValid");
        }


    }
}
