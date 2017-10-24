using System;
using System.Linq;
using LKTicket.Models;
using Dapper;
using System.Security.Cryptography;
using LKTicket.Data;
using System.Collections.Generic;

namespace LKTicket.Repositoires
{
    public static class UserRespository
    {
        internal static int Create(AppDb db, UserRequest request)
        {
                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Connection.Execute(@"insert Users(name, email) values (@name, @email)", new { request.Name, request.Email }, transaction);
                    var id = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
                    transaction.Commit();
                    return id;
                }
        }
        internal static UserResponse GetUser(AppDb db, int id)
        {
            var customers = db.Connection.Query<UserResponse>("select * from users where id = @Id", new { Id = id });
            return customers.FirstOrDefault();
        }
        internal static IEnumerable<UserResponse> GetUsers(AppDb db)
        {
            return db.Connection.Query<UserResponse>("select * from users");
        }
    }
}
