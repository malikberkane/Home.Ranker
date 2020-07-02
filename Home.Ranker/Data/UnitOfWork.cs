using System;
using System.Collections.Generic;
using System.Text;

namespace Home.Ranker.Data
{
    public class UnitOfWork: IDisposable
    {
        private readonly HomeRankerContext _currentContext;

        public CriteriaRepository CriteriaRepository { get; }

        public RateRepository RateRepository { get; }

        public ApartmentRepository ApartmentRepository { get; }

        public PhotoRepository PhotoRepository { get; }


        public UnitOfWork(HomeRankerContext context) 
        {
            _currentContext = context;
            CriteriaRepository = new CriteriaRepository(context);
            RateRepository = new RateRepository(context);
            ApartmentRepository = new ApartmentRepository(context);
            PhotoRepository = new PhotoRepository(context);
        }

        public int Complete()
        {
            return _currentContext.SaveChanges();
        }

        public void Dispose()
        {
            _currentContext.Dispose();
        }
        

        
    }
}
