using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dapper;
using LKTicket.Models;

namespace LKTicket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO: Make gerneric
            var mapper = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(ColumnAttributeTypeMapper<>).MakeGenericType(typeof(RateResponse)));
            SqlMapper.SetTypeMap(typeof(RateResponse), mapper);
            mapper = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(ColumnAttributeTypeMapper<>).MakeGenericType(typeof(CategoryResponse)));
            SqlMapper.SetTypeMap(typeof(CategoryResponse), mapper);
            mapper = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(ColumnAttributeTypeMapper<>).MakeGenericType(typeof(PriceResponse)));
            SqlMapper.SetTypeMap(typeof(PriceResponse), mapper);
            mapper = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(ColumnAttributeTypeMapper<>).MakeGenericType(typeof(PriceWithNameResponse)));
            SqlMapper.SetTypeMap(typeof(PriceWithNameResponse), mapper);
            mapper = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(ColumnAttributeTypeMapper<>).MakeGenericType(typeof(Ticket)));
            SqlMapper.SetTypeMap(typeof(Ticket), mapper);
            mapper = (SqlMapper.ITypeMap)Activator.CreateInstance(typeof(ColumnAttributeTypeMapper<>).MakeGenericType(typeof(TicketResponse)));
            SqlMapper.SetTypeMap(typeof(TicketResponse), mapper);

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
