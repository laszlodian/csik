using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using e77.MeasureBase.Extensions.ByteBuffer;
using e77.MeasureBase.Extensions.Hex;

namespace e77.MeasureBase.Data
{
    /// <summary>
    /// ReleaseNote: Sql Save/Load does not save unused flags.
    /// </summary>
    public class FlashBuffer : IComparable
    {
        /// <summary>
        /// The fist char of stored data is this type
        /// </summary>
        [Flags]
        public enum SqlStoreType
        {
            /// <summary>
            /// true: store the full size (fist 4 byte integer) and then the byte[]
            /// false: sores onyl the full byte[]
            /// </summary>
            OnlyEnd = 1,
            UseCompression = 2
        }

        public FlashBuffer()
        {
        }

        public FlashBuffer(int size)
        {
            Init(size);
        }

        public FlashBuffer(FlashBuffer fb)
        {
            Init(fb.BufferSize);
            CopyConstructor(fb);
        }

        protected void CopyConstructor(FlashBuffer fb)
        {
            for (int i = 0; i < BufferSize; i++)
            {
                byte data;
                if (fb.GetByte(i, out data))
                    this[i] = data;
            }
        }

        /// <summary>
        /// Copy arbitrary continual protion of another Flash Buffer to this flash buffer
        /// </summary>
        /// <param name="fb_in">this function will copy from this Flash Buffer</param>
        /// <param name="StartAddress_in">Start Address (inclusive)</param>
        /// <param name="EndAddress_in">End Address (inclusive)</param>
        public void CopyFrom(FlashBuffer fb_in, int StartAddress_in, int EndAddress_in)
        {
            if (EndAddress_in > BufferSize - 1)
                throw new ArgumentOutOfRangeException(String.Format("End address({0}) is pointing out of the buffer (size:{1})", EndAddress_in, BufferSize));
            if (StartAddress_in > EndAddress_in)
                throw new ArgumentException(String.Format("Start address({0}) should be lower than End Address({1})", StartAddress_in, EndAddress_in));

            for (int i = StartAddress_in; i <= EndAddress_in; i++)
            {
                byte data;
                if (fb_in.GetByte(i, out data))
                    this[i] = data;
            }
        }

        public void Init(int size)
        {
            _buffer = new byte[size];
            for (int i = 0; i < size; i++)
                _buffer[i] = DEFAULT_VALUE;

            _usedFlags = new System.Collections.BitArray(size, false);
            UsedSize = 0;
        }

        const byte DEFAULT_VALUE = 0xff;

        public bool IsDirty { get; protected set; }

        public int UsedSize { get; private set; }

        public int BufferSize
        {
            get { return _buffer.Length; }
        }

        /// <summary>
        /// -1 is there are no used byte
        /// </summary>
        public int FirstUsedByte
        {
            get
            {
                for (int res = 0; res < BufferSize; res++)
                    if (_usedFlags[res])
                        return res;
                return -1;
            }
        }

        protected byte[] _buffer;
        protected System.Collections.BitArray _usedFlags;

        public bool IsUsed(int address_in)
        {
            if (address_in < 0 || address_in > _buffer.Length)
                throw new ArgumentOutOfRangeException("address_in", address_in, "should be > 0 && < buffer size");

            return _usedFlags[address_in];
        }

        public byte this[int index_in]
        {
            get
            {
                if (index_in < 0 || index_in > _buffer.Length)
                    throw new ArgumentOutOfRangeException("index_in", index_in, "should be > 0 && < buffer size");

                return _buffer[index_in];
            }

            set
            {
                SetByte(index_in, value);
            }
        }

        public override string ToString()
        {
            return string.Format("FlashBuffer: Size= {0}; UsedSize= {1}",
                BufferSize, UsedSize);
        }

