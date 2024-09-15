using APICatalago.Context;
using APICatalago.DTOs.Mappings;
using APICatalago.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalagoxUnitTest.UnitTests
{
    public class ProdutosUnitTestController
    {
        public IUnitOfWork repository;
        public IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost\\SQLEXPRESS;Database=CatalagoDB;Trusted_Connection=True;TrustServerCertificate=True";

        static ProdutosUnitTestController()
        {

            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
        }

        public ProdutosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProdutoDTOMappingProfile());
            });

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            repository = new UnityOfWork(context);
        }

    }
}
