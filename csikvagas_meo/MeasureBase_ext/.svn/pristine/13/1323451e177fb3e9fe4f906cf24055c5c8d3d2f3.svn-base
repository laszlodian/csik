using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public interface IDeviceGeneralFunctions
    {
        bool TestConnection();

        void ResetDevice();

        void InitConnection();

        void CloseConnection();
    }

    public interface IDevice : IDeviceGeneralFunctions, IDisposable
    {
        string CollectionKey { get; }
    }

    public abstract class Device<T> : IDevice
         where T : IProtocol, IDisposable
    {
        public Device()
        {
        }

        protected T _protocol;

        #region IDevice Members

        public abstract string CollectionKey { get; }

        #endregion IDevice Members

        #region ICommunicationGeneralFunctions Members

        public abstract bool TestConnection();

        public abstract void ResetDevice();

        public abstract void InitConnection();

        public abstract void CloseConnection();

        #endregion ICommunicationGeneralFunctions Members

        #region IDisposable design pattern for base class imlementing IDisposable
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    _protocol.Dispose();
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // If disposing is false, only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion IDisposable design pattern for base class imlementing IDisposable
    }
}