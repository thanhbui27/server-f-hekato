﻿using System.Collections.ObjectModel;

namespace DoAn.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string Image_Url { get; set; }
        
        public int quantity { get; set; }
        
        public decimal PriceNew { get; set; }
        public decimal PriceOld { get; set; }

        public string ShortDetails { get; set; }
        public string ProductDescription { get; set; }

        public DateTime? dateAdd { get; set; }
        public int CategoryId { get; set; }

        public List<ProductInCategory> GetsProductInCategories { get; set; }
        public List<ProductImage> GetsProductImage { get; set; }

        public ProductActions? productAction { get; set; }

        public Cart? cart { get; set; }

        public List<OrderDetails> orderDetails { get; set; }

        public List<CommentsProducts> comments { get; set; }
    }
}
