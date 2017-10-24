using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKTicket.Models;
using Dapper;
using System.Data;

namespace LKTicket.Repositoires
{
    public static class CustomersRepository
    {
        internal static int CreateCustomer(AppDb db, Customer value)
        {
            int id = db.Connection.Execute(@"insert Customers(name, email, phone) values (@name, @email, @phone)", new[] { value });
            return id;
        }

        internal static CustomerResponse GetCustomer(AppDb db, int id)
        {
            var customers = db.Connection.Query<CustomerResponse>("select * from Customers where id = @Id", new { Id = id });
            return customers.FirstOrDefault();
        }
        internal static CustomerResponse GetCustomers(AppDb db)
        {
            var customers = db.Connection.Query<CustomerResponse>("select * from Customers");
            return customers.FirstOrDefault();
        }
    }
    public static class SeatRepository
    {
        internal static void CreateSeatsForShow(AppDb db, int performanceId, int categoryId, int nbrOfTickets, int profileId, IDbTransaction transaction)
        {
            for (int i = 0; i <= nbrOfTickets; i++)
            {
                db.Connection.Execute(@"insert Seats(performance_id, category_id, profile_id) values (@performanceId, @categoryId, @profileId)", new { performanceId, categoryId, profileId }, transaction);
            }
        }
    }

    public static class ProfileRepository
    {
        internal static int CreateProfile(AppDb db, ProfileRequest value)
        {
            var transaction = db.Connection.BeginTransaction();
            db.Connection.Execute(@"insert Profiles(name) values (@name)", new[] {value }, transaction);
            var profileId = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
            transaction.Commit();
            return profileId;
        }

        internal static List<ProfileResponse> GetProfiles(AppDb db)
        {
            return db.Connection.Query<ProfileResponse>(@"select * from profiles").ToList();
        }
    }
    public static class PerformancesRepository
    {
        internal static int CreatePerformance(AppDb db, int showId, PerformanceRequest value)
        {
            var transaction = db.Connection.BeginTransaction();
            db.Connection.Execute(@"insert Performances(show_id, start) values (@showId, @start)", new { showId , value.Start }, transaction);
            var performanceId = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
            var categorties = CategoriesRepository.GetCategoriesForShow(db, showId, transaction);
            foreach(var category in categorties)
            {
                SeatRepository.CreateSeatsForShow(db, performanceId, category.Id, category.TicketCount, value.DefaultProfileId, transaction);
            }
            transaction.Commit();
            return performanceId;
        }

        internal static List<PerformanceResponse> GetPerformancesForShow(AppDb db, int id)
        {
            var performances = db.Connection.Query<PerformanceResponse>("select * from Performances where show_id = @Id", new { Id = id });
            return performances.ToList();
        }
        internal static PerformanceResponse GetPerformance(AppDb db, int id)
        {
            var performances = db.Connection.Query<PerformanceResponse>("select * from Performances where id = @Id", new { Id = id });
            return performances.FirstOrDefault();
        }
    }
    }
