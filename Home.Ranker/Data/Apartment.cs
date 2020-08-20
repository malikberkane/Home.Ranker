using Home.Ranker.ViewModels;
using System;
using Xamarin.Forms;

namespace Home.Ranker.Data
{
    public class Apartment : BaseViewModel, IComparable<Apartment>
    {
        private double? ratesAverage;
        private string name;
        private string adresse;
        private ImageSource firstPictureImageSource;
        private string firstPictureUrl;

        public int Id { get; set; }

        public string Name
        {
            get => name; set
            {
                SetProperty(ref name, value);
            }
        }

        public string Adresse
        {
            get => adresse; set
            {
                SetProperty(ref adresse, value);
            }
        }


        public string FirstPictureUrl
        {
            get => firstPictureUrl; set
            {
                SetProperty(ref firstPictureUrl, value);
            }
        }


        public double? RatesAverage
        {
            get => ratesAverage; set
            {
                SetProperty(ref ratesAverage, value);
            }
        }

        public ImageSource FirstPictureImageSource
        {
            get => firstPictureImageSource; set
            {
                SetProperty(ref firstPictureImageSource, value);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Apartment other)
            {
                return other.Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Name, this.Adresse, this.FirstPictureUrl, this.RatesAverage, this.FirstPictureImageSource);
        }

        public int CompareTo(Apartment other)
        {
            if(!RatesAverage.HasValue && other.RatesAverage.HasValue)
            {
                return 1;
            }
            else if(RatesAverage.HasValue && !other.RatesAverage.HasValue)
            {
                return -1;
            }

            if (RatesAverage < other.RatesAverage)
            {
                return 1;
            }
            else if (RatesAverage > other.RatesAverage)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public void UpdateInstance(Apartment apt, bool resetFirstImageUrl)
        {
            Id = apt.Id;
            Name = apt.Name;
            Adresse = apt.Adresse;
            RatesAverage = apt.RatesAverage;

            if (resetFirstImageUrl)
            {
                FirstPictureImageSource = apt.FirstPictureImageSource;
                FirstPictureUrl = apt.FirstPictureUrl;
            }

        }

    }
}