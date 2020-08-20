using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Home.Ranker.Controls
{
    public class RatingControl : StackLayout
    {
        public RatingControl()
        {
            Orientation = StackOrientation.Horizontal;
        }

        public static BindableProperty RatingProperty = BindableProperty.Create(nameof(Rating), typeof(double), typeof(RatingControl), 0.0, BindingMode.OneWay, propertyChanged: (bindable, newValue, oldValue) =>
        {
            var control = (RatingControl)bindable;

            if (newValue != oldValue)
            {
                control.Draw();
            }
        });

        private readonly List<Point> originalFullStarPoints = new List<Point>()
        {
            new Point(96,1.12977573),
            new Point(66.9427701,60.0061542),
            new Point(1.96882894,69.4474205),
            new Point(48.9844145,115.27629),
            new Point(37.8855403,179.987692),
            new Point(96,149.435112),
            new Point(154.11446,179.987692),
            new Point(143.015586,115.27629),
            new Point(190.031171,69.4474205),
            new Point(125.05723,60.0061542),
            new Point(96,1.12977573),
        };

        private readonly List<Point> originalHalfStarPoints = new List<Point>()
        {
            new Point(96,1.12977573),
            new Point(66.9427701,60.0061542),
            new Point(1.96882894,69.4474205),
            new Point(48.9844145,115.27629),
            new Point(37.8855403,179.987692),
            new Point(96,149.435112),
            new Point(96,1.12977573)
        };

        private readonly PointCollection fullStarPoints = new PointCollection();
        private readonly PointCollection halfStarPoints = new PointCollection();

        private double ratio;

        private void Draw()
        {
            Children.Clear();

            var newRatio = Size / 200;

            if (newRatio != ratio)
            {
                ratio = newRatio;

                CalculatePoints(fullStarPoints, originalFullStarPoints);
                CalculatePoints(halfStarPoints, originalHalfStarPoints);
            }


            for (var i = 1; i <= Max; i++)
            {
                if (Rating >= i)
                {
                    Children.Add(GetFullStar());
                }
                else if (Rating > i - 1)
                {
                    Children.Add(GetHalfStar());
                }
                else
                {
                    Children.Add(GetEmptyStar());
                }
            }
        }

        private void CalculatePoints(PointCollection calculated, List<Point> original)
        {
            calculated.Clear();

            foreach (var point in original)
            {
                var x = point.X * ratio;
                var y = point.Y * ratio;

                var p = new Point(x, y);

                calculated.Add(p);
            }
        }

        private Polygon GetFullStar()
        {
            var fullStar = new Polygon()
            {
                Points = fullStarPoints,
                Fill = FillColor,
                StrokeThickness = StrokeThickness,
                Stroke = StrokeColor
            };

            return fullStar;
        }

        private Grid GetHalfStar()
        {
            var grid = new Grid();

            var halfStar = new Polygon()
            {
                Points = halfStarPoints,
                Fill = fillColor,
                Stroke = Brush.Transparent,
                StrokeThickness = 0,
            };

            var emptyStar = new Polygon()
            {
                Points = fullStarPoints,
                StrokeThickness = StrokeThickness,
                Stroke = StrokeColor
            };

            grid.Children.Add(halfStar);
            grid.Children.Add(emptyStar);

            return grid;
        }

        private Polygon GetEmptyStar()
        {
            var emptyStar = new Polygon()
            {
                Points = fullStarPoints,
                StrokeThickness = StrokeThickness,
                Stroke = StrokeColor
            };

            return emptyStar;
        }


        public double Rating
        {
            get => (double)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        private int max = 5;
        public int Max
        {
            get => max;
            set => Set(ref max, value);
        }

        private void Set<T>(ref T field, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                Draw();
            }
        }


        private Brush fillColor = Brush.Yellow;
        public Brush FillColor
        {
            get => fillColor;
            set => Set(ref fillColor, value);
        }

        private Brush strokeColor = Brush.Black;
        public Brush StrokeColor
        {
            get => strokeColor;
            set => Set(ref strokeColor, value);
        }

        private double strokeThickness = 0;
        public double StrokeThickness
        {
            get => strokeThickness;
            set => Set(ref strokeThickness, value);
        }

        private double size = 50;
        public double Size
        {
            get => size;
            set => Set(ref size, value);
        }


    }
}

