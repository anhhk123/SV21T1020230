﻿using Dapper;
using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers.SQLSever
{
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Category data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    INSERT INTO Categories(CategoryName, Description) 
                    VALUES (@CategoryName, @Description);

                    SELECT @@IDENTITY
                ";
                var param = new
                {
                    CategoryName = data.CategoryName,
                    Description = data.Description
                };
                id = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text);
                connection.Close();
            };
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    SELECT count(*) 
                    FROM Categories
                    WHERE CategoryName LIKE @searchValue
                ";
                var param = new
                {
                    searchValue = $"%{searchValue}%"
                };
                count = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    DELETE FROM Categories
                    WHERE CategoryID = @CategoryID
                ";
                var param = new
                {
                    CategoryID = id
                };
                result = connection.Execute(sql, param, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Category? Get(int id)
        {
            Category? category = null;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    SELECT * 
                    FROM Categories
                    WHERE CategoryID = @CategoryID 
                ";
                var param = new
                {
                    CategoryID = id
                };
                category = connection.QueryFirstOrDefault<Category>(sql, param, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return category;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    SELECT COUNT(*) 
                    FROM Products 
                    WHERE CategoryID = @CategoryID
                ";
                var param = new
                {
                    CategoryID = id
                };
                result = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// danh sách category nếu truyền tham số thì có phân trang tìm kiếm nếu ko có tìm lấy hết
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> result = new List<Category>();
            using (var connection = OpenConnection())
            {
                var sql = @"
                            SELECT * 
                            FROM (
		                            SELECT *, 
			                            ROW_NUMBER() OVER (ORDER BY CategoryName) AS RowNumber
		                            FROM Categories
		                            WHERE CategoryName LIKE @searchValue 
	                            ) AS t
                            WHERE @pageSize = 0
	                            OR (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                            ORDER BY RowNumber";
                var param = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = $"%{searchValue}%"
                };
                result = connection.Query<Category>(sql, param, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return result;
        }

        public bool Update(Category data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                        UPDATE Categories
                        SET CategoryName = @CategoryName,
	                        Description = @Description
                        WHERE CategoryID = @CategoryID
                       ";
                var param = new
                {
                    CategoryName = data.CategoryName ?? "",
                    Description = data.Description ?? "",
                    CategoryID = data.CategoryId
                };
                result = connection.Execute(sql, param, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
