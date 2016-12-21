using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using e77.MeasureBase;

namespace e77.MeasureBase.Extensions.ByteBuffer
{
    public static class ExtensionMethodsByteBuffer
    {
        static public UInt16[] ToUInt16Array(this byte[] buffer_in)
        {
            if (buffer_in.Count() % sizeof(UInt16) != 0)
                throw new ArgumentException();

            List<UInt16> ret = new List<UInt16>();
            for (int i = 0; i < buffer_in.Count(); i += sizeof(UInt16))
            {
                ret.Add(ToUInt16.SetWord(buffer_in[i + 1], buffer_in[i]));
            }

            return ret.ToArray();
        }

        static public Int16[] ToInt16Array(this byte[] buffer_in)
        {
            List<Int16> res = new List<Int16>();
            if (buffer_in.Count() % sizeof(Int16) != 0)
                throw new ArgumentException();

            using (MemoryStream ms = new MemoryStream(buffer_in, 0, buffer_in.Count(), false))
            using (BinaryReader br = new BinaryReader(ms))
                for (int i = 0; i < buffer_in.Count(); i += sizeof(Int16))
                    res.Add(br.ReadInt16());  // TODO_HL: 5 ?Gyula: jó ez így hogy pl. 100szor felépítjük a Stream-et meg a BinaryReadert

            return res.ToArray();
        }

        static public Int32[] ToInt32Array(this byte[] buffer_in)
        {
            List<Int32> res = new List<Int32>();
            if (buffer_in.Count() % sizeof(Int32) != 0)
                throw new ArgumentException();

            using (MemoryStream ms = new MemoryStream(buffer_in, 0, buffer_in.Count(), false))
            using (BinaryReader br = new BinaryReader(ms))
                for (int i = 0; i < buffer_in.Count(); i += sizeof(Int32))
                    res.Add(br.ReadInt32());  // TODO_HL: 5 ?Gyula: jó ez így hogy pl. 100szor felépítjük a Stream-et meg a BinaryReadert

            return res.ToArray();
        }

        static public float[] ToFloatArray(this byte[] buffer_in)
        {
            List<float> res = new List<float>();
            if (buffer_in.Count() % sizeof(float) != 0)
                throw new ArgumentException();

            using (MemoryStream ms = new MemoryStream(buffer_in, 0, buffer_in.Count(), false))
            using (BinaryReader br = new BinaryReader(ms))
                for (int i = 0; i < buffer_in.Count(); i += sizeof(float))
                    res.Add(br.ReadSingle());

            return res.ToArray();
        }

        static public List<byte> ToByteList(this List<UInt16> list)
        {
            List<byte> ret = new List<byte>(list.Count * sizeof(UInt16));
            foreach (UInt16 i in list)
            {
                ret.Add(i.Lo());
                ret.Add(i.Hi());
            }
            return ret;
        }

        static public byte[] ToByteArray(this IEnumerable<float> list_in)
        {
            List<byte> ret = new List<byte>();
            foreach (float i in list_in)
            {
                ret.AddRange(i.ToByteArray());
            }
            return ret.ToArray();
        }

        static public float ReadFloat(this byte[] buffer_in, int index_in)
        {
            float res = 0f;
            ReadStream(buffer_in, index_in, sizeof(float), delegate(BinaryReader reader_in) { res = reader_in.ReadSingle(); });
            return res;
        }

        static public void WriteFloat(this byte[] buffer_in, int index_in, float value_in)
        {
            WriteStream(buffer_in, index_in, sizeof(float), delegate(BinaryWriter writer_in) { writer_in.Write(value_in); });
        }

        static public byte[] ToByteArray(this float value_in)
        {
            byte[] buff = new byte[sizeof(float)];
            WriteFloat(buff, 0, value_in);
            return buff;
        }

        static public UInt32 ReadUInt32(this byte[] buffer_in, int index_in)
        {
            UInt32 res = 0;
            ReadStream(buffer_in, index_in, sizeof(UInt32), delegate(BinaryReader reader_in) { res = reader_in.ReadUInt32(); });
            return res;
        }

