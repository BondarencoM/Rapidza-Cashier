using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidzaCashier.dto
{
    class OrderedProductDto
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public OrderedProductDto(string name, string image)
        {
            this.Name = name;
            this.Image = image;
        }
    }
}
