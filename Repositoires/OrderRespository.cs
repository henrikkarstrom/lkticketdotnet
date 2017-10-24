using System;
using System.Linq;
using LKTicket.Models;
using Dapper;
using System.Security.Cryptography;
using LKTicket.Data;
using System.Net;
using System.Collections.Generic;

namespace LKTicket.Repositoires
{
    public static class OrderRespository
    {
        internal static OrderResponse CreateOrder(AppDb db)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                string token = Convert.ToBase64String(tokenData).Substring(0,7);
                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Connection.Execute(@"insert Orders(expires, identifier) values (@expires, @identifier)", new { expires = DateTime.Now.AddMinutes(30), identifier = token }, transaction);
                    var id = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
                    transaction.Commit();
                    return GetOrder(db, id);
                }
            }
        }
        internal static OrderResponse GetOrder(AppDb db, int id)
        {
            var customers = db.Connection.Query<OrderResponse>("select * from Orders where id = @Id", new { Id = id });
            return customers.FirstOrDefault();
        }
        internal static void DeleteTicketsForOrder(AppDb db, int orderId, int userId, int profileId)
        {
            using (var transaction = db.Connection.BeginTransaction())
            {
                var transactionId = TransactionRepository.CreateTransaction(db, userId, orderId, profileId, transaction);
                var tickets = db.Connection.Query<Ticket>("select * from tickets where order_id = @orderId FOR UPDATE", new { orderId });
                if (tickets.Any(t => t.Paid))
                {
                    transaction.Rollback();
                    throw new InternalException(HttpStatusCode.BadRequest, "There are paid tickets in order");
                }
                foreach (var ticket in tickets)
                {
                    TicketRepository.RemoveTicket(db, ticket, transactionId, profileId, transaction);
                }
                transaction.Commit();
            }
        }

        internal static List<OrderResponse> GetOrdersForCustomer(AppDb db, int id)
        {
            return db.Connection.Query<OrderResponse>("select * from Orders where customer_id = @Id", new { Id = id }).ToList();
        }

        internal static void AddTicketsToOrder(AppDb db, int id, TicketRequest ticketRequest)
        {
            using (var transaction = db.Connection.BeginTransaction())
            {

                var price = PricesRepository.GetPrice(db, ticketRequest.CategoryId, ticketRequest.RateId);
                if (price == null)
                    throw new InternalException(HttpStatusCode.BadRequest, "Price not defined");

                var transactionId = TransactionRepository.CreateTransaction(db, ticketRequest.UserId, id, ticketRequest.ProfileId, transaction);
                var seats = db.Connection.Query<Seat>("select * from seats where active_ticket_id is null AND category_Id = @categoryId and performance_id = @performanceId LIMIT @count FOR UPDATE;", new { ticketRequest.CategoryId, ticketRequest.PerformanceId, ticketRequest.Count }, transaction).ToList();
                if (seats.Count != ticketRequest.Count)
                {
                    transaction.Rollback();
                    throw new InternalException(HttpStatusCode.Conflict, "Unable to find tickets");
                }

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {

                    foreach (var seat in seats)
                    {
                        byte[] tokenData = new byte[32];
                        rng.GetBytes(tokenData);

                        string token = Convert.ToBase64String(tokenData).Substring(0, 7);
                        TicketRepository.CreateTicket(db, id, seat.Id, seat.PerformanceId, ticketRequest.RateId, price.Price, transactionId, token, transaction);
                    }
                }
                transaction.Commit();
            }
        }
    }
}
