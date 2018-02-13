using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace FlowersPlanner.Presentation.Controls
{
    public partial class CustomSelector : Grid
    {
        public CustomSelector()
        {
            InitializeComponent();
            ColumnSpacing = 0;
            RowSpacing = 4;
        }

        public static readonly BindableProperty NormalColorProperty = BindableProperty.Create(nameof(NormalColor), typeof(Color), typeof(CustomSelector), Color.Gray);

        public Color NormalColor
        {
            get => (Color)this.GetValue(NormalColorProperty);
            set => this.SetValue(NormalColorProperty, (object)value);
        }

        public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(CustomSelector), Color.Black);

        public Color SelectedColor
        {
            get => (Color)this.GetValue(SelectedColorProperty);
            set => this.SetValue(SelectedColorProperty, (object)value);
        }

        public static readonly BindableProperty SelectedFontFamilyProperty = BindableProperty.Create(nameof(SelectedFontFamily), typeof(string), typeof(CustomSelector), default(string));

        public string SelectedFontFamily
        {
            get => (string)this.GetValue(SelectedFontFamilyProperty);
            set => this.SetValue(SelectedFontFamilyProperty, (object)value);
        }

        public static readonly BindableProperty NormalFontFamilyProperty = BindableProperty.Create(nameof(NormalFontFamily), typeof(string), typeof(CustomSelector), default(string));

        public string NormalFontFamily
        {
            get => (string)this.GetValue(NormalFontFamilyProperty);
            set => this.SetValue(NormalFontFamilyProperty, (object)value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(CustomSelector), 14.0);

        public double FontSize
        {
            get => (double)this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, (object)value);
        }

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), typeof(int), typeof(CustomSelector), default(int));

        public int Position
        {
            get => (int)this.GetValue(PositionProperty);
            set => this.SetValue(PositionProperty, (object)value);
        }

        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(CustomSelector), default(string));

        public string LeftText
        {
            get => (string)this.GetValue(LeftTextProperty);
            set => this.SetValue(LeftTextProperty, (object)value);
        }

        public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(CustomSelector), default(string));

        public string RightText
        {
            get => (string)this.GetValue(RightTextProperty);
            set => this.SetValue(RightTextProperty, (object)value);
        }

        public event EventHandler<SelectedPositionChangedEventArgs> Selected;

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == SelectedFontFamilyProperty.PropertyName || 
                propertyName == NormalFontFamilyProperty.PropertyName ||
                propertyName == SelectedColorProperty.PropertyName ||
                propertyName == NormalColorProperty.PropertyName)
            {
                UpdateUi();
            }
            else if (propertyName == LeftTextProperty.PropertyName)
            {
                LeftLabel.Text = LeftText;
            }
            else if (propertyName == RightTextProperty.PropertyName)
            {
                RightLabel.Text = RightText;
            }
            else if (propertyName == FontSizeProperty.PropertyName)
            {
                LeftLabel.FontSize = FontSize;
                RightLabel.FontSize = FontSize;
            }

        }

        private void UpdateUi()
        {
            LeftLabel.FontFamily = Position > 0 ? NormalFontFamily : SelectedFontFamily;
            LeftLabel.TextColor = Position > 0 ? NormalColor : SelectedColor;
            RightLabel.FontFamily = Position > 0 ? SelectedFontFamily : NormalFontFamily;
            RightLabel.TextColor = Position > 0 ? SelectedColor : NormalColor;
            LeftBox.BackgroundColor = Position > 0 ? NormalColor : SelectedColor;
            RightBox.BackgroundColor = Position > 0 ? SelectedColor : NormalColor;
            LeftBox.HeightRequest = Position > 0 ? 1 : 2;
            RightBox.HeightRequest = Position > 0 ? 2 : 1;
        }

        private void OnLeftLabelTapped(object sender, EventArgs args)
        {
            Position = 0;
            Selected?.Invoke(this, new SelectedPositionChangedEventArgs(Position));
            UpdateUi();
        }

        private void OnRightLabelTapped(object sender, EventArgs args)
        {
            Position = 1;
            Selected?.Invoke(this, new SelectedPositionChangedEventArgs(Position));
            UpdateUi();
        }
    }

    public class CustomSelectorEvents : VisualElementEvents
    {
        private CustomSelector This;

        public CustomSelectorEvents(CustomSelector This) : base((VisualElement)This)
        {
            this.This = This;
        }

        public IObservable<SelectedPositionChangedEventArgs> Selected
        {
            get => Observable.FromEventPattern<EventHandler<SelectedPositionChangedEventArgs>, SelectedPositionChangedEventArgs>((Action<EventHandler<SelectedPositionChangedEventArgs>>)(x => this.This.Selected += x), (Action<EventHandler<SelectedPositionChangedEventArgs>>)(x => this.This.Selected -= x)).Select<EventPattern<SelectedPositionChangedEventArgs>, SelectedPositionChangedEventArgs>((Func<EventPattern<SelectedPositionChangedEventArgs>, SelectedPositionChangedEventArgs>)(x => x.EventArgs));
        }
    }
}
