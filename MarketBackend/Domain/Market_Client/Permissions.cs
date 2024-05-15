using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public enum Permission
    {
        addProduct,
        removeProduct,
        updateProductPrice,
        updateProductDiscount,
        updateProductQuantity,
        all = addProduct | removeProduct | updateProductPrice | updateProductDiscount | updateProductQuantity
    }
}
