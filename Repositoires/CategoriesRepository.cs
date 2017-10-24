using Dapper;
using LKTicket.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace LKTicket.Repositoires
{
    public static class CategoriesRepository
    {
        internal static CategoryResponse CreateCategory(AppDb db, int showId, CategoryRequest value)
        {
            var transaction = db.Connection.BeginTransaction();
            db.Connection.Execute(@"insert Categories(name, show_id, ticketCount) values (@name, @showId, @ticketCount)", new { value.Name, showId, value.TicketCount });
            var id = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
            transaction.Commit();
            return GetCategory(db, id);
        }

        internal static CategoryResponse GetCategory(AppDb db, int id)
        {
            var response = db.Connection.Query<CategoryResponse>("select * from Categories where id = @Id", new { Id = id });
            return response.FirstOrDefault();
        }
        internal static List<CategoryResponse> GetCategories(AppDb db)
        {
            var response = db.Connection.Query<CategoryResponse>("select * from Categories");
            return response.ToList();
        }
        internal static List<CategoryResponse> GetCategoriesForShow(AppDb db, int id, IDbTransaction transaction = null)
        {
            var response = db.Connection.Query<CategoryResponse>("select * from Categories where show_id = @id", new { id }, transaction);
            return response.ToList();
        }
    }

    public static class PricesRepository
    {
        internal static PriceResponse CreatePrice(AppDb db, int categoryId, int rateId, int price)
        {
            db.Connection.Execute(@"INSERT INTO Prices (rate_id,category_id, price) VALUES (@rateId,@categoryId,@price) ON DUPLICATE KEY UPDATE price = @price;", new { categoryId, rateId, price });
            return GetPrice(db, categoryId, rateId);
        }

        internal static PriceResponse GetPrice(AppDb db, int categoryId, int rateId)
        {
            var response = db.Connection.Query<PriceResponse>("select * from Prices where rate_id = @rateId and category_id = @categoryId", new { rateId, categoryId });
            return response.FirstOrDefault();
        }

        internal static void DeletePrice(AppDb db, int categoryId, int rateId)
        {
             db.Connection.Query<PriceResponse>("delete from Prices where rate_id = @rateId and category_id = @categoryId", new { rateId, categoryId });
        }

        internal static List<PriceResponse> GetPrice(AppDb db)
        {
            var response = db.Connection.Query<PriceResponse>("select * from Prices");
            return response.ToList();
        }
        internal static List<PriceResponse> GetPricesForCategory(AppDb db, int categoryId)
        {
            var response = db.Connection.Query<PriceResponse>("select * from Prices where category_id = @categoryId", new { categoryId });
            return response.ToList();
        }
        internal static List<PriceWithNameResponse> GetPricesForShow(AppDb db, int showId)
        {
            var response = db.Connection.Query<PriceWithNameResponse>("select p.*, c.name as category_name, r.name as rate_name from Prices as p join rates as r on r.id = p.rate_id join categories as c on c.id = p.category_id where r.show_id = @showId and c.show_id = @showId", new { showId });
            return response.ToList();
        }
    }
}
