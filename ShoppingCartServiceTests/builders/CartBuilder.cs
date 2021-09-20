using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartServiceTests.builders
{
    public class CartBuilder
    {
        private string _id = "cart1";
        private string _customerId = "customer1";
        private CustomerType _customerType = CustomerType.Standard;
        private ShippingMethod _shippingMethod = ShippingMethod.Standard;
        private Address _shippingAddress = new AddressBuilder().Build();
        private List<Item> _items = new List<Item>();

        public Cart Build()
        {
            return new Cart() { Id = _id, CustomerId = _customerId, CustomerType = _customerType, ShippingMethod = _shippingMethod, ShippingAddress = _shippingAddress, Items = _items};
        }

        public CartBuilder WithId(string id)
        {
            this._id = id;
            return this;
        }

        public CartBuilder WithCustomerId(string customerId)
        {
            this._customerId = customerId;
            return this;
        }

        public CartBuilder WithCustomerType(CustomerType customerType)
        {
            this._customerType = customerType;
            return this;
        }

        public CartBuilder WithShippingMethod(ShippingMethod shippingMethod)
        {
            this._shippingMethod = shippingMethod;
            return this;
        }

        public CartBuilder WithShippingAddress(Address shippingAddress)
        {
            this._shippingAddress = shippingAddress;
            return this;
        }

        public CartBuilder WithItems(List<Item> items)
        {
            this._items = items;
            return this;
        }
    }
}
