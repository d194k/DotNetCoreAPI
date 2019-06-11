using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreDAL.Models
{
    public class OrionDbContext : DbContext
    {
        public OrionDbContext(DbContextOptions<OrionDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; private set; }
    }

    public class Employee
    {
        [Required]
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public decimal annualsalary { get; set; }
        public DateTime dateofbirth { get; set; }
    }
}
