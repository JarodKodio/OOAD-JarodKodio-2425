using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZalaStock
{
    class StockBeheer
    {
        public Dictionary<Product, int> Verkocht { get; private set; }
        public Dictionary<Product, int> Geretourneerd { get; private set; }

        public StockBeheer()
        {
            Verkocht = new Dictionary<Product, int>();
            Geretourneerd = new Dictionary<Product, int>();
        }

        public void Verkopen(Product product, int aantal)
        {
            product.Verkoop(aantal);

            if (Verkocht.ContainsKey(product))
                Verkocht[product] += aantal;
            else
                Verkocht[product] = aantal;
        }

        public void Retourneren(Product product, int aantal)
        {
            product.Retourneer(aantal);

            if (Geretourneerd.ContainsKey(product))
                Geretourneerd[product] += aantal;
            else
                Geretourneerd[product] = aantal;
        }
    }
}
