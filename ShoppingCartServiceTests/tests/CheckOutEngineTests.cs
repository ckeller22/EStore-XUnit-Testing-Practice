using Xunit;
using FluentAssertions;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.BusinessLogic;
using AutoMapper;
using ShoppingCartService.Mapping;
using ShoppingCartServiceTests.builders;

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

        [Theory]
        [InlineData(CustomerType.Premium, 10)]
        [InlineData(CustomerType.Standard, 0)]
        public void CalculateTotals_DiscountBasedOnCustomerType(CustomerType customerType, uint discount)
        {
            var address = new AddressBuilder().Build();
            var shippingCalculator = new ShippingCalculator(address);
            var sut = new CheckOutEngine(shippingCalculator, _mapper);

            var cart = new CartBuilder().WithCustomerType(customerType).WithShippingAddress(address).Build();

            var result = sut.CalculateTotals(cart);

            result.CustomerDiscount.Should().Be(discount);
        }


        [Fact]
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping()
        {
            var originAddress = new AddressBuilder().Build();
            var destinationAddress = new AddressBuilder().WithCity("city2").WithStreet("street2").Build();

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
            var originAddress = new AddressBuilder().Build();
            var destinationAddress = new AddressBuilder().WithCity("city2").WithStreet("street2").Build();

            var shippingCalculator = new ShippingCalculator(originAddress);
            var sut = new CheckOutEngine(shippingCalculator, _mapper);
            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new() { new Item { ProductId = "prod-1", Price = productPrice, Quantity = productQuantity }, new Item { ProductId = "prod-2", Price = 4, Quantity = productQuantity } },
                ShippingAddress = destinationAddress
            };

            var result = sut.CalculateTotals(cart);

            result.Total.Should().Be((productPrice * productQuantity) + (4 * productQuantity) + result.ShippingCost);
        }

        [Fact]
        public void CalculateTotal_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var originAddress = new AddressBuilder().Build();
            var destinationAddress = new AddressBuilder().WithCity("city2").WithStreet("street2").Build();

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
