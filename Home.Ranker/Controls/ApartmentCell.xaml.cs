using Xamarin.Forms.Xaml;

namespace Home.Ranker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApartmentCell
    {
        private bool _subscribedToScroll;

        public ApartmentCell()
        {
            InitializeComponent();
        }

        //public int CellIndex { get; private set; }

        //protected override void OnParentSet()
        //{
        //    base.OnParentSet();



        //    if (this.Parent is CollectionView listView && !_subscribedToScroll)
        //    {

             

        //        listView.Scrolled += this.ListView_Scrolled;
        //        _subscribedToScroll = true;

        //    }

          
        //}

     

        //private async void ListView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        //{


        //    if (e.VerticalDelta > 0)
        //    {
        //        await this.RotateXTo(45, 100, Easing.Linear);

        //    }
        //    else
        //    {
        //        if (Math.Abs(e.VerticalDelta) <1)
        //        {
                   
        //            await this.RotateXTo(0, 100, Easing.Linear);

        //        }
        //        else
        //        {
        //            await this.RotateXTo(-45, 100, Easing.Linear);

        //        }
        //    }
        //}

        //private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    if (e.ItemIndex == CellIndex)
        //    {

        //        await this.View.RotateXTo(360, 1000, Easing.Linear);

        //        this.View.RotationX = 0;
        //    }
        //}
    
    }
}