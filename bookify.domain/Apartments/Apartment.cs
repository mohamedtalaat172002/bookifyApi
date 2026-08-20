using bookify.domain.Abstractions;
using bookify.domain.Shared;

namespace bookify.domain.Apartments
{
    public sealed class Apartment : Entity
    {
        public Apartment(Guid id,
            Name name,
            Description description,
            Address address,
            Money Price,
            Money CleaningFee,
            List<Amenity> amenities) : base(id)
        {
            this.Name = name;
            this.Description = description;
            this.Address = address;
            this.Price = Price;
            this.CleaningFee = CleaningFee;
            this.Amenities = amenities;
        }
        private Apartment()
        {

        }

        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public Address Address { get; private set; }


        public Money Price { get; private set; }

        public Money CleaningFee { get; private set; }

        public DateTime LastBookedOnUtc { get; internal set; }
        public List<Amenity> Amenities { get; private set; }
    }
}
