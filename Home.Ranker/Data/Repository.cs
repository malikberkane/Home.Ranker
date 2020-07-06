using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Home.Ranker.Data
{
    public class ApartmentRepository: IDisposable
    {

        private readonly HomeRankerContext _currentContext;

        public ApartmentRepository(HomeRankerContext context)
        {
            _currentContext = context;
        }
        public IEnumerable<Apartment> GetAllApartments()
        {
            return _currentContext.Apartments.ToList();

        }



        public Apartment GetAppartmentById(int id)
        {
            return _currentContext.Apartments.Find(id);
        }

        public void InsertAppartment(Apartment appartment)
        {
            _currentContext.Apartments.Add(appartment);
        }

        public void DeleteApartment(int apartmentId)
        {
            Apartment student = _currentContext.Apartments.Find(apartmentId);
            _currentContext.Apartments.Remove(student);
        }

        public void UpdateApartment(Apartment apartment)
        {
            _currentContext.Entry(apartment).State = EntityState.Modified;

        }


        public void Save()
        {
            _currentContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _currentContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}



