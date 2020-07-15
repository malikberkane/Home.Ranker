using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Home.Ranker.Data
{
    public class PhotoRepository: IDisposable
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

        public void DeletePhotos(Func<Photo, bool> predicate)
        {
            var photosToDelete = _currentContext.Photos.Where(predicate);

            _currentContext.Photos.RemoveRange(photosToDelete);
        }

        public void UpdatePhoto(Photo Photo)
        {
            _currentContext.Entry(Photo).State = EntityState.Modified;
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