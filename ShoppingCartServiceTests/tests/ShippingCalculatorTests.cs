using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ShoppingCartServiceTests.tests
{
    public class ShippingCalculatorTests
    {
        #region Same city 
        [Fact]
        public void CalculateShippingCosts_SameCityStandardShippingNoItems_Return0()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item>(),
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(0);
        }

        [Fact]
        public void CalculateShippingCosts_SameCityStandardShippingOneItem_Return1()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item> { new Item { Quantity = 1 } },
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCityRate * 1);
        }

        [Fact]
        public void CalculateShippingCosts_SameCityStandardShippingMultipleItems_ReturnQuantityOfItems()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item> { new Item { Quantity = 1 }, new Item { Quantity = 5 } },
                ShippingAddress = new Address { City = "city", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCityRate * (1 + 5));
        }

        #endregion

        #region Same country 
        
        [Fact]
        public void CalculateShippingCosts_SameCountryStandardShippingNoItems_Returns0()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> {},
                ShippingAddress = new Address { City = "city2", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(0);
        }

        [Fact]
        public void CalculateShippingCosts_SameCountryStandardShippingOneItemQuantity1_ReturnQuantityMultipliedBySameCountryRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { new Item { Quantity = 1 } },
                ShippingAddress = new Address { City = "city2", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCountryRate * 1);
        }

        [Fact]
        public void CalculateShippingCosts_SameCountryStandardShippingOneItemQuantity5_ReturnQuantityMultipliedBySameCountryRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { new Item { Quantity = 5 } },
                ShippingAddress = new Address { City = "city2", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCountryRate * 5);
        }

        [Fact]
        public void CalculateShippingCosts_SameCountryStandardShippingMultipleItems_ReturnQuantityMultipliedBySameCountryRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { new Item { Quantity = 1 }, new Item { Quantity = 3 } },
                ShippingAddress = new Address { City = "city2", Country = "country", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCountryRate * (1 + 3));
        }

        #endregion

        #region International

        [Fact]
        public void CalculateShippingCosts_InternationalStandardShippingNoItems_Returns0()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(0);
        }

        [Fact]
        public void CalculateShippingCosts_InternationalStandardShippingOneItemQuantity1_ReturnsQuantityTimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { new Item { Quantity = 1} },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * 1);
        }

        [Fact]
        public void CalculateShippingCosts_InternationalStandardShippingOneItemQuantity5_ReturnsQuantityTimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { new Item { Quantity = 5 } },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * 5);
        }

        [Fact]
        public void CalculateShippingCosts_InternationalStandardShippingMultipleItems_ReturnsQuantityTimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new List<Item> { new Item { Quantity = 2 }, new Item { Quantity = 3 } },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * (2 + 3));
        }
        #endregion

        #region Shipping Speeds

        [Fact]
        public void CalculateShippingCosts_InternationalStandardShippingOneItemQuantity1_ReturnsQuantityTimesInternationalRateTimesStandardShipping()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                Items = new List<Item> { new Item { Quantity = 1 } },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * 1.0 * 1);

            
        }

        [Fact]
        public void CalculateShippingCosts_InternationalExpeditedShippingOneItemQuantity1_ReturnsQuantityTimesInternationalRateTimesExpeditedShipping()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Expedited,
                Items = new List<Item> { new Item { Quantity = 1 } },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * 1.2 * 1);


        }

        [Fact]
        public void CalculateShippingCosts_InternationalPriorityShippingOneItemQuantity1_ReturnsQuantityTimesInternationalRateTimesPriorityShipping()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Priority,
                Items = new List<Item> { new Item { Quantity = 1 } },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * 2.0 * 1);


        }

        [Fact]
        public void CalculateShippingCosts_InternationalExpressShippingOneItemQuantity1_ReturnsQuantityTimesInternationalRateTimesExpressShipping()
        {
            var address = new Address { City = "city", Country = "country", Street = "street" };

            var sut = new ShippingCalculator(address);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Express,
                Items = new List<Item> { new Item { Quantity = 1 } },
                ShippingAddress = new Address { City = "city2", Country = "country2", Street = "street2" }
            };

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * 2.5 * 1);


        }

        #endregion
    }
}
