
using System.Collections.Generic;

namespace P2FixAnAppDotNetCode.Models
{
    public interface ICart
    {
        IEnumerable<CartLine> Lines { get; }
        void AddItem(Product product, int quantity);

        void RemoveLine(Product product);

        void Clear();

        double GetTotalValue();

        double GetAverageValue();
    }
}