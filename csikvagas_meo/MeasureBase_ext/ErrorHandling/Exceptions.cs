using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase
{
  /// <summary>
  /// Base class for all exception (MeasureBase and applications)
  /// </summary>
  [global::System.Serializable]
  public class E77Exception : Exception
  {
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public E77Exception() { }
    public E77Exception(string message) : base(message) { }
    public E77Exception(string message, Exception inner) : base(message, inner) { }
    protected E77Exception(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context)
      : base(info, context) { }
  }

    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message) :
            base(message)
        { ; }

        public InvalidConfigurationException(string format_in, params object[] params_in) :
            base(string.Format(format_in, params_in))
        { ; }

        public InvalidConfigurationException(string message, Exception innerException) :
            base(message, innerException)
        { ; }
    }

    /// <summary>
    /// Base class for all exception of MesaureBase
    /// </summary>
    public class MeasureBaseException : E77Exception
    {
        public MeasureBaseException() { ; }

        public MeasureBaseException(string message) :
            base(message)
        { ;}

        public MeasureBaseException(string format_in, params object[] params_in) :
            base(string.Format(format_in, params_in))
        { ;}

        public MeasureBaseException(string message, Exception innerException) :
            base(message, innerException)
        { ;}
    }

    /// <summary>
    /// Class for exception of Mesaure
    /// </summary>
    public class MeasureException : E77Exception
    {
        public MeasureException() { ; }

        public MeasureException(string message) :
            base(message)
        { ;}

        public MeasureException(string format_in, params object[] params_in) :
            base(string.Format(format_in, params_in))
        { ;}

        public MeasureException(string message, Exception innerException) :
            base(message, innerException)
        { ;}
    }

    /// <summary>
    /// Base class for all internal exception of MesaureBase
    /// </summary>
    public class MeasureBaseInternalException : MeasureBaseException
    {
        public MeasureBaseInternalException(string message) :
            base(message)      { ; }

        public MeasureBaseInternalException(string format_in, params object[] params_in) :
            base(string.Format(format_in, params_in))
        { ; }

        public MeasureBaseInternalException(string message, Exception innerException) :
            base(message, innerException)   { ; }
    }

    /// <summary>
    /// Base class for all internal exception of the Mesaure
    /// </summary>
    public class MeasureInternalException : E77Exception
    {
        public MeasureInternalException(string message, Exception innerException_in) :
            base(string.Format("Hiba a mérőprogramban: {0}", message), innerException_in) { ; }

        public MeasureInternalException(string format_in, params object[] params_in) :
            this(string.Format(format_in, params_in), (Exception)null)
        { ; }

        public MeasureInternalException(string message) :
            this(message, (Exception)null) { ; }
    }

    public class NotFoundException : MeasureBaseException
    {
        public NotFoundException() { ;}
        public NotFoundException(string message) :
            base(message) { ; }

        public NotFoundException(string message, Exception innerException) :
            base(message, innerException) { ; }

        public NotFoundException(string format_in, params object[] params_in) :
            this(string.Format(format_in, params_in) ) 
        { ; }
    }

    public class SqlStructureInconsistencyException : MeasureBaseException
    {
        public SqlStructureInconsistencyException(string format_in, params object[] params_in) :
            base(string.Format("Inconsistent SQL: {0}", string.Format(format_in, params_in) ) ) 
        { ; }
    }
    
    public class SqlNoValueException : MeasureBaseException
    {
        public SqlNoValueException(string id, DateTime baseItme, int newerThanHour_in) :
            base(string.Format(Resources.EXCEPTION_SQL_NO_DATA_FOR_WITHIN_HOURS,
            id, baseItme, newerThanHour_in))
        { ;}

        public SqlNoValueException(string id) :
            base(string.Format(Resources.EXCEPTION_SQL_NO_DATA_FOR, id))
        { ;}
    }

    [Serializable]
    public class SingletonException : Exception
    {
        public SingletonException() { }
        public SingletonException(string message) : base(message) { }
        public SingletonException(string message, Exception inner) : base(message, inner) { }
        protected SingletonException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    [Serializable]
    public class SqlInsertException : Exception
    {
        public SqlInsertException() { }
        public SqlInsertException(string message) : base(message) { }
        public SqlInsertException(string message, Exception inner) : base(message, inner) { }
        protected SqlInsertException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    public class SqlNotFoundException : MeasureBaseException
    {
        public SqlNotFoundException(string tableName_in, string format_in, object param_in) :
            base( string.Format( Resources.EXCEPTION_SQL_NO_ROW_AT_TABLE, tableName_in, string.Format(format_in, param_in))) 
        {
            TableName = tableName_in;
        }

        public SqlNotFoundException(string tableName_in, Dictionary<string, object> sqldata_in) :
            this(tableName_in, Resources.SEARCHING_VALUES, sqldata_in.ItemsToString())  
        {
            SqlData = new Dictionary<string,object>( sqldata_in );
        }

        public Dictionary<string, object> SqlData { get; private set; }
        public string TableName { get; private set; }
    }
}
