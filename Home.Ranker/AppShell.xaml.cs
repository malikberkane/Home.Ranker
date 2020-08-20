using Home.Ranker.Views;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Home.Ranker
{
    public partial class AppShell : SharedTransitionShell
    {
        public AppShell()
        {
            InitializeComponent();
        }


        protected override bool OnBackButtonPressed()
        {
            var page = (Current?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;

            switch (page)
            {
                case SetApartmentPage setAptPage:
                    if (Current.Navigation.NavigationStack.Count > 1)
                    {
                        setAptPage.NavBackImplementation();
                    }
                    break;
                case SetCriteriaPage setCriteriaPage:
                    setCriteriaPage.NavBackImplementation();
                    break;
                default:
                    return base.OnBackButtonPressed();
            }


            

            return true;

        }
    }
}
