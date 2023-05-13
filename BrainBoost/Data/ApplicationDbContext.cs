using BrainBoost.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainBoost.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Billing> Billing { get; set; }
        public DbSet<BillingCard> BillingCard { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseMaterial> CourseMaterial { get; set; }
        public DbSet<CourseProgress> CourseProgress { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Professor> Professor { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Billing>().ToTable("Billing");
            modelBuilder.Entity<BillingCard>().ToTable("BillingCard");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<CourseMaterial>().ToTable("CourseMaterial");
            modelBuilder.Entity<CourseProgress>().ToTable("CourseProgress");
            modelBuilder.Entity<Email>().ToTable("Email");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Professor>().ToTable("Professor");
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<QuestionAnswer>().ToTable("QuestionAnswer");
            modelBuilder.Entity<Quiz>().ToTable("Quiz");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<User>().ToTable("User");

            base.OnModelCreating(modelBuilder);
        }
    }
}
