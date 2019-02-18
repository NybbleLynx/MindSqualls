namespace NKH.MindSqualls.HiTechnic
{
    /// <summary>
    /// <para>Class representing the Acceleration / Tilt Sensor sensor from HiTechnic:
    /// <a href="http://www.hitechnic.com" target="_blank">http://www.hitechnic.com</a></para>
    /// </summary>
    /// <remarks>
    /// <para>A special thanks to the people at HiTechnic for providing me with the necessary technical information.</para>
    /// <para>Also a special thanks to Ronny H. for providing me with qualified implementation details on the acceleration sensor before I had purchased my own copy.</para>
    /// </remarks>
    public class HiTechnicAccelerationSensor : NxtDigitalSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public HiTechnicAccelerationSensor()
            : base()
        { }

        #region Sensor readings.

        /// <summary>
        /// <para>Acceleration coordinate of the X-axis.</para>
        /// </summary>
        /// <returns>The acceleration coordinate</returns>
        public double? XAxisAcceleration()
        {
            return XyzAxisAcceleration(XAxisUpper8Bits());
        }

        /// <summary>
        /// <para>Acceleration coordinate of the Y-axis.</para>
        /// </summary>
        /// <returns>The acceleration coordinate</returns>
        public double? YAxisAcceleration()
        {
            return XyzAxisAcceleration(YAxisUpper8Bits());
        }

        /// <summary>
        /// <para>Acceleration coordinate of the Z-axis.</para>
        /// </summary>
        /// <returns>The acceleration coordinate</returns>
        public double? ZAxisAcceleration()
        {
            return XyzAxisAcceleration(ZAxisUpper8Bits());
        }

        /// <summary>
        /// <para>Change this value, to finetune the value of the acceleration sensor.</para>
        /// <para>The current value, = 9.82/206.3, is the best fit for my own sensor.</para>
        /// <remarks>
        /// <para>The component used in the sensor is only guaranteed to be accurate to +/- 10%.</para>
        /// </remarks>
        /// </summary>
        public double scalíng = 9.82 / 200;

        private double? XyzAxisAcceleration(sbyte? xyz)
        {
            if (!xyz.HasValue) return null;

            int tmpXyz = xyz.Value;

            // Reverse the sign.
            tmpXyz *= -1;

            // Shift 2 bits up.
            tmpXyz = tmpXyz << 2;

            return scalíng * tmpXyz;
        }

        #endregion

        #region I2C protocol.

        /// <summary>
        /// <para>X axis upper 8 bits</para>
        /// </summary>
        /// <returns>The X-component of the acceleration vector</returns>
        /// <seealso cref="YAxisUpper8Bits"/>
        /// <seealso cref="ZAxisUpper8Bits"/>
        /// <seealso cref="XAxisLower2Bits"/>
        public sbyte? XAxisUpper8Bits()
        {
            byte? xAxisUpper8 = ReadByteFromAddress(0x42);
            if (xAxisUpper8.HasValue)
                return (sbyte)xAxisUpper8.Value;
            else
                return null;
        }

        /// <summary>
        /// <para>Y axis upper 8 bits</para>
        /// </summary>
        /// <returns>The Y-component of the acceleration vector</returns>
        /// <seealso cref="XAxisUpper8Bits"/>
        /// <seealso cref="ZAxisUpper8Bits"/>
        /// <seealso cref="YAxisLower2Bits"/>
        public sbyte? YAxisUpper8Bits()
        {
            byte? yAxisUpper8 = ReadByteFromAddress(0x43);
            if (yAxisUpper8.HasValue)
                return (sbyte)yAxisUpper8.Value;
            else
                return null;
        }

        /// <summary>
        /// <para>Z axis upper 8 bits</para>
        /// </summary>
        /// <returns>The Z-component of the acceleration vector</returns>
        /// <seealso cref="XAxisUpper8Bits"/>
        /// <seealso cref="YAxisUpper8Bits"/>
        /// <seealso cref="ZAxisLower2Bits"/>
        public sbyte? ZAxisUpper8Bits()
        {
            byte? zAxisUpper8 = ReadByteFromAddress(0x44);
            if (zAxisUpper8.HasValue)
                return (sbyte)zAxisUpper8.Value;
            else
                return null;
        }

        /// <summary>
        /// <para>X axis lower 2 bits</para>
        /// </summary>
        /// <remarks>
        /// <para>Should only be used if the full precision is needed.</para>
        /// </remarks>
        /// <returns>The lower 2 bits of the X-component of the acceleration vector</returns>
        /// <seealso cref="XAxisUpper8Bits"/>
        public byte? XAxisLower2Bits()
        {
            return ReadByteFromAddress(0x45);
        }

        /// <summary>
        /// <para>Y axis lower 2 bits</para>
        /// </summary>
        /// <remarks>
        /// <para>Should only be used if the full precision is needed.</para>
        /// </remarks>
        /// <returns>The lower 2 bits of the Y-component of the acceleration vector</returns>
        /// <seealso cref="YAxisUpper8Bits"/>
        public byte? YAxisLower2Bits()
        {
            return ReadByteFromAddress(0x46);
        }

        /// <summary>
        /// <para>Z axis lower 2 bits</para>
        /// </summary>
        /// <remarks>
        /// <para>Should only be used if the full precision is needed.</para>
        /// </remarks>
        /// <returns>The lower 2 bits of the Z-component of the acceleration vector</returns>
        /// <seealso cref="ZAxisUpper8Bits"/>
        public byte? ZAxisLower2Bits()
        {
            return ReadByteFromAddress(0x47);
        }

        #endregion

        #region NXT-G like events & NxtPollable overrides.
        #endregion
    }
}
