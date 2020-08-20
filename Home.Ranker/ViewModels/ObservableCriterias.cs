using Home.Ranker.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;

namespace Home.Ranker.ViewModels
{
    public class ObservableCriterias : ObservableCollection<CriteriaViewModel>, IDisposable
    {
        private double? ratesAverage;

        public double? RatesAverage
        {
            get => ratesAverage;
            set
            {
                ratesAverage = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RatesAverage)));


            }
        }


        public ObservableCriterias(IEnumerable<CriteriaViewModel> criterias) : base(criterias)
        {
            foreach (var item in criterias)
            {
                item.PropertyChanged += this.Item_PropertyChanged;
            }
            AvgCalculation();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CriteriaViewModel.RateValue))
            {
                AvgCalculation();
            }
        }

        public ObservableCriterias() : base(new List<CriteriaViewModel>())
        {

        }

        protected override void SetItem(int index, CriteriaViewModel item)
        {
            this[index].PropertyChanged -= Item_PropertyChanged;
            item.PropertyChanged+= Item_PropertyChanged;
            base.SetItem(index, item);

            AvgCalculation();

        }

        protected override void InsertItem(int index, CriteriaViewModel item)
        {
            item.PropertyChanged += this.Item_PropertyChanged;
            base.InsertItem(index, item);

            AvgCalculation();

        }

        protected override void RemoveItem(int index)
        {
            this[index].PropertyChanged -= this.Item_PropertyChanged;
            base.RemoveItem(index);

            AvgCalculation();
        }

        private void AvgCalculation()
        {
            RatesAverage = this.Where(n => n.RateValue.HasValue).WeightedAverage(c => c.RateValue.Value, w => w.Criteria.ImportanceLevel);

        }

        public void Dispose()
        {
            foreach (var item in this)
            {
                item.PropertyChanged -= this.Item_PropertyChanged;
            }



            GC.SuppressFinalize(this);
        }

    }

}
