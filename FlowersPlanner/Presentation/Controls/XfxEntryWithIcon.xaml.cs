using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FlowersPlanner.Presentation.Controls
{
    public partial class XfxEntryWithIcon : Grid
    {
        public XfxEntryWithIcon()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(XfxEntryWithIcon), (object)null);

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource IconSource
        {
            get => (ImageSource)this.GetValue(IconSourceProperty);
            set => this.SetValue(IconSourceProperty, (object)value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(XfxEntryWithIcon), default(string));

        public string FontFamily
        {
            get => (string)this.GetValue(FontFamilyProperty);
            set => this.SetValue(FontFamilyProperty, (object)value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(XfxEntryWithIcon), default(string), BindingMode.TwoWay, (BindableProperty.ValidateValueDelegate)null, new BindableProperty.BindingPropertyChangedDelegate(XfxEntryWithIcon.OnTextChanged));

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, (object)value);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(XfxEntryWithIcon), default(string));

        public string Placeholder
        {
            get => (string)this.GetValue(PlaceholderProperty);
            set => this.SetValue(PlaceholderProperty, (object)value);
        }

        public static readonly BindableProperty ActivePlaceholderColorProperty = BindableProperty.Create(nameof(ActivePlaceholderColor), typeof(Color), typeof(XfxEntryWithIcon), Color.Accent);

        public Color ActivePlaceholderColor
        {
            get => (Color)this.GetValue(ActivePlaceholderColorProperty);
            set => this.SetValue(ActivePlaceholderColorProperty, (object)value);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(XfxEntryWithIcon), Color.Accent);

        public Color PlaceholderColor
        {
            get => (Color)this.GetValue(PlaceholderColorProperty);
            set => this.SetValue(PlaceholderColorProperty, (object)value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(XfxEntryWithIcon), default(double));

        public double FontSize
        {
            get => (double)this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, (object)value);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            XfxEntryWithIcon entry = (XfxEntryWithIcon)bindable;
            // ISSUE: reference to a compiler-generated field
            EventHandler<TextChangedEventArgs> textChanged = entry.TextChanged;
            if (textChanged == null)
                return;
            textChanged((object)entry, new TextChangedEventArgs((string)oldValue, (string)newValue));
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == IconSourceProperty.PropertyName)
            {
                IconView.Source = IconSource;
            }
            else if (propertyName == PlaceholderColorProperty.PropertyName)
            {
                Entry.PlaceholderColor = PlaceholderColor;
            }
            else if (propertyName == ActivePlaceholderColorProperty.PropertyName)
            {
                Entry.ActivePlaceholderColor = ActivePlaceholderColor;
            }
            else if (propertyName == PlaceholderProperty.PropertyName)
            {
                Entry.Placeholder = Placeholder;
            }
            else if (propertyName == FontSizeProperty.PropertyName)
            {
                Entry.FontSize = FontSize;
            }
            else if (propertyName == FontFamilyProperty.PropertyName)
            {
                Entry.FontFamily = FontFamily;
            }
            else if (propertyName == TextProperty.PropertyName)
            {
                Entry.Text = Text;
            }
        }
    }
}
