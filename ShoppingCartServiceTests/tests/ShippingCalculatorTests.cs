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
using ShoppingCartServiceTests.builders;

namespace ShoppingCartServiceTests.tests
{
    public class ShippingCalculatorTests
    {
        private static Item ITEM_1 = new ItemBuilder().WithQuantity(1).Build();
        private static Item ITEM_2 = new ItemBuilder().WithQuantity(5).Build();
        
        public static List<object[]> ItemListsWithExpectedQuantity()
        {
            return new()
            {
                new object[] { new List<Item>(), 0 },
                new object[] { new List<Item>() { ITEM_1 }, ITEM_1.Quantity },
                new object[] { new List<Item>() { ITEM_2 }, ITEM_2.Quantity },
                new object[] { new List<Item>() { ITEM_1, ITEM_2 }, (ITEM_1.Quantity + ITEM_2.Quantity) }
            };
        }

        #region Same city
        
        [Theory]
        [MemberData(nameof(ItemListsWithExpectedQuantity))]
        public void SameCity_StandardShipping(List<Item> items, uint expected)
        {
            var address = new AddressBuilder().Build();
            var shippingAddress = new AddressBuilder().WithStreet("street2").Build();
            var sut = new ShippingCalculator(address);


            var cart = new CartBuilder().WithShippingAddress(shippingAddress).WithItems(items).Build();

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCityRate * expected);
        }


        #endregion

        #region Same country 
        [Theory]
        [MemberData(nameof(ItemListsWithExpectedQuantity))]
        public void SameCountry_StandardShipping(List<Item> items, uint expected)
        {
            var address = new AddressBuilder().Build();
            var shippingAddress = new AddressBuilder().WithCity("city2").Build();
            var sut = new ShippingCalculator(address);


            var cart = new CartBuilder().WithShippingAddress(shippingAddress).WithItems(items).Build();

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.SameCountryRate * expected);
        }

        #endregion

        #region International
        [Theory]
        [MemberData(nameof(ItemListsWithExpectedQuantity))]
        public void International_StandardShipping(List<Item> items, uint expected)
        {
            var address = new AddressBuilder().Build();
            var shippingAddress = new AddressBuilder().WithCountry("country2").Build();
            var sut = new ShippingCalculator(address);


            var cart = new CartBuilder().WithShippingAddress(shippingAddress).WithItems(items).Build();

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * expected);
        }

        
        #endregion

        #region Shipping Speeds
        public static List<object[]> ShippingMethodsWithRates()
        {
            return new()
            {
                new object[] { ShippingMethod.Standard, 1.0 },
                new object[] { ShippingMethod.Expedited, 1.2 },
                new object[] { ShippingMethod.Priority, 2.0 },
                new object[] { ShippingMethod.Express, 2.5 }
            };
        }

        [Theory]
        [MemberData(nameof(ShippingMethodsWithRates))]
        public void CalculateShippingCosts_InternationalShipping_ReturnsShippingRateTimesShippingSpeed(ShippingMethod shippingMethod, double shippingSpeedRate)
        {
            var address = new AddressBuilder().Build();
            var shippingAddress = new AddressBuilder().WithCity("city2").WithCountry("country2").Build();
            var sut = new ShippingCalculator(address);

            var items = new List<Item> { new ItemBuilder().Build() };
            var cart = new CartBuilder().WithShippingAddress(shippingAddress).WithShippingMethod(shippingMethod).WithItems(items).Build();

            var result = sut.CalculateShippingCost(cart);

            result.Should().Be(ShippingCalculator.InternationalShippingRate * shippingSpeedRate * 1);
        }

        #endregion
    }
}