        static public Int32 ReadInt32(this byte[] buffer_in, int index_in)
        {
            Int32 res = 0;
            ReadStream(buffer_in, index_in, sizeof(Int32), delegate(BinaryReader reader_in) { res = reader_in.ReadInt32(); });
            return res;
        }

        static public void WriteUInt32(this byte[] buffer_in, int index_in, UInt32 value_in)
        {
            WriteStream(buffer_in, index_in, sizeof(UInt32), delegate(BinaryWriter writer_in) { writer_in.Write(value_in); });
        }

        static public byte[] ToByteArray(this UInt32 value_in)
        {
            byte[] buff = new byte[sizeof(UInt32)];
            WriteUInt32(buff, 0, value_in);
            return buff;
        }

        static public UInt16 ReadUInt16(this byte[] buffer_in, int index_in)
        {
            UInt16 res = 0;
            ReadStream(buffer_in, index_in, sizeof(UInt16), delegate(BinaryReader reader_in) { res = reader_in.ReadUInt16(); });
            return res;
        }

        static public void WriteUInt16(this byte[] buffer_in, int index_in, UInt16 value_in)
        {
            WriteStream(buffer_in, index_in, sizeof(UInt16), delegate(BinaryWriter writer_in) { writer_in.Write(value_in); });
        }

        static public byte[] ToByteArray(this UInt16 value_in)
        {
            byte[] buff = new byte[sizeof(UInt16)];
            WriteUInt16(buff, 0, value_in);
            return buff;
        }

        static public Int16 ReadInt16(this byte[] buffer_in, int index_in)
        {
            Int16 res = 0;
            ReadStream(buffer_in, index_in, sizeof(Int16), delegate(BinaryReader reader_in) { res = reader_in.ReadInt16(); });
            return res;
        }

        static public void WriteInt16(this byte[] buffer_in, int index_in, Int16 value_in)
        {
            WriteStream(buffer_in, index_in, sizeof(Int16), delegate(BinaryWriter writer_in) { writer_in.Write(value_in); });
        }

        static public byte[] ToByteArray(this Int16 value_in)
        {
            byte[] buff = new byte[sizeof(Int16)];
            WriteInt16(buff, 0, value_in);
            return buff;
        }

        //static public sbyte ReadSByte(this byte[] buffer_in, int index_in)
        //{
        //    return (sbyte)(buffer_in[index_in] <= sbyte.MaxValue ? buffer_in[index_in] : buffer_in[index_in] - byte.MaxValue - 1);
        //}

        //static public void WriteSByte(this byte[] buffer_in, int index_in, sbyte value_in)
        //{
        //    buffer_in[index_in] = (byte)(value_in >= 0 ? value_in : value_in + byte.MaxValue + 1);
        //}

        //static public byte[] ToByteArray(this sbyte value_in)
        //{
        //    return new byte[sizeof(sbyte)] { value_in };
        //}

        static public sbyte ToSByte(this byte value_in)
        {
            return (sbyte)(value_in <= sbyte.MaxValue ? value_in : value_in - byte.MaxValue - 1);
        }

        static public byte ToByte(this sbyte value_in)
        {
            return (byte)(value_in >= 0 ? value_in : value_in + byte.MaxValue + 1);
        }

        delegate void ReadDelegate(BinaryReader reader_in);

        private static void ReadStream(byte[] buffer_in, int index_in, int count_in, ReadDelegate delegate_in)
        {
            using (MemoryStream ms = new MemoryStream(buffer_in, index_in, count_in, false))
            using (BinaryReader br = new BinaryReader(ms))
                delegate_in(br);
        }

        delegate void WriteDelegate(BinaryWriter reader_in);

        private static void WriteStream(byte[] buffer_in, int index_in, int size_in, WriteDelegate delegate_in)
        {
            using (MemoryStream ms = new MemoryStream(buffer_in, index_in, size_in, true))
            using (BinaryWriter bw = new BinaryWriter(ms))
                delegate_in(bw);
        }
    }
}