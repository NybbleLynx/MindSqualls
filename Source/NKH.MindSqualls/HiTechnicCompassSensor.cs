using System;

// TODO: Add support for calibration.

namespace NKH.MindSqualls.HiTechnic
{
    /// <summary>
    /// <para>Class representing the Compass sensor from HiTechnic:
    /// <a href="http://www.hitechnic.com" target="_blank">http://www.hitechnic.com</a></para>
    /// </summary>
    /// <remarks>
    /// <para>A special thanks to the people at HiTechnic for providing me with the necessary technical information.</para>
    /// </remarks>
    public class HiTechnicCompassSensor : NxtDigitalSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public HiTechnicCompassSensor()
            : base()
        { }

        #region Sensor readings
        #endregion

        #region I2C protocol

        /// <summary>
        /// <para>Returns the current value of the mode control of the sensor.</para>
        /// </summary>
        /// <returns>The value of the mode control</returns>
        public byte? ModeControl()
        {
            return ReadByteFromAddress(0x41);
        }

        /// <summary>
        /// <para>Returns the current heading to a 2-degree accuracy.</para>
        /// </summary>
        /// <returns>The heading</returns>
        public byte? TwoDegreeHeading()
        {
            byte? twoDegreeHeading = ReadByteFromAddress(0x42);

            if (twoDegreeHeading != null)
                return (twoDegreeHeading != 0xFF) ? twoDegreeHeading : (byte)0;
            else
                return null;
        }

        /// <summary>
        /// <para>Returns an 1-degree adder to use with the TwoDegreeHeading()-method.</para>
        /// </summary>
        /// <returns>The adder</returns>
        public byte? OneDegreeAdder()
        {
            return ReadByteFromAddress(0x43);
        }

        /// <summary>
        /// <para>Heading</para>
        /// </summary>
        /// <returns></returns>
        public UInt16? Heading
        {
            // WAS (pre v2.2): get { return ReadWordFromAdress(0x44); }
            get
            {
                if (pollData == null) return null;
                return (UInt16)(2 * pollData.Value);
            }
        }

        #endregion

        #region NXT-G like events & NxtPollable overrides

        private UInt16 rangeA = 0;

        /// <summary>
        /// <para>Lower boundary of the Range-interval.</para>
        /// </summary>
        /// <seealso cref="RangeB"/>
        /// <seealso cref="OnInsideRange"/>
        /// <seealso cref="OnOutsideRange"/>
        /// <seealso cref="Poll"/>
        public UInt16 RangeA
        {
            get { return rangeA; }
            set
            {
                // Ensure that 0 <= RangeA <= RangeB < 360
                if (value > rangeB)
                {
                    rangeA = rangeB;
                    rangeB = value;

                    if (rangeB >= 360) rangeB = 359;
                }
                else
                    rangeA = value;
            }
        }

        private UInt16 rangeB = 359;

        /// <summary>
        /// <para>Upper boundary of the Range-interval.</para>
        /// </summary>
        /// <seealso cref="RangeA"/>
        /// <seealso cref="OnInsideRange"/>
        /// <seealso cref="OnOutsideRange"/>
        /// <seealso cref="Poll"/>
        public UInt16 RangeB
        {
            get { return rangeB; }
            set
            {
                // Ensure that 0 <= RangeA <= RangeB < 360
                if (value < rangeA)
                {
                    rangeB = rangeA;
                    rangeA = value;
                }
                else
                {
                    rangeB = value;
                    if (rangeB >= 360) rangeB = 359;
                }
            }
        }

        /// <summary>
        /// <para>This event is fired when the Heading passes inside the Range-interval.</para>
        /// </summary>
        /// <seealso cref="RangeA"/>
        /// <seealso cref="RangeB"/>
        /// <seealso cref="OnOutsideRange"/>
        /// <seealso cref="Poll"/>
        public event NxtSensorEvent OnInsideRange;

        /// <summary>
        /// <para>This event is fired when the Heading passes outside the Range-interval.</para>
        /// </summary>
        /// <seealso cref="RangeA"/>
        /// <seealso cref="RangeB"/>
        /// <seealso cref="OnInsideRange"/>
        /// <seealso cref="Poll"/>
        public event NxtSensorEvent OnOutsideRange;

        /// <summary>
        /// <para>The data from the previous poll of the sensor.</para>
        /// </summary>
        /// <seealso cref="Poll"/>
        protected byte? pollData;

        private object pollDataLock = new object();

        /// <summary>
        /// <para>Polls the sensor, and fires the NXT-G like events if appropriate.</para>
        /// </summary>
        /// <seealso cref="RangeA"/>
        /// <seealso cref="RangeB"/>
        /// <seealso cref="OnInsideRange"/>
        /// <seealso cref="OnOutsideRange"/>
        public override void Poll()
        {
            if (Brick.IsConnected)
            {
                UInt16? oldHeading, newHeading;
                lock (pollDataLock)
                {
                    oldHeading = Heading;
                    pollData = TwoDegreeHeading();
                    base.Poll();
                    newHeading = Heading;
                }

                if (OnInsideRange != null &&
                    (oldHeading < RangeA || RangeB < oldHeading) &&
                    (RangeA <= newHeading && newHeading <= RangeB))
                    OnInsideRange(this);
                else if (OnOutsideRange != null &&
                    (RangeA <= oldHeading && oldHeading <= RangeB) &&
                    (newHeading < RangeA || RangeB < newHeading))
                    OnOutsideRange(this);
            }
        }

        #endregion
    }
}
