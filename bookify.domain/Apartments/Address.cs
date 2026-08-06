namespace bookify.domain.Apartments
{
    public sealed record Address(
        string Country,
        string City,
        string Street,
        string ZipCode
   );

}
