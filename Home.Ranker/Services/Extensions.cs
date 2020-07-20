using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Home.Ranker.Services
{
    public static class Extensions
    {
        public static double? WeightedAverage<T>(this IEnumerable<T> records, Func<T, double> value, Func<T, double> weight)
        {
            double weightedValueSum = records.Sum(x => value(x) * weight(x));
            double weightSum = records.Sum(x => weight(x));

            if (weightSum != 0)
                return weightedValueSum / weightSum;
            else
                return null;
        }

        public static void Sort<T>(this ObservableCollection<T> observable) where T : IComparable<T>
        {
            List<T> sorted = observable.OrderBy(x => x).ToList();

            int ptr = 0;
            while (ptr < sorted.Count)
            {
                if (!observable[ptr].Equals(sorted[ptr]))
                {
                    T t = observable[ptr];
                    observable.RemoveAt(ptr);
                    observable.Insert(sorted.IndexOf(t), t);
                }
                else
                {
                    ptr++;
                }
            }
        }

       
    }
}
