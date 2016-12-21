using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Communication
{
    public interface ICheckSum
    {
        void AddChecksum(ICollection<byte> data);
        bool Check(IEnumerable<byte> data);
        int Length { get; }
    }

    public class CheckSumAdd1Byte : ICheckSum
    {
        public CheckSumAdd1Byte()      { ; }

        public CheckSumAdd1Byte(byte startOffset)
        {
            _startOffset = startOffset;
        }

        readonly byte _startOffset;

        public int GetChecksum(IEnumerable<byte> data)
        {
            int crc = _startOffset;
            foreach (byte b in data)
            {
                crc += b;
            }

            return ((~crc & 0xff) + 1) & 0xff;
        }

        #region ICheckSum Members

        public void AddChecksum(ICollection<byte> data)
        {
            data.Add((byte)GetChecksum(data));
        }

        public bool Check(IEnumerable<byte> data)
        {
            return GetChecksum(data) == 0;
        }

        public int Length
        {
            get { return 1; }
        }

        #endregion
    }

    public class CheckSumXor1Byte : ICheckSum
    {
        public CheckSumXor1Byte()        { ;  }
        public CheckSumXor1Byte(byte skipBytes)
        {
            _skipBytes = skipBytes;
        }
        readonly byte _skipBytes;

        public int GetChecksum(IEnumerable<byte> data)
        {
            byte crc = 0;
            int i = 1;
            foreach (byte b in data)
            {
                if (i > _skipBytes)
                {
                    crc ^= b;
                }
                i++;
            }
            return crc;
        }

        #region ICheckSum Members

        public void AddChecksum(ICollection<byte> data)
        {
            data.Add((byte)GetChecksum(data));
        }

        public bool Check(IEnumerable<byte> data)
        {
            return GetChecksum(data) == 0;
        }

        public int Length
        {
            get { return 1; }
        }

        #endregion
    }

    /// <summary>
    /// For the communications that uses no checksum
    /// </summary>
    public class NoCheckSum : ICheckSum
    {
        public NoCheckSum()  { ; }

        #region ICheckSum Members

        public void AddChecksum(ICollection<byte> data) { ; }

        public bool Check(IEnumerable<byte> data)
        {
            return true;
        }

        public int Length
        {
            get { return 0; }
        }

        #endregion
    }
}
