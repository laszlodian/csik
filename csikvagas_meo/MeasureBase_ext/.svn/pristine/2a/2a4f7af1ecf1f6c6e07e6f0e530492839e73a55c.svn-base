using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;


namespace e77.MeasureBase.Communication
{
    public interface ILisPort : IPortProxy, INotifyPropertyChanged
    {
        //bool TestConnection();

        byte ReadByte();

        void WriteByte(byte data_in);

        IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback);

        //string ReadLine();

        //void SendAck();
    }
}