using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Home.Ranker.Data
{
    public class PhotoRepository
    {
        private readonly HomeRankerContext _currentContext;

        public PhotoRepository(HomeRankerContext context)
        {
            _currentContext = context;
        }
        public IEnumerable<Photo> GetAllPhotos()
        {
            return _currentContext.Photos.ToList();
        }

        public IEnumerable<Photo> GetPhotos(Expression<Func<Photo, bool>> predicate)
        {
            return _currentContext.Photos.Where(predicate).ToList();
        }

        public void InsertPhoto(Photo Photo)
        {
            _currentContext.Photos.Add(Photo);
        }

        public void DeletePhoto(string url, int appartmentId)
        {
            var Photo = _currentContext.Photos.Find(url, appartmentId);

            _currentContext.Photos.Remove(Photo);
        }

        public void UpdatePhoto(Photo Photo)
        {
            _currentContext.Entry(Photo).State = EntityState.Modified;
        }
    }

}