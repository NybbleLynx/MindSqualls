using System;

namespace NKH.MindSqualls.HiTechnic
{
    /// <summary>
    /// <para>Class representing the Color sensor from HiTechnic:
    /// <a href="http://www.hitechnic.com" target="_blank">http://www.hitechnic.com</a></para>
    /// </summary>
    /// <remarks>
    /// <para>The HiTechnic color sensor should not be confused with the NXT 2.0 color sensor.</para>
    /// <para>A special thanks to the people at HiTechnic for providing me with the necessary technical information.</para>
    /// </remarks>
    /// <seealso cref="Nxt2ColorSensor"/>
    public class HiTechnicColorSensor : NxtDigitalSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public HiTechnicColorSensor()
            : base()
        { }

        #region Sensor readings
        #endregion

        #region I2C protocol

        /// <summary>
        /// <para>Returns a single number color estimate.</para>
        /// </summary>
        public byte? ColorNumber
        {
            get { return ReadByteFromAddress(0x42); }
        }

        /// <summary>
        /// <para>Returns the current detection level for the color red.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public byte? RedReading()
        {
            return ReadByteFromAddress(0x43);
        }

        /// <summary>
        /// <para>Returns the current detection level for the color green.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public byte? GreenReading()
        {
            return ReadByteFromAddress(0x44);
        }

        /// <summary>
        /// <para>Returns the current detection level for the color blue.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public byte? BlueReading()
        {
            return ReadByteFromAddress(0x45);
        }

        /// <summary>
        /// <para>Returns the current sensor analog signal level for the color red.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public UInt16? RawRedSensorReading()
        {
            return ReadWordFromAdress(0x46);
        }

        /// <summary>
        /// <para>Returns the current sensor analog signal level for the color green.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public UInt16? RawGreenSensorReading()
        {
            return ReadWordFromAdress(0x48);
        }

        /// <summary>
        /// <para>Returns the current sensor analog signal level for the color blue.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public UInt16? RawBlueSensorReading()
        {
            return ReadWordFromAdress(0x4A);
        }

        /// <summary>
        /// <para>Returns a single 6 bit number color index. Bits 5 and 4 encode the red signal level, bits 3 and 2 encode the green signal level and bits 1 and 0 encode the blue signal levels.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public byte? ColorIndexNumber()
        {
            return ReadByteFromAddress(0x4C);
        }

        /// <summary>
        /// <para>Return the current relative level for the red color component.</para>
        /// </summary>
        /// <remarks>The normalization sets the highest value of the three red, green and blue reading to 255 and adjusts the other two proportionately.</remarks>
        /// <returns>... TBD ...</returns>
        public byte? NormalizedRedReading()
        {
            return ReadByteFromAddress(0x4D);
        }

        /// <summary>
        /// <para>Return the current relative level for the green color component.</para>
        /// </summary>
        /// <remarks>The normalization sets the highest value of the three red, green and blue reading to 255 and adjusts the other two proportionately.</remarks>
        /// <returns>... TBD ...</returns>
        public byte? NormalizedGreenReading()
        {
            return ReadByteFromAddress(0x4E);
        }

        /// <summary>
        /// <para>Return the current relative level for the blue color component.</para>
        /// </summary>
        /// <remarks>The normalization sets the highest value of the three red, green and blue reading to 255 and adjusts the other two proportionately.</remarks>
        /// <returns>... TBD ...</returns>
        public byte? NormalizedBlueReading()
        {
            return ReadByteFromAddress(0x4F);
        }

        #endregion

        #region NXT-G like events & NxtPollable overrides
        #endregion
    }
}
