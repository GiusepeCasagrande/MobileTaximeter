using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Taxi.Controls
{
    public class ExtendedMap : Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Taxi.ExtendedMap"/> class.
        /// </summary>
        public ExtendedMap() : base()
        {
            Radius = Distance.FromKilometers(5);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Taxi.ExtendedMap"/> class.
        /// </summary>
        /// <param name="region">Region.</param>
        public ExtendedMap(MapSpan region) : base(region)
        {
            Radius = Distance.FromKilometers(5);
        }

        /// <summary>
        /// The center point property.
        /// </summary>
        public static readonly BindableProperty CenterPointProperty = BindableProperty.Create(nameof(CenterPoint), typeof(Position), typeof(ExtendedMap), default(Position));

        /// <summary>
        /// Gets or sets the center point.
        /// </summary>
        /// <value>The center point.</value>
        public Position CenterPoint
        {
            get
            {
                return (Position)GetValue(CenterPointProperty);
            }
            set
            {
                SetValue(CenterPointProperty, value);
            }
        }

        /// <summary>
        /// The radius property.
        /// </summary>
        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(Distance), typeof(ExtendedMap), default(Distance));

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>The radius.</value>
        public Distance Radius
        {
            get
            {
                return (Distance)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <returns>The property changed.</returns>
        /// <param name="propertyName">Property name.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (nameof(CenterPoint) == propertyName)
            {
                this.MoveToRegion(MapSpan.FromCenterAndRadius(CenterPoint, Radius));
            }
        }
    }
}