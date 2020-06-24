using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Home.Ranker.Data
{
    public class RateRepository
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
    }

}