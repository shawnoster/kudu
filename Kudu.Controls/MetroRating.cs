using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Kudu.Controls
{
    [TemplatePart(Name = ImagePart, Type = typeof(Image))]
    public class MetroRating : ContentControl
    {
        private const string ImagePart = "Image";

        private const double MaxRating = 5;
        private const double MinRating = 0;

        private static BitmapImage[] _stars = new BitmapImage[11];
        private static bool _starsInitialized;

        #region public double Rating
        /// <summary>
        /// Rating property.
        /// </summary>
        public double Rating
        {
            get { return (double)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        /// <summary>
        /// Identifies the Rating dependency property.
        /// </summary>
        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
                "Rating",
                typeof(double),
                typeof(MetroRating),
                new PropertyMetadata(0.0, OnRatingPropertyChanged));
        

        /// <summary>
        /// RatingProperty property changed handler.
        /// </summary>
        /// <param name="d">MetroRating that changed its Rating.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnRatingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MetroRating source = d as MetroRating;
            double value = (double)e.NewValue;

            UpdateRatingImage(source, value);
        }

        private static void UpdateRatingImage(MetroRating source, double value)
        {
            // make sure the static cache of images is initialized
            InitializeStars();

            // constrain rating between min and max rating values (0-5)
            value = Round(value, 1);
            value = Math.Min(value, MaxRating);
            value = Math.Max(value, MinRating);

            // swap out the image
            if (source._image != null)
            {
                int starIndex = (int)(value * 2);
                source._image.Source = _stars[starIndex];
            }
        }
        #endregion public double Rating

        private static void InitializeStars()
        {
            if (!_starsInitialized)
            {
                if (IsDarkTheme)
                {
                    _stars[0] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.0.png", UriKind.Relative));
                    _stars[1] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.0half.png", UriKind.Relative));
                    _stars[2] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.1.png", UriKind.Relative));
                    _stars[3] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.1half.png", UriKind.Relative));
                    _stars[4] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.2.png", UriKind.Relative));
                    _stars[5] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.2half.png", UriKind.Relative));
                    _stars[6] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.3.png", UriKind.Relative));
                    _stars[7] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.3half.png", UriKind.Relative));
                    _stars[8] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.4.png", UriKind.Relative));
                    _stars[9] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.4half.png", UriKind.Relative));
                    _stars[10] = new BitmapImage(new Uri("/Assets/stars/dark/search.stars.5.png", UriKind.Relative));
                }
                else
                {
                    _stars[0] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.0.png", UriKind.Relative));
                    _stars[1] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.0half.png", UriKind.Relative));
                    _stars[2] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.1.png", UriKind.Relative));
                    _stars[3] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.1half.png", UriKind.Relative));
                    _stars[4] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.2.png", UriKind.Relative));
                    _stars[5] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.2half.png", UriKind.Relative));
                    _stars[6] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.3.png", UriKind.Relative));
                    _stars[7] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.3half.png", UriKind.Relative));
                    _stars[8] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.4.png", UriKind.Relative));
                    _stars[9] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.4half.png", UriKind.Relative));
                    _stars[10] = new BitmapImage(new Uri("/Assets/stars/light/search.stars.5.png", UriKind.Relative));
                }
                _starsInitialized = true;
            }
        }

        private static bool IsDarkTheme
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]
                    == Visibility.Visible;
            }
        }

        private static double Round(double input, int decimalPlaces)
        {
            double precision = 2.0 * Math.Pow(10, decimalPlaces - 1);
            return Math.Ceiling(input * precision) / precision;
        }

        /// <summary>
        /// The
        /// <see cref="System.Windows.Controls.Image"/>
        /// template part.
        /// </summary>
        private Image _image;
        
        public MetroRating()
        {
            DefaultStyleKey = typeof(MetroRating);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = GetTemplateChild(ImagePart) as Image;
            if (_image != null)
            {
                UpdateRatingImage(this, Rating);
            }
        }
    }
}