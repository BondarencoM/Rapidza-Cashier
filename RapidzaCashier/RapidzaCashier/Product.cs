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

        public Product(string name, string image, double price)
        {
            Name = name;
            Image = image;
            Price = price;
        }
        public Product(string name, double price)
        {
            throw new NotImplementedException("this constructor is not yet implemented");
            Name = name;
            Image = "Insert here path to default image";
            Price = price;
        }
    }
}
