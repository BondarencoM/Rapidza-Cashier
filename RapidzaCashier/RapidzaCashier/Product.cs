using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier
{
    struct Product
    {
        public string Name { get; }
        public string Image { get; }
        public double Price { get; }
        public string PriceString { get => Price + "€"; }

        public Product(string name, string image, double price)
        {
            Name = name;
            Image = image;
            Price = price;
        }
        public Product(string name, double price)
        {
            Name = name;
            Image = "/res/no image available .png";
            Price = price;
        }
    }
}
