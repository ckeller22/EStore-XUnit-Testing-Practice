using ShoppingCartService.DataAccess.Entities;


namespace ShoppingCartServiceTests.builders
{
    public class ItemBuilder
    {
        private string _productId = "prod1";
        private string _productName = "product1";
        private double _price = 10.0;
        private uint _quantity = 1;

        public Item Build() {  return new Item() { ProductId = _productId, ProductName = _productName, Price = _price, Quantity = _quantity}; }

        public ItemBuilder WithProductId(string productId)
        {
            this._productId = productId;
            return this;
        }

        public ItemBuilder WithProductName(string productName)
        {
            this._productName = productName;
            return this;
        }

        public ItemBuilder WithPrice(double price)
        {
            this._price = price;
            return this;
        }

        public ItemBuilder WithQuantity(uint quantity)
        {
            this._quantity = quantity;
            return this;
        }
    }
}
