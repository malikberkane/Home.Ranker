using Home.Ranker.Data;
using Home.Ranker.Fonts;
using Microsoft.EntityFrameworkCore.Internal;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PancakeView;

namespace Home.Ranker.Controls
{
    public class AnimatedCell : ViewCell
    {


        public static readonly BindableProperty ApartmentProperty =
        BindableProperty.Create("Apartment", typeof(Apartment), typeof(ViewCell), null, propertyChanged:OnApartmentChanged);

        private static void OnApartmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is AnimatedCell currentCell && newValue is Apartment newApt)
            {
                if (newApt?.FirstPictureUrl == null)
                {
                    return;
                }
                using (Stream stream = new MemoryStream(File.ReadAllBytes(newApt.FirstPictureUrl)))
                {
                    currentCell.bitmap = SKBitmap.Decode(stream);
                }
            }
        }

        SKBitmap bitmap;



        public Apartment Apartment
        {
            get { return (Apartment)GetValue(ApartmentProperty); }
            set { SetValue(ApartmentProperty, value); }
        }
        private SKCanvasView canvasView;
        public AnimatedCell()
        {
            canvasView = new SKCanvasView() { HeightRequest=100};

            canvasView.PaintSurface += OnCanvasViewPaintSurface;

            //SwipeItem deleteSwipeItem = new SwipeItem
            //{
            //    IconImageSource = new FontImageSource { FontFamily = "MaterialFontFamily", Glyph = IconFont.Delete, Color = Color.Black, Size = 20 },
            //    BackgroundColor = Color.Red
            //};

            //List<SwipeItem> swipeItems = new List<SwipeItem>() {  deleteSwipeItem };

            var contentView = new ContentView() { Padding = new Thickness(10, 25), Content = canvasView };
            //SwipeView swipeView = new SwipeView
            //{
            //    RightItems = new SwipeItems(swipeItems),
            //    Content = contentView
            //};
            View = contentView;
        }
            



        

        private ListView ParentCollectionView;
        private ObservableCollection<Apartment> parentItemsSource;
        private int CellIndex;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
        protected override void OnParentSet()
        {
            base.OnParentSet();

            ParentCollectionView = this.Parent as ListView;


            if (ParentCollectionView != null)
            {
                parentItemsSource = ParentCollectionView.ItemsSource as ObservableCollection<Apartment>;
                if (parentItemsSource != null)
                {
                    CellIndex = parentItemsSource.IndexOf(this.Apartment);

                }

                
                ParentCollectionView.ItemAppearing += this.ParentCollectionView_ItemAppearing;


               
            }


        }

       

        private async void ParentCollectionView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e.ItemIndex == CellIndex)
            {
                angle = 180;
                canvasView.InvalidateSurface();

               

                while (angle>=0)
                {
                    angle=angle-9;

                    canvasView.InvalidateSurface();
                    await Task.Delay(5);
                }

                 
            }
        }

        private float angle=0;
        private bool _isRotating;

        private void Parent_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

            if (parentItemsSource != null)
            {
                

                if (e.VerticalDelta>0 && e.LastVisibleItemIndex<=CellIndex && !_isRotating)
                {
                    _isRotating = true;
                    angle = 180;
                    canvasView.InvalidateSurface();

                    Device.StartTimer(TimeSpan.FromMilliseconds(0.5), () =>
                    {

                        angle--;
                        canvasView.InvalidateSurface();


                        return angle >=0;
                      
                    });

                    _isRotating = false;



                }


            }

          

            canvasView.InvalidateSurface();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            string strName = Apartment.Name;
            string strRating = Apartment.RatesAverage?.ToString();

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Typeface= SKTypeface.FromFamilyName("Brandon_reg"),
                Color = SKColors.Black
            };

            // Adjust TextSize property so text is 90% of screen width
            textPaint.TextSize = 0.05f * info.Width;

            // Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(strName, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xTextName = 0.1f*info.Width;
            float yTextName = info.Height / 2 - textBounds.MidY;

            float yTextRating =  yTextName + 1.2f*textBounds.Height;


            // And draw the text


            float xCenter = info.Width / 2;
            float yCenter = info.Height / 2;

            // Translate center to origin
            SKMatrix matrix = SKMatrix.MakeTranslation(-xCenter, -yCenter);

            // Use 3D matrix for 3D rotations and perspective
            SKMatrix44 matrix44 = SKMatrix44.CreateIdentity();
            matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(1, 0, 0, angle));


            SKMatrix44 perspectiveMatrix = SKMatrix44.CreateIdentity();
            perspectiveMatrix[3, 2] = -1 / (float)2000;
            matrix44.PostConcat(perspectiveMatrix);

            // Concatenate with 2D matrix
            SKMatrix.PostConcat(ref matrix, matrix44.Matrix);

            // Translate back to center
            SKMatrix.PostConcat(ref matrix,
                SKMatrix.MakeTranslation(xCenter, yCenter));

            // Set the matrix and display the bitmap
            canvas.SetMatrix(matrix);

            // Create a new SKRect object for the frame around the text
            SKRect frameRect = SKRect.Create(0.05f*info.Width,0,0.9f*info.Width,info.Height) ;
            
            // Create an SKPaint object to display the frame
            SKPaint framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Blue
            };

            SKPaint rectanglePaint = new SKPaint
            {
               
                Color = SKColors.Beige
            };

            // Draw one frame
            canvas.DrawRoundRect(frameRect, 20, 20, rectanglePaint);

            canvas.DrawText(strName, xTextName, yTextName, textPaint);
            canvas.DrawText($"{strRating}", xTextName, yTextRating, textPaint);


            if (bitmap != null)
            {
                var pictureFrame = SKRect.Create(info.Width*(float)0.7, 0, info.Width * (float)0.25, info.Height);

                float xBitmap = (float)0.8*info.Width;
                float yBitmap = 0;
               
                canvas.DrawBitmap(bitmap, pictureFrame);
            }




        }

    }
}
