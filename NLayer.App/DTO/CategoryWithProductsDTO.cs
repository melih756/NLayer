using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTO
{
    public class CategoryWithProductsDTO:CategoryDTO
    {
        public List<ProductDTO> Products { get; set; }
    }
}
