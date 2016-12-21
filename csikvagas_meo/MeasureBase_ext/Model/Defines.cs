using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Model
{
    public interface ICountable
    {
        void Count();

        string Text { get; set; }
    }

    public interface IPreProcessable
    {
        bool PreProcess();
    }

    public interface IPostProcessable
    {
        bool PostProcess();
    }

    public interface ICollectionKey
    {
        /// <summary>
        /// CollectionKey for MeasureBaseClass
        /// </summary>
        string CollectionKey { get; }
    }

    /// <summary>
    /// FW set it when the object added to a MeasureDatabase
    /// 
    /// Cast example: implement interface explicitly and:
    /// public MeasureMeo MyMeasure { get { return (MeasureMeo)(this as IMyMeasure).MyMeasure; } } 
    /// </summary>
    public interface IMyMeasure
    {
        MeasureBaseClass MyMeasure { get; set; }
    }

    public interface ICheckableBase
    {
        /// <summary>
        /// Contains return value of ICheckable.Check() if the class implements ICheckable. Else manual set required.
        /// </summary>
        bool? CheckResult { get; set; }

        /// <summary>
        /// Contains 'ok' column of the SQL table, if the object has been loaded from SQL
        /// </summary>
        bool? SqlStoredCheckResult { get; set; }
    }
    
    public interface ICheckable : ICheckableBase
    {
        bool Check();
    }

    /// <summary>
    /// for MeasureSteps, Measure cannot be ICheckableThreeState
    /// </summary>
    public interface ICheckableThreeState : ICheckableBase
    {
        bool? Check();
    }

    /// <summary>
    /// Only for MeasureEnvironment.InfoForm information dialog:
    /// the result of this interface will be appeares as childrens at the InfoForm
    /// </summary>
    public interface IChildInfo
    {
        ICollection<string> ChildInfo { get; }
    }

    /// <summary>
    /// Only for MeasureEnvironment.InfoForm information dialog:
    /// the result of this interface will be appeares as childrens at the InfoForm
    /// </summary>
    public interface IParentInfo
    {
        ICollection<string> ParentInfo { get; }
    }

    public class SuccessfulEventArgs : EventArgs
    {
        public SuccessfulEventArgs() { ;}
        public SuccessfulEventArgs(bool successful)
        {
            Successful = successful;
        }
        public bool Successful { get; set; }
    }

    public delegate void SuccessfulEventHandler(object sender, SuccessfulEventArgs e);


    public class WrongSqlRecordNumberException : MeasureBaseException
    {
        public WrongSqlRecordNumberException(bool saveNotLoad_in, SqlTableDescriptorHierarhyTableBase table_in) :
            base(string.Format(" SQL {0} - Inconsistent record number at {1}",
            saveNotLoad_in ? "Save" : "Load",
            table_in))
        { ;}

        public SqlTableDescriptorHierarhyTableBase TableDescriptor { get; private set; }
        public bool Save_nLoad { get; private set; }
    }

    public class WrongCheckNumberException : MeasureBaseException
    {
        public WrongCheckNumberException(MeasureRoot measure_in, int assertedNr_in, int realNr_in)  
            : base(string.Format("WrongCheckNumberException at measure {0}. Asserted: {1}, real {2}", 
                measure_in, assertedNr_in, realNr_in))  {;}
    }
}
