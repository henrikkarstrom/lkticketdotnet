using System.Linq;
using Dapper;
using System.Data;
using LKTicket.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace LKTicket.Repositoires
{
 public static class TicketRepository
 {
 public const int TicketTransactionActivityReserveration = 1;
 public const int TicketTransactionActivityPayment = 2;


        public const int TicketTransactionActivityRemoveReserveration = 11;
        public const int TicketTransactionActivityRemovePayment = 12;

        internal static List<TicketResponse> GetTicketsForOrder(AppDb db, int orderId, bool onlyActive)
 {
            if(onlyActive)
     return db.Connection.Query<TicketResponse>("SELECT t.*, (s.active_ticket_id = t.id) as active, c.id as category_id, c.name as category_name, r.name as rate_name, sh.name as show_name, p.start, s.id as show_id, p.id as performance_id FROM `tickets` as t LEFT JOIN `rates` as r ON t.`rate_id` = r.`id` LEFT JOIN `seats` as s ON t.`seat_id` = s.`id` LEFT JOIN `categories` as c ON s.`category_id` = c.`id` LEFT JOIN `performances` as p ON s.`performance_id` = p.`id` LEFT JOIN `shows` as sh ON p.`show_id` = sh.`id` where t.order_id = @orderId and s.active_ticket_id = t.id", new {orderId}).ToList();
            else
                return db.Connection.Query<TicketResponse>("SELECT t.*, (s.active_ticket_id = t.id) as active, c.name as category_name, c.id as category_id, r.name as rate_name, sh.name as show_name, p.start, s.id as show_id, p.id as performance_id FROM `tickets` as t LEFT JOIN `rates` as r ON t.`rate_id` = r.`id` LEFT JOIN `seats` as s ON t.`seat_id` = s.`id` LEFT JOIN `categories` as c ON s.`category_id` = c.`id` LEFT JOIN `performances` as p ON s.`performance_id` = p.`id` LEFT JOIN `shows` as sh ON p.`show_id` = sh.`id` where t.order_id = @orderId", new { orderId }).ToList();

        }

         internal static void CreateTicket(AppDb db, int orderId, int seatId, int performanceId, int rateId, int price, int transactionId, string identifier, IDbTransaction transaction)
         {
         db.Connection.Execute(@"insert Tickets(order_id, seat_id, rate_id, price, identifier, paid, printed, scanned, confirmed) values ( @orderId, @seatId, @rateId, @price, @identifier, @paid, @printed, @scanned, @confirmed)", new { orderId, seatId, rateId, price, identifier, paid = false, printed = false, scanned = false, confirmed = true }, transaction);
         var ticketId = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
         db.Connection.Execute(@"update Seats set active_ticket_id = @ticketId where id = @seatId", new { ticketId, seatId });
         CreateTicketTransaction(db, transactionId,ticketId, TicketTransactionActivityReserveration, transaction);
         }

         private static void CreateTicketTransaction(AppDb db, int transactionId, int ticketId, int activityId, IDbTransaction transaction)
         {
         db.Connection.Execute(@"insert ticket_transactions(ticket_id, transaction_id, activity) values ( @ticketId, @transactionId, @activityId)", new { ticketId, transactionId, activityId }, transaction);
         }

        internal static void RemoveTicket(AppDb db, Ticket ticket, int transactionId, int newProfileId, MySqlTransaction transaction)
        {
            if (ticket.Paid)
                throw new Exception("Ticket paid");
            db.Connection.Execute(@"update Seats set active_ticket_id = null, profile_id = @newProfileId where id = @seatId AND active_ticket_id = @ticketId", new { ticketId = ticket.Id, seatId = ticket.SeatId, newProfileId });
            CreateTicketTransaction(db, transactionId, ticket.Id, TicketTransactionActivityRemoveReserveration, transaction);
        }
        internal static void RemoveTicketAndRepay(AppDb db, Ticket ticket, int transactionId, int newProfileId, MySqlTransaction transaction)
        {
            if (!ticket.Paid)
                throw new Exception("Ticket not paid");
            db.Connection.Execute(@"update tickets set paid = false where id = @ticketId", new { ticketId = ticket.Id });
            CreateTicketTransaction(db, transactionId, ticket.Id, TicketTransactionActivityRemovePayment, transaction);
            ticket.Paid = false;
            RemoveTicket(db, ticket, transactionId, newProfileId, transaction);
        }

        internal static void SetTicketAsPaid(AppDb db, Ticket ticket, int transactionId, MySqlTransaction transaction)
        {
            if (ticket.Paid)
                throw new Exception("Ticket already paid");
            db.Connection.Execute(@"update tickets set paid = true where id = @ticketId", new { ticketId = ticket.Id});
            ticket.Paid = true;
            CreateTicketTransaction(db, transactionId, ticket.Id, TicketTransactionActivityPayment, transaction);
        }
    }

}
