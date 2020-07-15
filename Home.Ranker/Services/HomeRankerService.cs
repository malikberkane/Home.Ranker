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


        public IEnumerable<Apartment> GetAllApartments()
        {
            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {
                var appartments = unitOfWork.ApartmentRepository.GetAllApartments();

                foreach (var appart in appartments)
                {
                    appart.FirstPictureImageSource = ImageSource.FromStream(() =>
                    {
                        var bytes = File.ReadAllBytes(appart.FirstPictureUrl);
                        return new MemoryStream(bytes);
                    });

                    var allRates = unitOfWork.RateRepository.GetAllRates(n => n.ApartmentId == appart.Id);
                    if (allRates != null && allRates.Any())
                    {
                        appart.RatesAverage = allRates.WeightedAverage(n => n.RateValue, w =>
                          {
                              var criteria = unitOfWork.CriteriaRepository.GetCriteriaById(w.CriteriaId);
                              return criteria?.ImportanceLevel ?? default;
                          });


                    }
                }

                return appartments;
            }
        }

        public IEnumerable<Photo> GetPhotos(Apartment apartment)
        {
            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {
                var photos = unitOfWork.PhotoRepository.GetPhotos(p => p.ApartmentId == apartment.Id);

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
        }



        public IEnumerable<CriteriaViewModel> GetCriteriasAndRates(Apartment appartment)
        {
            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {
                var result = unitOfWork.CriteriaRepository.GetAllCriterias()?
                    .Select(n => new CriteriaViewModel { Criteria = n })
                    .OrderByDescending(n => n.Criteria.ImportanceLevel)
                    .ToList();

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        var rate = unitOfWork.RateRepository.GetRateById(appartment.Id, item.Criteria.Id);

                        item.RateValue = rate?.RateValue;
                    }

                }


                return result;

            }


        }



        public void InsertCriteria(Criteria criteria)
        {
            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {
                if (criteria.Id == 0)
                {
                    unitOfWork.CriteriaRepository.InsertCriteria(criteria);

                }
                else
                {
                    unitOfWork.CriteriaRepository.UpdateCriteria(criteria);

                }

                unitOfWork.Complete();
            }


        }


        public void DeleteCriteria(Criteria criteria)
        {
            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {

                unitOfWork.RateRepository.DeleteRates(r => r.CriteriaId == criteria.Id);
                unitOfWork.CriteriaRepository.DeleteCriteria(criteria.Id);
                unitOfWork.Complete();
            }


        }

        public void InsertApartment(Apartment apartment, IEnumerable<Photo> photos, IEnumerable<CriteriaViewModel> criteriaRates)
        {

            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {
                if (photos.Any())
                {
                    apartment.FirstPictureUrl = photos.First().PhotoUrl;
                    apartment.FirstPictureImageSource = photos.First().Source;

                }

                if (apartment.Id == 0)
                {
                    unitOfWork.ApartmentRepository.InsertAppartment(apartment);


                }
                else
                {
                    unitOfWork.ApartmentRepository.UpdateApartment(apartment);
                }

                unitOfWork.Complete();

                foreach (var photo in photos)
                {
                    photo.ApartmentId = apartment.Id;

                    if (photo.PhotoId == 0)
                    {
                        unitOfWork.PhotoRepository.InsertPhoto(photo);

                    }


                }

                unitOfWork.Complete();

                foreach (var criteria in criteriaRates)
                {
                    if (criteria.RateValue.HasValue)
                    {
                        var rate = new Rate
                        {
                            CriteriaId = criteria.Criteria.Id,
                            RateValue = criteria.RateValue.Value,
                            ApartmentId = apartment.Id
                        };
                        if (unitOfWork.RateRepository.GetRateById(rate.ApartmentId, rate.CriteriaId) == null)
                        {
                            unitOfWork.RateRepository.InsertRate(rate);

                        }
                        else
                        {
                            unitOfWork.RateRepository.UpdateRate(rate);

                        }





                    }

                    unitOfWork.Complete();



                }

                apartment.RatesAverage = criteriaRates.Where(n => n.RateValue.HasValue).WeightedAverage(c => c.RateValue.Value, w => w.Criteria.ImportanceLevel);

            }

        }

        public void DeleteApartment(Apartment apartment)
        {
            using (var unitOfWork = new UnitOfWork(new HomeRankerContext()))
            {

                unitOfWork.RateRepository.DeleteRates(r => r.ApartmentId == apartment.Id);
                unitOfWork.PhotoRepository.DeletePhotos(r => r.ApartmentId == apartment.Id);

                unitOfWork.ApartmentRepository.DeleteApartment(apartment.Id);

                unitOfWork.Complete();
            }


        }


    }
}
