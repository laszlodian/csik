using System;

namespace e77.MeasureBase.Communication
{
    public enum EPortProxyPropNames
    {
        IsOpen,
    }

    public interface IPortProxy : IDisposable
    {
        string PortName { get; }

        void InitPort();

        bool IsOpen { get; }

        void Open();

        void Close();

        int ReadTimeout { get; set; }

        int WriteTimeout { get; set; }

        void Write(byte[] buffer, int offset, int count);

        int Read(byte[] buffer, int offset, int count);

        bool IsBytesToReadImplemented { get; }

        int BytesToRead { get; }

        void DiscardInBuffer();

        bool IsReadBufferEmpty { get; }

        bool IsWriteBufferEmpty { get; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">class of the port that should be initialised</typeparam>
    public interface IPortProxy<T> : IPortProxy
        where T : class
    {
        Action<T> InitPortAction { get; set; }
    }
}