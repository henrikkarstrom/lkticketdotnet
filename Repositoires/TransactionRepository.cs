using System;
using System.Linq;
using Dapper;
using System.Data;

namespace LKTicket.Repositoires
{
    public static class TransactionRepository
    {
        internal static int CreateTransaction(AppDb db, int userId, int orderId, int profileId, IDbTransaction transaction)
        {

            db.Connection.Execute(@"insert Transactions(user_id, order_id, profile_id, date) values (@userId, @orderId, @profileId, @date)", new { date = DateTime.UtcNow, userId, orderId, profileId }, transaction);
            return db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
        }


    }
}
