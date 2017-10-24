using System;
using System.Linq;
using LKTicket.Models;
using Dapper;

namespace LKTicket.Repositoires
{
    public static class PaymentRepository
    {
        internal static int CreatePayment(AppDb db, int orderId, PaymentRequest paymentRequest)
        {
            var transaction = db.Connection.BeginTransaction();
            try
            {
                var transactionId = TransactionRepository.CreateTransaction(db, paymentRequest.UserId, orderId, paymentRequest.ProfileId, transaction);
                var tickets = db.Connection.Query<Ticket>("SELECT t.* FROM tickets as t join seats as s on t.id = s.active_ticket_id where order_id = @orderId AND paid = false FOR UPDATE", new { orderId });
                var totalAmount = tickets.Sum(t => t.Price);
                if (paymentRequest.Amount != totalAmount)
                {

                    throw new Exception("Amount is wrong");
                }
                db.Connection.Execute(@"insert Payments(transaction_id, order_id, amount, paymentmethod, paymentreference)  values (@transactionId, @orderId, @totalAmount, @paymentmethod, @paymentreference)", new { transactionId, orderId, totalAmount, paymentRequest.PaymentMethod, paymentRequest.PaymentReference }, transaction);
                var paymentId = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();

                foreach (var ticket in tickets)
                {
                    TicketRepository.SetTicketAsPaid(db, ticket, transactionId, transaction);
                }
                transaction.Commit();
                return paymentId;
            }
            catch(Exception)
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}
