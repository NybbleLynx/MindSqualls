using System;

namespace NKH.MindSqualls
{
    class Util
    {
        #region Int16

        internal static Int16 GetInt16(byte[] byteArr, byte index)
        {
            Int16 result = 0;
            for (sbyte offset = 1; offset >= 0; offset--)
            {
                result <<= 8;
                if (offset == 1)
                    result += (sbyte)byteArr[index + offset];
                else
                    result += (byte)byteArr[index + offset];
            }

            return result;
        }

        #endregion

        #region UInt16

        internal static UInt16 GetUInt16(byte[] byteArr, byte index)
        {
            UInt16 result = 0;
            for (sbyte offset = 1; offset >= 0; offset--)
            {
                result <<= 8;
                result += (byte)byteArr[index + offset];
            }

            return result;
        }

        internal static void SetUInt16(UInt16 number, byte[] byteArr, byte index)
        {
            for (byte offset = 0; offset <= 1; offset++)
            {
                byteArr[index + offset] = (byte)(number % 0x100);
                number >>= 8;
            };
        }

        #endregion

        #region Int32

        internal static Int32 GetInt32(byte[] byteArr, byte index)
        {
            Int32 result = 0;
            for (sbyte offset = 3; offset >= 0; offset--)
            {
                result <<= 8;
                if (offset == 3)
                    result += (sbyte)byteArr[index + offset];
                else
                    result += (byte)byteArr[index + offset];
            }

            return result;
        }

        #endregion

        #region UInt32

        internal static UInt32 GetUInt32(byte[] byteArr, byte index)
        {
            UInt32 result = 0;
            for (sbyte offset = 3; offset >= 0; offset--)
            {
                result <<= 8;
                result += (byte)byteArr[index + offset];
            }

            return result;
        }

        internal static void SetUInt32(UInt32 number, byte[] byteArr, byte index)
        {
            for (byte offset = 0; offset <= 3; offset++)
            {
                byteArr[index + offset] = (byte)(number % 0x100);
                number >>= 8;
            };
        }

        #endregion
    }
}
