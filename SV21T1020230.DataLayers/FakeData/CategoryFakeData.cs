using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers.FakeData
{
    public class CategoryFakeData
    {
        private static List<Category> categories = new List<Category>()
        {
            new Category(){ CategoryID = 1, CategoryName = "Quần áo", Description = "Description 1"},
            new Category(){ CategoryID = 2, CategoryName = "Giày dép", Description = "Description 2"},
            new Category(){ CategoryID = 3, CategoryName = "Đồng hồ", Description = "Description 3"},
            new Category(){ CategoryID = 4, CategoryName = "Điện thoại", Description = "Description 4"},
            new Category(){ CategoryID = 5, CategoryName = "Laptop", Description = "Description 5"},
            new Category(){ CategoryID = 6, CategoryName = "Trái cây", Description = "Description 6"},
            new Category(){ CategoryID = 7, CategoryName = "Đèn", Description = "Description 7"},
            new Category(){ CategoryID = 8, CategoryName = "Ô tô", Description = "Description 8"}
        };
        public static List<Category> GetListCategory()
        {
            return categories;
        }
    }
}
