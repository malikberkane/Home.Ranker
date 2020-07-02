using System;
using Xamarin.Forms;

namespace Home.Ranker.Data
{
    public class Apartment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string   Adresse { get; set; }


        public string FirstPictureUrl { get; set; }


        public double? RatesAverage { get; set; }

        public ImageSource FirstPictureImageSource { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Apartment other)
            {
                return other.Id == Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Name, this.Adresse, this.FirstPictureUrl, this.RatesAverage, this.FirstPictureImageSource);
        }
    }
}