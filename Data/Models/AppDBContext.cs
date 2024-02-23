using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan_current_repairs.Data.Models
{
    public class AppDBContext :DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<DictionarySector> DictionarySector { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<GroupNameOfWorks> Groups { get; set; }
        public DbSet<NameOfWorks> Works { get; set; }
        public DbSet<Jornal> Jornals { get; set; }
        public DbSet<PlanByMount> PlanValue { get; set; }
        public DbSet<FactByMount> FactValue { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Parameters_1> Parameters_1 { get; set; }
        public DbSet<Parameters_2> Parameters_2 { get; set; }
        public DbSet<Parameters_3> Parameters_3 { get; set; }
        public DbSet<OtherWork> OtherWorks { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ActOfWork> ActsOfWorks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OtherWork>().Property(o => o._FactValue).HasColumnName("OtherFactValue");
            modelBuilder.Entity<OtherWork>().Property(o => o._PlanValue).HasColumnName("OtherPlanValue");
            modelBuilder.Entity<Status>().Property(o => o.AssertionPlan).HasColumnName("AssertionPlan_Array");
            modelBuilder.Entity<Status>().Property(o => o.Assertion_1_CalendarQuarter).HasColumnName("Assertion_1_CalendarQuarter_Array");
            modelBuilder.Entity<Status>().Property(o => o.Assertion_2_CalendarQuarter).HasColumnName("Assertion_2_CalendarQuarter_Array");
            modelBuilder.Entity<Status>().Property(o => o.Assertion_3_CalendarQuarter).HasColumnName("Assertion_3_CalendarQuarter_Array");
            modelBuilder.Entity<Status>().Property(o => o.Assertion_4_CalendarQuarter).HasColumnName("Assertion_4_CalendarQuarter_Array");
            modelBuilder.Entity<Status>().Property(o => o.Blocking).HasColumnName("Blocking_Array");
            modelBuilder.Entity<ActOfWork>().Property(o => o.Employee).HasColumnName("EmployeeList");
            modelBuilder.Entity<GroupNameOfWorks>().HasData(new GroupNameOfWorks { GroupID = 1, NameGroup = "Дополнительные работы, не предусмотренные планом" }) ;
            modelBuilder.Entity<Department>().HasData(new Department { ID=1, NameDepartment="Администрация" , DirectorDepartment="Игнатьев Игнат Игнатович"});
        }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
    }
}
