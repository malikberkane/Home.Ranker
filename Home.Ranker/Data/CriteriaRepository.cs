using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Home.Ranker.Data
{
    public class CriteriaRepository: IDisposable
    {

        private readonly HomeRankerContext _currentContext;

        public CriteriaRepository(HomeRankerContext context)
        {
            _currentContext = context;
        }
        public IEnumerable<Criteria> GetAllCriterias()
        {
            return _currentContext.Criterias.ToList();

        }

     
        public Criteria GetCriteriaById(int id)
        {
            return _currentContext.Criterias.Find(id);
        }

        public void InsertCriteria(Criteria criteria)
        {
            _currentContext.Criterias.Add(criteria);
        }

        public void DeleteCriteria(int criteriaId)
        {
            Criteria criteria = _currentContext.Criterias.Find(criteriaId);
            _currentContext.Criterias.Remove(criteria);
        }

        public void UpdateCriteria(Criteria criteria)
        {
            _currentContext.Entry(criteria).State = EntityState.Modified;
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