using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;
using e77.MeasureBase.Helpers;

namespace e77.MeasureBase.GUI.CustomControls
{
    /// <summary>
    /// Interaction logic for LevelMeter.xaml
    /// </summary>

    public static class ColorHelper
    {
        public static Color Interpolate(this Color cFrom, Color cTo, double weight)
        {
            Color ret = new Color();
            ret.ScA = (float)(cFrom.ScA * (1 - weight) + cTo.ScA * weight);
            ret.ScB = (float)(cFrom.ScB * (1 - weight) + cTo.ScB * weight);
            ret.ScG = (float)(cFrom.ScG * (1 - weight) + cTo.ScG * weight);
            ret.ScR = (float)(cFrom.ScR * (1 - weight) + cTo.ScR * weight);
            return ret;
        }
    }

    public partial class LevelMeter : UserControl
    {
        #region Properties

        #region Value

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(LevelMeter),
            //new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnValueChanged)));
          new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelMeter lm = (LevelMeter)d;
            d.CoerceValue(LevelMeter.BaseColorProperty);
        }

        #endregion Value

        #region BaseColor

        public Color BaseColor
        {
            get { return (Color)GetValue(BaseColorProperty); }
            set { SetValue(BaseColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaseColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseColorProperty =
            //DependencyProperty.Register("BaseColor", typeof(Color), typeof(LevelMeter), new UIPropertyMetadata(Colors.Green, new PropertyChangedCallback(OnBaseColorChanged), new CoerceValueCallback(CoerceBaseColor)));
            DependencyProperty.Register("BaseColor", typeof(Color), typeof(LevelMeter), new UIPropertyMetadata(Colors.Green));

        //private static void OnBaseColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //  return;
        //}

        //private static object CoerceBaseColor(DependencyObject d, object value)
        //{
        //  LevelMeter lm = (LevelMeter)d;
        //  float? lowerKey = lm.BoundingColors.Keys.LastOrDefault(k => k < lm.Value);
        //  float? exactKey = lm.BoundingColors.Keys.FirstOrDefault(k => k == lm.Value);
        //  float? upperKey = lm.BoundingColors.Keys.FirstOrDefault(k => k > lm.Value);
        //  if (exactKey != null) return lm.BoundingColors[(float)exactKey];
        //  if (lowerKey == null && upperKey == null) return value;
        //  if (lowerKey == null) return lm.BoundingColors[(float)upperKey];
        //  if (upperKey == null) return lm.BoundingColors[(float)lowerKey];
        //  float weight = (float)((lm.Value - lowerKey) / (upperKey - lowerKey));
        //  return lm.BoundingColors[(float)lowerKey].Interpolate(lm.BoundingColors[(float)upperKey], weight);
        //}
        #endregion BaseColor

        #region SecondaryValue

        public double SecondaryValue
        {
            get { return (double)GetValue(SecondaryValueProperty); }
            set { SetValue(SecondaryValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryValueProperty = DependencyProperty.Register("SecondaryValue", typeof(double), typeof(LevelMeter),
          new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSecondaryValueChanged)));

        private static void OnSecondaryValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LevelMeter lm = (LevelMeter)d;
            d.CoerceValue(LevelMeter.BaseColorProperty);
        }

        #endregion SecondaryValue

        #region SecondaryBaseColor

        public Color SecondaryBaseColor
        {
            get { return (Color)GetValue(SecondaryBaseColorProperty); }
            set { SetValue(SecondaryBaseColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaseColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryBaseColorProperty =
            DependencyProperty.Register("SecondaryBaseColor", typeof(Color), typeof(LevelMeter), new UIPropertyMetadata(Colors.DarkGray));

        #endregion SecondaryBaseColor

        #region Orientation

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LevelMeter), new UIPropertyMetadata(Orientation.Horizontal));
        #endregion Orientation

        #region TextPercentageVisibility

        public Visibility TextPercentageVisibility
        {
            get { return (Visibility)GetValue(TextPercentageVisibilityProperty); }
            set { SetValue(TextPercentageVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextPercentageVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextPercentageVisibilityProperty =
            DependencyProperty.Register("TextPercentageVisibility", typeof(Visibility), typeof(LevelMeter), new UIPropertyMetadata(Visibility.Visible));
        #endregion TextPercentageVisibility

        #region BoundingColors

        public GradientStopCollection BoundingColors
        {
            get { return (GradientStopCollection)GetValue(BoundingColorsProperty); }
            set { SetValue(BoundingColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoundingColors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoundingColorsProperty =
            DependencyProperty.Register("BoundingColors", typeof(GradientStopCollection), typeof(LevelMeter), new FrameworkPropertyMetadata(new GradientStopCollection()));

        //void BoundingColors_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //  Type t = sender.GetType();
        //  this.InvalidateVisual();
        //}
        //private static void onBoundingColorsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //  LevelMeter lm = (LevelMeter)d;
        //}
        #endregion BoundingColors

        #endregion Properties

        //--------------------------------------------------------------------------------------------
        public LevelMeter()
            : base()
        {
            InitializeComponent();
            //SetValue(BoundingColorsProperty, new ValueColorPairs());
            //BoundingColors.ListChanged += new ListChangedEventHandler(BoundingColors_ListChanged);
        }

        private void BoundingColors_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.InvalidateVisual();
        }

        //--------------------------------------------------------------------------------------------
    }

    #region Value Converters

    public class ConverterPool
    {
        // TODO_HL : MeasureBase.GUI.CustomControls.ConverterPool this should be a singleton

        protected static ConverterPool _instance;

        public static ConverterPool Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConverterPool();
                return _instance;
            }
        }

        protected ConverterPool()
        {
        }

        // TODO_HL: MeasureBase.GUI.CustomControls.ConverterPool implement the rest of the VAlue Converters

        //public NotValueConverter NotValueConverter { get; }

        //public PercentValueConverter PercentValueConverter { }

        private ColorConverter _colorConverter;

        public ColorConverter ColorConverter
        {
            get
            {
                if (_colorConverter == null)
                    _colorConverter = new ColorConverter();
                return _colorConverter;
            }
        }

        private PercentStringValueConverter _percentStringValueConverter;

        public PercentStringValueConverter PercentStringValueConverter
        {
            get
            {
                if (_percentStringValueConverter == null)
                    _percentStringValueConverter = new PercentStringValueConverter();
                return _percentStringValueConverter;
            }
        }
    }

    [ValueConversion(typeof(double), typeof(String))]
    public class PercentStringValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double d = (double)value;
            return (d / 100).ToString("#00.00%", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }

    /// <summary>
    /// It negates the bool value
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NotValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion IValueConverter Members
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class PercentValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double d = (double)value;
            return d / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double d = (double)value;
            return d * 100;
        }

        #endregion IValueConverter Members
    }

    [ValueConversion(typeof(Orientation), typeof(Visibility))]
    public class OrientationToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Orientation ori = (Orientation)Enum.Parse(typeof(Orientation), parameter.ToString(), true);
            return ((Orientation)value) == ori ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Orientation ori = (Orientation)Enum.Parse(typeof(Orientation), parameter.ToString(), true);
            return ((Visibility)value) == Visibility.Visible ? ori : OtherOrientation(ori);
        }

        #endregion IValueConverter Members

        private Orientation OtherOrientation(Orientation ori)
        {
            return ori == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class TextWidthConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) * 2;
        }

        #endregion IValueConverter Members
    }

    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BoolBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? isOK = value as bool?;
            if (isOK != null && (bool)isOK)
            {
                return new SolidColorBrush(Colors.Green);
            }
            else
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }

