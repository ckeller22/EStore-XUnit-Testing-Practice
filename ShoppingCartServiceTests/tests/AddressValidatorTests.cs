using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using System;
using Xunit;
using FluentAssertions;

namespace ShoppingCartServiceTests
{
    public class AddressValidatorTests
    {

        [Theory]
        [ClassData(typeof(AddressValidatorTestsData))]
        public void validatesAddresses(string city, string country, string street, bool expected)
        {
            // Arrange
            Address invalidAddress = new Address { City = city, Country = country, Street = street };
            AddressValidator sut = new AddressValidator();

            // Act
            bool result = sut.IsValid(invalidAddress);


            // Assert
            result.Should().Be(expected);
        }
    }
}
