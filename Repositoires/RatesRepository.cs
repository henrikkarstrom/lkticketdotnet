using Dapper;
using LKTicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKTicket.Repositoires
{
    public class RatesRepository
    {
            internal static RateResponse CreateRate(AppDb db, int showId, RateRequest value)
        {
            var transaction = db.Connection.BeginTransaction();
            db.Connection.Execute(@"insert Rates(name, show_id) values (@name, @showId)", new { value.Name, showId });
            var id = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
            transaction.Commit();
            return GetRate(db, id);
        }

            internal static RateResponse GetRate(AppDb db, int id)
            {
                var response = db.Connection.Query<RateResponse>("select * from Rates where id = @Id", new { Id = id });
                return response.FirstOrDefault();
            }
            internal static List<RateResponse> GetRates(AppDb db)
            {
                var response = db.Connection.Query<RateResponse>("select * from Rates");
                return response.ToList();
            }
        internal static List<RateResponse> GetRatesForShow(AppDb db, int id)
        {
            var response = db.Connection.Query<RateResponse>("select * from Rates where show_id = @id", new { id });
            return response.ToList();
        }
    }
}
