using System;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace Home.Ranker.Data
{
    public class HomeRankerContext : DbContext
    {

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<Photo> Photos { get; set; }


        #region Private implementation

        protected string DatabasePath { get; set; }

        public HomeRankerContext()
        {
            if (Device.RuntimePlatform != Device.UWP)
            {
                DatabasePath = DependencyService.Get<IFileHelper>().GetConnection();
            }

            EnsureAndMigrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasKey(c => new { c.CriteriaId, c.ApartmentId });

            
            modelBuilder.Entity<Apartment>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Apartment>()
             .Ignore(c => c.FirstPictureImageSource);
            modelBuilder.Entity<Apartment>()
           .Ignore(c => c.RatesAverage);
            modelBuilder.Entity<Criteria>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Photo>()
                .HasKey(c => new { c.PhotoId });
            modelBuilder.Entity<Photo>()
             .Ignore(c=>c.Source);





        }







        public void EnsureAndMigrate()
        {
            try
            {
                this.Database.EnsureCreated();
                this.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                optionsBuilder.UseSqlite($"Filename=test.db");
                
            }
            else
            {
                optionsBuilder.UseSqlite($"Filename={DatabasePath}");

            }

        }

        #endregion
    }
}