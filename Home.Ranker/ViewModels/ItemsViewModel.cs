using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Home.Ranker.Views;
using Home.Ranker.Data;

namespace Home.Ranker.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Apartment> Items { get; set; }
        public Command LoadItemsCommand { get; set; }


        private readonly ApartmentRepository ApartmentRepository;

        public ItemsViewModel()
        {

            ApartmentRepository = new ApartmentRepository(new HomeRankerContext());

            Title = "Browse";
            Items = new ObservableCollection<Apartment>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Apartment>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Apartment;
                Items.Add(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = ApartmentRepository.GetAllApartments();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}