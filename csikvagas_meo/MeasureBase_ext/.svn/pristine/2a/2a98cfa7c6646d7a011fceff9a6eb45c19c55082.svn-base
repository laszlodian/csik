using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public class DeviceCollection : Dictionary<string, IDevice>, IDeviceGeneralFunctions, IDisposable
    {
        public virtual void Add(IDevice device)
        {
            this.Add(device.CollectionKey, device);
        }

        #region IDeviceGeneralFunctions Members

        public virtual bool TestConnection()
        {
            bool ret = true;
            foreach (IDevice device in this.Values)
            {
                ret = ret && ((IDeviceGeneralFunctions)device).TestConnection();
            }
            return ret;
        }

        public virtual void ResetDevice()
        {
            foreach (IDevice device in this.Values)
            {
                ((IDeviceGeneralFunctions)device).ResetDevice();
            }
        }

        public virtual void InitConnection()
        {
            foreach (IDevice device in this.Values)
            {
                ((IDeviceGeneralFunctions)device).InitConnection();
            }
        }

        public virtual void CloseConnection()
        {
            foreach (IDevice device in this.Values)
            {
                ((IDeviceGeneralFunctions)device).CloseConnection();
            }
        }

        #endregion IDeviceGeneralFunctions Members

        #region IDisposable Members

        public void Dispose()
        {
            foreach (IDevice device in this.Values)
            {
                device.Dispose();
            }
        }

        #endregion IDisposable Members
    }
}