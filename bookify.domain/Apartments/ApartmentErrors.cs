using bookify.domain.Abstractions;

namespace bookify.domain.Apartments
{
    public sealed class ApartmentErrors
    {
        public static Error NotFound = new("apartment.Notfound", "Apartment With The Specified Identifier not found.");
    }
}