        public virtual string GetBufferContent()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte b in this._buffer)
            {
                if (i == 16)
                {
                    sb.Append("\n");
                    i = 0;
                }

                sb.Append(b.ToHex());
                i++;
            }
            return sb.ToString();
        }

        const int DECOMPRESS_BUFF_SIZE = 256;

        /// <summary>
        /// not used yet
        /// </summary>
        /// <param name="data_in"></param>
        public void SqlLoad(byte[] data_in)
        {
            SqlStoreType storeType = (SqlStoreType)data_in[0];
            int firstResultPos = 0;
            int firstDataPos = 1;  /*lenght of SqlStoreType*/

            byte[] dataBuff = new List<byte>(data_in.Where((item, index) => index >= firstDataPos)).ToArray();

            if ((storeType & SqlStoreType.UseCompression) != 0)
            {
                using (MemoryStream stream = new MemoryStream(dataBuff))
                {
                    List<byte> unzipBuff = new List<byte>();

                    using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[DECOMPRESS_BUFF_SIZE];
                        int loadedBytes;
                        int tolalBytes = 0;
                        while ((loadedBytes = zip.Read(buffer, 0, DECOMPRESS_BUFF_SIZE)) != 0)
                        {
                            tolalBytes += loadedBytes;
                            unzipBuff.AddRange(buffer);
                        }

                        if (unzipBuff.Count > tolalBytes)
                            unzipBuff.RemoveRange(tolalBytes, unzipBuff.Count - tolalBytes);
                    }

                    dataBuff = unzipBuff.ToArray();
                }
            }

            int size;
            if ((storeType & SqlStoreType.OnlyEnd) != 0)
            {
                size = (int)dataBuff.ReadUInt32(0);
                dataBuff = new List<byte>(dataBuff.Where((item, index) => index >= sizeof(int))).ToArray();
                firstResultPos = size - dataBuff.Length;
            }

            else
            {
                firstResultPos = 0;
                size = dataBuff.Length;
            }

            Init(size);
            SetRange(firstResultPos, dataBuff);
        }

        /// <summary>
        /// not used yet
        /// </summary>
        /// <param name="storeType_in"></param>
        /// <returns></returns>
        public byte[] SqlStoreObj(SqlStoreType storeType_in)
        {
            List<byte> resBuff = new List<byte>();

            if ((storeType_in & SqlStoreType.OnlyEnd) != 0)
            {
                //store size
                resBuff.AddRange(((UInt32)this.BufferSize).ToByteArray());

                int first = this.FirstUsedByte;
                if (first == -1)
                    first = this.BufferSize;

                for (; first < BufferSize; first++)
                    resBuff.Add(this._buffer[first]);
            }
            else
            {
                resBuff.AddRange(this._buffer);
            }

            if ((storeType_in & SqlStoreType.UseCompression) != 0)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress))
                    {
                        zip.Write(resBuff.ToArray(), 0, resBuff.Count);
                        zip.Close();
                    }

                    resBuff = new List<byte>(stream.GetBuffer());
                }
            }

            List<byte> res = new List<byte>(resBuff.Count + 1);
            res.Add((byte)storeType_in);
            res.AddRange(resBuff);

            return res.ToArray();
        }

        public bool GetByte(int address_in, out byte data_out)
        {
            if (address_in < 0 || address_in > _buffer.Length)
                throw new ArgumentOutOfRangeException("address_in", address_in, "should be > 0 && < buffer size");

            data_out = _buffer[address_in];

            return _usedFlags[address_in];
        }

        public byte GetByte(int address_in)
        {
            if (address_in < 0 || address_in > _buffer.Length)
                throw new ArgumentOutOfRangeException("address_in", address_in, "should be > 0 && < buffer size");

            return _buffer[address_in];
        }

        public void SetByte(int address_in, byte data_in)
        {
            if (address_in < 0 || address_in > _buffer.Length)
                throw new ArgumentOutOfRangeException("address_in", address_in, "should be > 0 && < buffer size");

            _buffer[address_in] = data_in;

            if (!_usedFlags[address_in])
                UsedSize++;

            _usedFlags[address_in] = true;
        }

        public void SetRange(int address_in, byte[] data_in)
        {
            SetRange(address_in, data_in, data_in.Length);
        }

        public void SetRange(int address_in, byte[] data_in, int dataLength_in)
        {
            if (address_in < 0 || address_in + dataLength_in > _buffer.Length)
                throw new ArgumentOutOfRangeException("address_in", address_in, "should be > 0 && < buffer size");

            Array.Copy(data_in, 0, _buffer, address_in, dataLength_in);

            for (int i = 0; i < dataLength_in; i++)
            {
                if (!_usedFlags[address_in + i])
                    UsedSize++;
                _usedFlags[address_in + i] = true;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _buffer.Length; i++)
                _buffer[i] = DEFAULT_VALUE;

            _usedFlags.SetAll(false);
            UsedSize = 0;
        }

        public void ReadFlashHexFile(byte[] buffer_in)
        {
            using (MemoryStream ms = new MemoryStream(buffer_in, false))
            {
                using (StreamReader sr = new StreamReader(ms, Encoding.ASCII))
                {
                    ReadFlashHexFile(sr);
                    sr.Close();
                }
            }
        }

        public void ReadFlashHexFile(string fileName_in)
        {
            using (StreamReader sr = new StreamReader(fileName_in, Encoding.ASCII))
            {
                ReadFlashHexFile(sr);
                sr.Close();
            }
        }

        public void ReadFlashHexFile(StreamReader sr_in)
        {
            while (!sr_in.EndOfStream)
            {
                String s = sr_in.ReadLine();

                if (s[0] != ':')
                {
                    throw new System.Exception("a HEX file nem : karakterrel kezdődik!");
                }

                int RecLen = Convert.ToInt32(s.Substring(1, 2), 16);
                int Address = Convert.ToInt32(s.Substring(3, 4), 16);
                int RecType = Convert.ToInt32(s.Substring(7, 2), 16);

                // sor crc ellenőrzése

                int i;
                int crc = 0;
                for (i = 0; i < RecLen + 5; i++)
                {
                    int b = Convert.ToInt32(s.Substring(i * 2 + 1, 2), 16);
                    crc = (crc - b) & 255;
                }
                if (crc != 0)
                {
                    throw new System.Exception("CRC hiba a HEX file-ban!");
                }

                // beírás FlashBufferbe;
                if (RecType == 0)
                {
                    for (i = 0; i < RecLen; i++)
                    {
                        byte b = Convert.ToByte(s.Substring(i * 2 + 9, 2), 16);
                        SetByte(Address, b);

                        Address++;
                    }
                }
            };
        }

        public UInt32 GetOneAndAHalfDimensionCRC()
        {
            /*[0] XOR crc of each byte (bits are rotated to left, in order to modify bit position (must be different than next 3 byte) )
             * [1], [2], [3] XOR crcs of each 3. bytes */

            byte[] crcArr = new byte[] { 0, 0, 0, 0 };
            int ipos = 0;
            foreach (byte b in _buffer)
            {
                byte rotated = (byte)(b << 1);
                if ((b & 0x80) != 0)
                    rotated |= 1; //rotate

                crcArr[(ipos % 3) + 1] ^= b;    //the one dimension: each 3. bytes
                crcArr[0] ^= rotated;           //and a half: all (rotated) bytes
            }

            UInt32 res = 0;
            foreach (byte b in crcArr)
            {
                res <<= 8;
                res += b;
            }

            return res;
        }

        public virtual void GenerateCRC()
        {
            throw new NotImplementedException();
        }

        #region IComparable Members

        public int CompareTo(object obj_in)
        {
            if (obj_in == null || (obj_in is byte[]) == false)
                return -1;

            FlashBuffer other = new FlashBuffer();
            other.SqlLoad(obj_in as byte[]);

            if (this.BufferSize != other.BufferSize)
                return -1;

            for (int i = 0; i < this.BufferSize; i++)
            {
                if (this.GetByte(i) != other.GetByte(i))
                    return -1;
            }

            return 0;
        }

        #endregion IComparable Members
    }
}