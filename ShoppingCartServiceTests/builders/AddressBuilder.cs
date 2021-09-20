using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartServiceTests.builders
{
    public class AddressBuilder
    {
        private string _city = "city";
        private string _country = "country";
        private string _street = "123 street";

        public Address Build()
        {
            return new Address()
            {
                City = _city,
                Country = _country,
                Street = _street
            };
        }

        public AddressBuilder WithCity(string city)
        {
            this._city = city;
            return this;
        }

        public AddressBuilder WithCountry(string country)
        {
            this._country= country;
            return this;
        }

        public AddressBuilder WithStreet(string street)
        {
            this._street = street;
            return this;
        }
    }
}
