using Dapper;
using LKTicket.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKTicket.Repositoires
{
    public class ShowsRepository
    {
            internal static ShowResponse CreateShow(AppDb db, ShowRequest value)
        {
            var transaction = db.Connection.BeginTransaction();
            db.Connection.Execute(@"insert Shows(name, description) values (@name, @description)", new[] { value });
            var id = db.Connection.Query<int>("SELECT LAST_INSERT_ID();", transaction: transaction).First();
            transaction.Commit();
            return GetShow(db, id);
        }

            internal static ShowResponse GetShow(AppDb db, int id)
            {
                var shows = db.Connection.Query<ShowResponse>("select * from Shows where id = @Id", new { Id = id });
                return shows.FirstOrDefault();
            }
            internal static List<ShowResponse> GetShows(AppDb db)
            {
                var shows = db.Connection.Query<ShowResponse>("select * from Shows");
                return shows.ToList();
            }

        internal static IEnumerable<PerformanceWithCategories> GetPerformancesForShowWhitAvibility(AppDb db, int id, int profileId)
        {
            string sql = "select p.*, c.*, count(s.id) as tickets from performances as p join categories as c on c.show_id = p.show_id left join seats as s on s.performance_id = p.id and s.category_id = c.id and s.profile_id = @profileId and s.active_ticket_id is null where p.show_id = @showId group by p.id, c.id;";
            var lookup = new Dictionary<int, PerformanceWithCategories>();
            db.Connection.Query<PerformanceWithCategories, CategoryWithAviablity, PerformanceWithCategories>(sql, (p, c) =>
            {
                PerformanceWithCategories performance;
                if (!lookup.TryGetValue(p.Id, out performance))
                {
                    lookup.Add(p.Id, p);
                    p.Categories = new List<CategoryWithAviablity>();
                    performance = p;
                }
                performance.Categories.Add(c);
                return performance;
            }, new { showId = id, profileId });
            return lookup.Values;

        }
        internal static IEnumerable<PerformanceWithProfileData> GetPerformancesForShowWhitProfileData(AppDb db, int id)
        {
            string sql = "select p.*, c.*, pro.*, sum(s.active_ticket_id is not null) as reservated, count(s.id) as total from performances as p join categories as c on c.show_id = p.show_id join profiles as pro left join seats as s on s.performance_id = p.id and s.category_id = c.id and s.profile_id = pro.id where p.show_id = @showId group by p.id, c.id, s.profile_id, s.active_ticket_id is null";
            var lookup = new Dictionary<int, PerformanceWithProfileData>();
            db.Connection.Query<PerformanceWithProfileData, CategoryWithProfileData,ProfilesWithData, PerformanceWithProfileData>(sql, (p, c, pro) =>
            {
                PerformanceWithProfileData performance;
                if (!lookup.TryGetValue(p.Id, out performance))
                {
                    lookup.Add(p.Id, p);
                    p.CategoriesDictionary = new Dictionary<int, CategoryWithProfileData>();
                    performance = p;
                }

                CategoryWithProfileData category;
                if (!performance.CategoriesDictionary.TryGetValue(c.Id, out category))
                {
                    performance.CategoriesDictionary.Add(c.Id, c);
                    c.Profiles = new List<ProfilesWithData>();
                    category = c;
                }

                category.Profiles.Add(pro);
                return performance;
            }, new { showId = id });
            return lookup.Values;

        }
        public class PerformanceWithProfileData
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public List<CategoryWithProfileData> Categories { get { return CategoriesDictionary.Values.ToList(); } }
            [JsonIgnore]
            public Dictionary<int, CategoryWithProfileData> CategoriesDictionary { get; set; }
        }
        public class CategoryWithProfileData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Tickets { get; set; }
            public List<ProfilesWithData> Profiles { get; set; }
        }
        public class ProfilesWithData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Aviable { get { return Total - Reservated; } }
            public int Reservated { get; set; }
            public int Total { get; set; }
        }
        public class PerformanceWithCategories
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public List<CategoryWithAviablity> Categories { get; set; }
        }
        public class CategoryWithAviablity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Tickets { get; set; }
        }
    }
}
