using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    class Product
    {
        public string Name { get; }
        public string Image { get; }
        public double Price { get; }
        public string PriceString { get => Price + "€"; }

        protected Product()
        {
        }

        [JsonConstructor]
        public Product(string Name, string Image, double Price)
        {
            this.Name = Name;
            this.Image = Image;
            this.Price = Price;
        }
        public Product(string name, double price)
        {
            Name = name;
            Image = "/res/no image available .png";
            Price = price;
        }
        public Product(Product product)
        {
            Name = product.Name;
            Price = product.Price;
            Image = product.Image;
        }
    }
}
