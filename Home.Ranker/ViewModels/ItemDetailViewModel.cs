using System;
using Home.Ranker.Data;
using Home.Ranker.Models;

namespace Home.Ranker.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Apartment Apartment { get; set; }
        public ItemDetailViewModel(Apartment item = null)
        {
            Title = item?.Name;
            Apartment = item;
        }
    }
}
