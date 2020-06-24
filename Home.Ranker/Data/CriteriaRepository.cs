using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Home.Ranker.Data
{
    public class CriteriaRepository
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
            Criteria student = _currentContext.Criterias.Find(criteriaId);
            _currentContext.Criterias.Remove(student);
        }

        public void UpdateCriteria(Criteria criteria)
        {
            _currentContext.Entry(criteria).State = EntityState.Modified;
        }
    }

}