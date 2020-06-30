using Home.Ranker.Data;
using Home.Ranker.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Home.Ranker.Services
{
    public class HomeRankerService
    {
        private readonly ApartmentRepository ApartmentRepository;

        private readonly PhotoRepository PhotoRepository;

        private readonly CriteriaRepository CriteriaRepository;

        private readonly RateRepository RateRepository;



        public HomeRankerService()
        {
            ApartmentRepository = new ApartmentRepository(new HomeRankerContext());
            PhotoRepository = new PhotoRepository(new HomeRankerContext());

            CriteriaRepository = new CriteriaRepository(new HomeRankerContext());

            RateRepository = new RateRepository(new HomeRankerContext());



        }
        public IEnumerable<Apartment> GetAllApartments()
        {
            var appartments = ApartmentRepository.GetAllApartments();

            foreach (var appart in appartments)
            {
                appart.FirstPictureImageSource = ImageSource.FromStream(() =>
                {
                    var bytes = File.ReadAllBytes(appart.FirstPictureUrl);
                    return new MemoryStream(bytes);
                });
            }

            return appartments;
        }

        public IEnumerable<Photo> GetPhotos(Apartment apartment)
        {
            var photos = PhotoRepository.GetPhotos(p => p.ApartmentId == apartment.Id);

            foreach (var photo in photos)
            {
                photo.Source = ImageSource.FromStream(() =>
                {
                    var bytes = File.ReadAllBytes(photo.PhotoUrl);
                    return new MemoryStream(bytes);
                });


            }

            return photos;
        }

        public IEnumerable<CriteriaViewModel> GetCriterias()
        {
            return CriteriaRepository.GetAllCriterias()?.Select(n => new CriteriaViewModel { Criteria = n });
        }

        public IEnumerable<CriteriaViewModel> GetCriteriasAndRates(Apartment appartment)
        {
            var result = CriteriaRepository.GetAllCriterias()?.Select(n => new CriteriaViewModel { Criteria = n });

            result?.ForEach(n =>
            {
                var rate = RateRepository.GetRateById(appartment.Id, n.Criteria.Id);

                n.RateValue = rate?.RateValue;
            });

            return result;


        }

        public void InsertRate(Rate rate)
        {

            var existingRate = RateRepository.GetRateById(rate.ApartmentId, rate.CriteriaId);
            if (existingRate != null)
            {
                RateRepository.UpdateRate(rate);

            }
            else
            {
                RateRepository.InsertRate(rate);

            }




            RateRepository.Save();

        }


        public void InsertCriteria(Criteria criteria)
        {
            if (criteria.Id == 0)
            {
                CriteriaRepository.InsertCriteria(criteria);

            }
            else
            {
                CriteriaRepository.UpdateCriteria(criteria);

            }

            CriteriaRepository.Save();

        }


        public void InsertApartment(Apartment apartment, IEnumerable<Photo> photos)
        {

            if (photos.Any())
            {
                apartment.FirstPictureUrl = photos.First().PhotoUrl;

            }

            if (apartment.Id == 0)
            {
                ApartmentRepository.InsertAppartment(apartment);

            }
            else
            {
                ApartmentRepository.UpdateApartment(apartment);
            }
            ApartmentRepository.Save();

            foreach (var photo in photos)
            {
                photo.ApartmentId = apartment.Id;

                if (photo.PhotoId == 0)
                {
                    PhotoRepository.InsertPhoto(photo);

                }

            }

            PhotoRepository.Save();
        }
    }
}
