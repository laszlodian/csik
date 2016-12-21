using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using e77.MeasureBase;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase.Electronics
{
    public class Resistor : INotifyPropertyChanged
    {
        public const int MAX_EXPONENT = 10;// TOhm handled as infinite

        public enum EResistorSerie { E12 = 12, E24 = 24, E48 = 48, E96 = 96, E192 = 192 };

        #region Constructor

        static Resistor()
        {
            _resistorSerieValues.Add(EResistorSerie.E12,
                new int[] { 100, 120, 150, 180, 220, 270, 330, 390, 470, 560, 680, 820, 1000/*! for last average*/ });

            _resistorSerieValues.Add(EResistorSerie.E24,
                new int[] { 100, 110, 120, 130, 150, 160, 180, 200, 220, 240, 270, 300,
                            330, 360, 390, 430, 470, 510, 560, 620, 680, 750, 820, 910, 1000/*!*/ });

            #region E48
            _resistorSerieValues.Add(EResistorSerie.E48,
                new int[] { 100, 105, 110, 115, 121, 127, 133, 140, 147, 154, 162, 169,
                            178, 187, 196, 205, 215, 226, 237, 249, 261, 274, 287, 301,
                            316, 332, 348, 365, 383, 402, 422, 442, 464, 487, 511, 536,
                            562, 590, 619, 649, 681, 715, 750, 787, 825, 866, 909, 953, 1000/*!*/});
            #endregion E48

            #region E96
            _resistorSerieValues.Add(EResistorSerie.E96,
                new int[] { 100, 102, 105, 107, 110, 113, 115, 118, 121, 124, 127, 130,
                            133, 137, 140, 143, 147, 150, 154, 158, 162, 165, 169, 174,
                            178, 182, 187, 191, 196, 200, 205, 210, 215, 221, 226, 232,
                            237, 243, 249, 255, 261, 267, 274, 280, 287, 294, 301, 309,
                            316, 324, 332, 340, 348, 357, 365, 374, 383, 392, 402, 412,
                            422, 432, 442, 453, 464, 475, 487, 491, 511, 523, 536, 549,
                            562, 576, 590, 604, 619, 634, 649, 665, 681, 698, 715, 732,
                            750, 768, 787, 806, 825, 845, 866, 887, 909, 931, 959, 976, 1000/*!*/});
            #endregion E96

            #region E192
            _resistorSerieValues.Add(EResistorSerie.E192,
                new int[] { 100, 101, 102, 104, 105, 106, 107, 109, 110, 111, 113, 114,
                            115, 117, 118, 120, 121, 123, 124, 126, 127, 129, 130, 132,
                            133, 135, 137, 138, 140, 142, 143, 145, 147, 149, 150, 152,
                            154, 156, 158, 160, 162, 164, 165, 167, 169, 172, 174, 176,
                            178, 180, 182, 184, 187, 189, 191, 193, 196, 198, 200, 203,
                            205, 208, 210, 213, 215, 218, 221, 223, 226, 229, 232, 234,
                            237, 240, 243, 246, 249, 252, 255, 258, 261, 264, 267, 271,
                            274, 277, 280, 284, 287, 291, 294, 298, 301, 305, 309, 312,
                            316, 320, 324, 328, 332, 336, 340, 344, 348, 352, 357, 361,
                            365, 370, 374, 379, 383, 388, 392, 397, 402, 407, 412, 417,
                            422, 427, 432, 437, 442, 448, 453, 459, 464, 470, 475, 481,
                            487, 493, 499, 505, 511, 517, 523, 530, 536, 542, 549, 556,
                            562, 569, 576, 583, 590, 597, 604, 612, 619, 626, 634, 642,
                            649, 657, 665, 673, 681, 690, 698, 706, 715, 723, 732, 741,
                            750, 759, 768, 777, 787, 796, 806, 816, 825, 835, 845, 856,
                            866, 876, 887, 898, 909, 920, 931, 942, 953, 965, 976, 988, 1000 });
            #endregion E192

            _defaultSerie = (EResistorSerie)(-1); //invalid
        }

        public Resistor()
        {
            _resistorSerie = Resistor.DefaultSerie;
        }

        public Resistor(EResistorSerie serie_in)
        {
            _resistorSerie = serie_in;
        }

        public Resistor(EResistorSerie serie_in, int r)
        {
            _resistorSerie = serie_in;
            ValueInt = r;
        }

        public Resistor(int r)
        {
            _resistorSerie = Resistor.DefaultSerie;
            ValueInt = r;
        }

        public Resistor(EResistorSerie serie_in, float r)
        {
            _resistorSerie = serie_in;
            ValueFloat = r;
        }

        public Resistor(float r)
        {
            _resistorSerie = Resistor.DefaultSerie;
            ValueFloat = r;
        }

        public Resistor(Resistor obj_in)
        {
            this._exponent = obj_in._exponent;
            this._mantissa = obj_in._mantissa;
            this._resistorSerie = obj_in._resistorSerie;
        }

        public Resistor(EResistorSerie serie_in, int mantissa_in, int exponent_in)
        {
            _resistorSerie = serie_in;
            _mantissa = mantissa_in;
            _exponent = exponent_in;
        }

        #endregion Constructor

        #region Fields
        static EResistorSerie _defaultSerie = (EResistorSerie)(-1);

        private EResistorSerie _resistorSerie;
        private int _exponent;

        /// <summary>
        /// 100 <= mantissa < 1000
        /// </summary>
        private int _mantissa = -1;//invalid (0 Ohm is valid resistance)

        #endregion Fields

        #region Properties

        static public EResistorSerie DefaultSerie
        {
            get
            {
                if (DesignMode.IsOn())
                    return EResistorSerie.E24; //HACK: for WPF UI VS2008 IDE Design time support (else it will display error at design time)

                if ((int)_defaultSerie == -1)
                    throw new InvalidOperationException("Resistor.DefaultSerie has not been set. This function uses DefaultSerie, please set it before using this API.");
                return _defaultSerie;
            }

            set
            {
                if (Enum.GetValues(typeof(EResistorSerie)).Cast<EResistorSerie>().Contains(value))
                    _defaultSerie = value;
                else
                    throw new ArgumentOutOfRangeException("Only the predefined Series can be chosen");
            }
        }

        public bool IsInitialized { get { return _mantissa != -1; } }

        public float ValueFloat
        {
            get
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("Not initialized (use appropiate constructor, or a Set function).");
                //AproximateInternal(_resistorSerie, (float)(Math.Pow((double)10, (double)_exponent) * _mantissa), out _exponent, out _mantissa); // TODO_HL: 4 only approximate when _resistorSerie has been changed
                return (float)(Math.Pow((double)10, (double)_exponent) * _mantissa);
            }

            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("A resistor should have non negative value");
                AproximateInternal(_resistorSerie, value, out _exponent, out _mantissa);
                OnPropertiesChanged();
            }
        }

        public int ValueInt
        {
            get
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("Not initialized (use appropiate constructor, or a Set function).");

                if (_exponent < 0)
                    throw new InvalidOperationException(string.Format("Value {0} cannot be represented by integer, use ValueFloat property.", ValueFloat));

                return (int)(ValueFloat + .5d);
            }

            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("A resistor should have non negative value");
                this.ValueFloat = (float)value;
                OnPropertiesChanged();
            }
        }

        #endregion Properties

        #region Methods

        public virtual void Clear()
        {
            _mantissa = -1;
        }

        /// <summary>
        /// Approximate the proper resistor value from the argumentum. Uses Resistor.DefaultSreie.
        /// </summary>
        /// <param name="value_in">Required arbitrary resistance value. </param>
        /// <returns>It returns the closest resistor value to the argument</returns>
        public static Resistor Approximate(int value_in)
        {
            return Approximate(Resistor.DefaultSerie, (float)value_in);
        }

        /// <summary>
        /// Approximate the proper resistor value from the argumentum.
        /// </summary>
        /// <param name="value_in">Required arbitrary resistance value.</param>
        /// <returns>It returns the closest resistor value to the argument</returns>
        public static Resistor Approximate(EResistorSerie serie_in, int value_in)
        {
            return Approximate(serie_in, (float)value_in);
        }

        /// <summary>
        /// Approximate the proper resistor value from the argumentum. Uses Resistor.DefaultSreie.
        /// </summary>
        /// <param name="value_in">Required arbitrary resistance value.</param>
        /// <returns>It returns the closest resistor value to the argument</returns>
        public static Resistor Approximate(float value_in)
        {
            int exponent;
            int mantissaInt;
            AproximateInternal(Resistor.DefaultSerie, value_in, out exponent, out mantissaInt);

            return new Resistor(Resistor.DefaultSerie, mantissaInt, exponent);
        }

        /// <summary>
        /// Approximate the proper resistor value from the argumentum.
        /// </summary>
        /// <param name="value_in">Required arbitrary resistance value.</param>
        /// <returns>It returns the closest resistor value to the argument</returns>
        public static Resistor Approximate(EResistorSerie serie_in, float value_in)
        {
            int exponent;
            int mantissaInt;
            AproximateInternal(serie_in, value_in, out exponent, out mantissaInt);

            return new Resistor(serie_in, mantissaInt, exponent);
        }

        private static void AproximateInternal(EResistorSerie serie_in, float value_in, out int exponent, out int mantissaInt)
        {
            //count mantissa:
            float mantissa;
            exponent = CountExponent(value_in, out mantissa);

            if (exponent == Resistor.MAX_EXPONENT)
            {
                //infinite
                mantissaInt = 10;
                exponent = Resistor.MAX_EXPONENT;
                return;
            }

            //get nearest value
            int nearestResistorIndex = 0;
            float[] averages = GeometricAverages(serie_in);
            while (averages[nearestResistorIndex] < mantissa)
            {
                nearestResistorIndex++;
                if (averages.Length == nearestResistorIndex) //overflow
                {
                    nearestResistorIndex = 0;
                    exponent++;
                    break;
                }
            }
            mantissaInt = _resistorSerieValues[serie_in][nearestResistorIndex];
        }

        public bool IsInfinite
        {
            get
            {
                return (this._exponent == Resistor.MAX_EXPONENT);
            }
        }

        override public String ToString()
        {
            if (IsInfinite)
                return Resources.RESISTOR_INFINITE;
            else if (!IsInitialized)
                return Resources.NOT_INITIALIZED;

            float mantissa = (float)_mantissa / 100; //convert mantissa intervall 1..10
            int exponent = _exponent + 2;

            string unit;
            if (exponent >= 0)
                switch (exponent / 3)
                {
                    case 0:
                        unit = "Ohm";
                        break;
                    case 1:
                        unit = "kOhm";
                        break;
                    case 2:
                        unit = "MOhm";
                        break;
                    case 3:
                        unit = "GOhm";
                        break;
                    case 4:
                        unit = "TOhm";
                        break;
                    default:
                        throw new InvalidConfigurationException(string.Format("Invalid unit exponent {0} at resistor {1}", exponent / 3, ValueFloat));
                }
            else
            {
                unit = "mOhm";
                exponent += 3;
            }

            mantissa *= (float)(Math.Pow((double)10, (double)exponent % 3));

            return string.Format("{0} {1}", mantissa.ToString(), unit);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value_in"></param>
        /// <param name="mantissa_out">10..100</param>
        /// <returns></returns>
        private static int CountExponent(float value_in, out float mantissa_out)
        {
            int exponent = 0;
            while (value_in > 1000f)
            {
                value_in /= 10;
                exponent++;

                if (exponent == MAX_EXPONENT)
                {
                    mantissa_out = 10;
                    return MAX_EXPONENT; //infinite
                }
            }

            while (value_in < 100f)
            {
                value_in *= 10;
                exponent--;
            }

            mantissa_out = value_in;
            return exponent;
        }

        private static float[] GeometricAverages(EResistorSerie serie_in)
        {
            if (!_geometricAverages.ContainsKey(serie_in))
            {
                List<float> averages = new List<float>();
                for (int i = 0; i < _resistorSerieValues[serie_in].Length - 1; i++)
                {
                    float average = (float)_resistorSerieValues[serie_in][i] * _resistorSerieValues[serie_in][i + 1];
                    averages.Add((float)Math.Sqrt((double)average));
                }

                _geometricAverages[serie_in] = averages.ToArray();
            }
            return _geometricAverages[serie_in];
        }

        static Dictionary<EResistorSerie, int[]> _resistorSerieValues = new Dictionary<EResistorSerie, int[]>();
        static Dictionary<EResistorSerie, float[]> _geometricAverages = new Dictionary<EResistorSerie, float[]>();
        #endregion Methods

        #region INotifyPropertyChanged Members

        private void OnPropertiesChanged()
        {
            OnPropertyChanged("ValueFloat");
            OnPropertyChanged("ValueInt");
        }

        public void OnPropertyChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members
    }

    public static class ResistorExtensionMethods
    {
        static public Resistor GetResistor(this int obj_in, Resistor.EResistorSerie serie_in)
        {
            return new Resistor(serie_in, obj_in);
        }

        static public Resistor GetResistor(this float obj_in, Resistor.EResistorSerie serie_in)
        {
            return new Resistor(serie_in, obj_in);
        }
    }

    [ValueConversion(typeof(Resistor), typeof(String))]
    public class ResistorStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return String.Empty;
            if (value.GetType() != typeof(Resistor)) throw new ArgumentException("wrong type");

            return ((Resistor)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException();

            string resString = value as string;
            if (resString == null) throw new ArgumentException("wrong type");
            if (resString.Length == 0) return null;
            StringBuilder sb = new StringBuilder();
            resString = resString.TrimEnd(new char[] { ' ', '\n', '\r' });
            if (resString.ToLowerInvariant().EndsWith("ohm"))
                resString = resString.Remove(resString.Length - 3 - 1);
            float resValue = resString.SiParse();
            return new Resistor(resValue);
        }

        #endregion IValueConverter Members
    }
}