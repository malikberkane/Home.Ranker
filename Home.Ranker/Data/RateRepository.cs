using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Home.Ranker.Data
{
    public class RateRepository: IDisposable
    {
        private readonly HomeRankerContext _currentContext;

        public RateRepository(HomeRankerContext context)
        {
            _currentContext = context;
        }
        public IEnumerable<Rate> GetAllRates()
        {
            return _currentContext.Rates.ToList();
        }

        public Rate GetRateById(int apartmentId, int criteriaId)
        {
            return _currentContext.Rates.Find(apartmentId, criteriaId);
        }

        public void InsertRate(Rate Rate)
        {
            _currentContext.Rates.Add(Rate);
        }

        public void DeleteRate(int apartmentId, int criteriaId)
        {
            var rate = _currentContext.Rates.Find(apartmentId, criteriaId);

            _currentContext.Rates.Remove(rate);
        }

        public void UpdateRate(Rate Rate)
        {
            _currentContext.Entry(Rate).State = EntityState.Modified;
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