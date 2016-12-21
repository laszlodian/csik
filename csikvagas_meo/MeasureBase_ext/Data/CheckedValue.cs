using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using e77.MeasureBase;
using e77.MeasureBase.Model;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase.Data
{
    /// <summary>
    /// Note: Int and float descent needed because I have no found to add and substract generic type (T a, T b => a+b not working)
    /// </summary>
    public class CheckedInt : CheckedValue<int>
    {
        public CheckedInt() { ;}

        public CheckedInt(int value_in)
            : base(value_in)
        { ;}

        public CheckedInt(CheckedInt obj_in)
            : base(obj_in)
        { ;}

        public CheckedInt(int value_in, int ref_in, int refBound_in)
            : base(value_in, ref_in, refBound_in, ref_in - refBound_in, ref_in + refBound_in)
        { ;}

        public void SetReference(int ref_in, int refBound_in)
        {
            base.SetBounds(ref_in, refBound_in, ref_in - refBound_in, ref_in + refBound_in);
        }

        public override int GetHashCode()
        {
            return (Value + RefLowBound + RefHiBound).GetHashCode();
        }
    }

    public class CheckedFloat : CheckedValue<float>
    {
        public CheckedFloat() { ;}

        public CheckedFloat(CheckedFloat obj_in)
            : base(obj_in)
        { ;}

        public CheckedFloat(float value_in)
            : base(value_in)
        { ;}

        public CheckedFloat(float value_in, float ref_in, float refBound_in)
            : base(value_in, ref_in, refBound_in, ref_in - refBound_in, ref_in + refBound_in)
        { ; }

        public void SetReference(float ref_in, float refBound_in)
        {
            base.SetBounds(ref_in, refBound_in, ref_in - refBound_in, ref_in + refBound_in);
        }

        public override int GetHashCode()
        {
            return (Value + RefLowBound + RefHiBound).GetHashCode();
        }
    }

    abstract public class CheckedValueBase : INotifyPropertyChanged
    {
        abstract public bool Check();

        public object Value { get; set; }

        public object RefLowBound { get; protected set; }

        public object RefHiBound { get; protected set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName_in)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName_in));
        }

        #endregion INotifyPropertyChanged Members
    }

    public class CheckedValue<T> : CheckedValueBase
        where T : IComparable<T>
    {
        internal CheckedValue()
        { ; }

        internal CheckedValue(T value_in)
        {
            Value = value_in;
        }

        internal CheckedValue(T refLowBound_in, T refHiBound_in)
        {
            RefLowBound = refLowBound_in;
            RefHiBound = refHiBound_in;
        }

        internal CheckedValue(T value_in, T ref_in, T refBound_in, T refLowBound_in, T refHiBound_in)
            : this(value_in)
        {
            SetBounds(ref_in, refBound_in, refLowBound_in, refHiBound_in);
            Value = value_in;
        }

        internal CheckedValue(CheckedValue<T> obj_in)
            : this(obj_in.Value, obj_in._Ref, obj_in._RefBoundary, obj_in.RefLowBound, obj_in.RefHiBound)
        {
            ;
        }

        internal void SetBounds(T ref_in, T refBound_in, T refLowBound_in, T refHiBound_in)
        {
            _Ref = ref_in;
            _RefBoundary = refBound_in;

            RefLowBound = refLowBound_in;
            RefHiBound = refHiBound_in;

            Check();
        }

        public void SetBounds(T lowBound_in, T hiBound_in)
        {
            RefLowBound = lowBound_in;
            RefHiBound = hiBound_in;
        }

        private const int INVALID_RESULT = 0xcd;

        /// <summary>
        ///
        /// </summary>
        /// <returns>-1: value is smaler than the reference bound. 0- OK, 1: value is bigger than the reference bound. </returns>
        public override bool Check()
        {
            if (base.Value == null)
                throw new Exception("Value has not been set before calling Check().");

            if (Value.CompareTo(RefLowBound) < 0)
                _Result = -1;
            else if (Value.CompareTo(RefHiBound) <= 0)
                _Result = 0;
            else
                _Result = 1;

            return IsOK;
        }

        public string ResultStr
        {
            get
            {
                switch (_Result)
                {
                    case -1:
                        return Resources.CHECKEDVALUE_BELOW_LIMIT;
                    case 0:
                        return Resources.CHECKEDVALUE_WITHIN_LIMIT;
                    case 1:
                        return Resources.CHECKEDVALUE_ABOVE_LIMIT;
                    default:
                        throw new MeasureBaseInternalException(string.Format("Invalid _Result: '{0}'. this. Perhaps, this.Check has not been called.", _Result));
                }
            }
        }

        public string ToLocalizedString()
        {
            if (_Result == INVALID_RESULT)
                throw new InvalidOperationException();

            return string.Format(Resources.CHECKEDVALUE_TOSTRING,
                Value, RefLowBound, RefHiBound, ResultStr);
        }

        public override string ToString()
        {
            if (_Result != INVALID_RESULT)
                return string.Format("CheckedValue: value '{0}', Boundaries: '{1}' .. '{2}'. Result: {3}",
                    Value, RefHiBound, RefLowBound, ResultStr);
            else
                return string.Format("CheckedValue: value '{0}', , Boundaries: '{1}' .. '{2}'. Result: not checked",
                    Value, RefHiBound, RefLowBound);
        }

        public bool IsOK
        {
            get
            {
                if (_Result == INVALID_RESULT)
                    throw new InvalidOperationException("this.Check() must be called before using this property.");

                return _Result == 0;
            }
        }

        new public T Value
        {
            get
            {
                if (base.Value != null)
                    return (T)base.Value;
                else
                    return default(T);
            }
            set 
            {
                base.Value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// Optional additional info
        /// </summary>
        public T _RefBoundary { get; private set; }

        /// <summary>
        /// Optional additional info
        /// </summary>
        protected internal T _Ref { get; private set; }

        new public T RefLowBound
        {
            get
            {
                return (T)base.RefLowBound;
            }
            private set { base.RefLowBound = value; }
        }

        new public T RefHiBound
        {
            get { return (T)base.RefHiBound; }
            private set { base.RefHiBound = value; }
        }

        /// <summary>
        /// -1  value is smaler than the reference bound.
        /// 0   inside bounds
        /// +1   value is bigger than the reference bound.
        /// </summary>
        internal protected int _Result = INVALID_RESULT; //initial value (check must be called)

       
    }
}