    public class ColorConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double? _value = values[0] as double?;
            Color? _baseColor = values[1] as Color?;
            GradientStopCollection _boundingColors = values[2] as GradientStopCollection;

            if (_value == null)
            {
                if (_baseColor == null)
                {
                    return Colors.Transparent;
                }
                else
                {
                    return _baseColor;
                }
            }
            if (_boundingColors == null || _boundingColors.Count == 0) return _baseColor;

            _value /= 100.0f;
            int lowerIndex = _boundingColors.IndexOf(_boundingColors.LastOrDefault(gradStop => gradStop.Offset < _value));
            int exactIndex = _boundingColors.IndexOf(_boundingColors.FirstOrDefault(gradStop => gradStop.Offset == _value));
            int upperIndex = _boundingColors.IndexOf(_boundingColors.FirstOrDefault(gradStop => gradStop.Offset > _value));
            if (exactIndex != -1) return _boundingColors[exactIndex].Color;
            if (lowerIndex == -1 && upperIndex == -1) return _baseColor;
            if (lowerIndex == -1) return _boundingColors[upperIndex].Color;
            if (upperIndex == -1) return _boundingColors[lowerIndex].Color;
            double weight = (double)((_value - _boundingColors[lowerIndex].Offset) / (_boundingColors[upperIndex].Offset - _boundingColors[lowerIndex].Offset));
            return _boundingColors[lowerIndex].Color.Interpolate(_boundingColors[upperIndex].Color, weight);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IMultiValueConverter Members
    }

    public class BrushConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush((Color)ConverterPool.Instance.ColorConverter.Convert(values, targetType, parameter, culture));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IMultiValueConverter Members
    }

    public class VerticalHeightConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double? _value = values[0] as double?;
            double? _controlHeight = values[1] as double?;
            if (_value == null) return 0;
            if (_controlHeight == null) return 0;
            return _controlHeight * _value / 100;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IMultiValueConverter Members
    }

    #endregion Value Converters
}