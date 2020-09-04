
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lecom.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public ApplicationDbContext(IConfiguration config)
        {
            _config = config;
            Database.SetCommandTimeout(3660);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"]);
        }
    }
}
