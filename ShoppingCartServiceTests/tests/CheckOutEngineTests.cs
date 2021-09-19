using Xunit;
using FluentAssertions;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.BusinessLogic;
using AutoMapper;
using ShoppingCartService.Mapping;

namespace ShoppingCartServiceTests.tests
{
    public class CheckOutEngineTests
    {
        private readonly IMapper _mapper;

        private const double productPrice = 2;
        private const uint productQuantity = 3;
        private const double customerDiscount = 0.9;

        public CheckOutEngineTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void CalculateTotals_StandardCustomer_NoCustomerDiscount()
        {
            var address = new Address { Country = "country", City = "city", Street = "street" };
            var shippingCalculator = new ShippingCalculator(address);
            var sut = new CheckOutEngine(shippingCalculator, _mapper);
            var cart = new Cart {
                CustomerType = CustomerType.Standard,
                Items = new() { new Item { ProductId = "prod-1", Price = productPrice, Quantity = productQuantity } },
                ShippingAddress = address
            };
            
            var result = sut.CalculateTotals(cart);

            result.CustomerDiscount.Should().Be(0);
        }

        [Fact]
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping()
        {
            var originAddress = new Address { Country = "country", City = "city", Street = "street" };
            var destinationAddress = new Address { Country = "country", City = "city2", Street = "street2" };

            var shippingCalculator = new ShippingCalculator(originAddress);
            var sut = new CheckOutEngine(shippingCalculator, _mapper);
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new() { new Item { ProductId = "prod-1", Price = productPrice, Quantity = productQuantity } },
                ShippingAddress = destinationAddress
            };

            var result = sut.CalculateTotals(cart);

            result.Total.Should().Be(productPrice * productQuantity + result.ShippingCost);
        }

        [Fact]
        public void CalculateTotals_StandardCustomerWithMultipleProducts_TotalEqualsCostPlusShipping()
        {
            // TODO: check for multiple products
        }

        [Fact]
        public void CalculateTotal_PremiumCustomer_HasDiscount()
        {
            var address = new Address { Country = "country", City = "city", Street = "street" };
            var shippingCalculator = new ShippingCalculator(address);
            var sut = new CheckOutEngine(shippingCalculator, _mapper);
            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                Items = new() { new Item { ProductId = "prod-1", Price = 2, Quantity = 3 } },
                ShippingAddress = address
            };

            var result = sut.CalculateTotals(cart);

            result.CustomerDiscount.Should().Be(10);
        }

        [Fact]
        public void CalculateTotal_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var originAddress = new Address { Country = "country", City = "city", Street = "street" };
            var destinationAddress = new Address { Country = "country", City = "city2", Street = "street2" };

            var shippingCalculator = new ShippingCalculator(originAddress);
            var sut = new CheckOutEngine(shippingCalculator, _mapper);
            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                Items = new() { new Item { ProductId = "prod-1", Price = productPrice, Quantity = productQuantity } },
                ShippingAddress = destinationAddress
            };

            var result = sut.CalculateTotals(cart);

            result.Total.Should().Be((productPrice * productQuantity + result.ShippingCost) * customerDiscount);
        }
    }

   

}
