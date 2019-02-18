using System;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Color-values for the NXT 2.0 Color sensor.</para>
    /// </summary>
    public enum Nxt2Color
    {
        /// <summary>
        /// <para>Black</para>
        /// </summary>
        Black = 1,

        /// <summary>
        /// <para>Blue</para>
        /// </summary>
        Blue = 2,

        /// <summary>
        /// <para>Green</para>
        /// </summary>
        Green = 3,

        /// <summary>
        /// <para>Yellow</para>
        /// </summary>
        Yellow = 4,

        /// <summary>
        /// <para>Red</para>
        /// </summary>
        Red = 5,

        /// <summary>
        /// <para>White</para>
        /// </summary>
        White = 6
    }

    /// <summary>
    /// <para>Sensor-modes for the NXT 2.0 color sensor.</para>
    /// </summary>
    public enum Nxt2ColorSensorMode
    {
        /// <summary>
        /// <para>Color detector mode with no light.</para>
        /// </summary>
        ColorDetector,

        /// <summary>
        /// <para>Light sensor mode with or without light.</para>
        /// </summary>
        LightSensor
    }

    /// <summary>
    /// <para>Class representing the NXT 2.0 color sensor.</para>
    /// </summary>
    /// <remarks>
    /// <para>The color sensor has two modes; it can act as a color detector and as a light sensor. In the last capacity it can be seen as a replacement for the NXT light sensor. The sensor can also act as a lamp.</para>
    /// <para>The NXT 2.0 color sensor should not be confused with the HiTechnic color sensor.</para>
    /// </remarks>
    /// <seealso cref="NxtLightSensor"/>
    /// <seealso cref="HiTechnic.HiTechnicColorSensor"/>
    public class Nxt2ColorSensor : NxtPassiveSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public Nxt2ColorSensor()
            : base(NxtSensorType.COLORFULL, NxtSensorMode.RAWMODE)
        {
            this.sensorMode = Nxt2ColorSensorMode.ColorDetector;

            SetColorRange(Nxt2Color.Black, Nxt2Color.White);
            TriggerIntensity = 100;
        }

        #region Sensor readings

        private Nxt2ColorSensorMode sensorMode;

        /// <summary>
        /// <para>Returns the current mode of the sensor; color detector or light sensor mode.</para>
        /// </summary>
        public Nxt2ColorSensorMode SensorMode
        {
            get { return sensorMode; }
        }

        /// <summary>
        /// <para>Switches the sensor into color detector mode.</para>
        /// </summary>
        public void SetColorDetectorMode()
        {
            this.sensorType = NxtSensorType.COLORFULL;

            this.sensorMode = Nxt2ColorSensorMode.ColorDetector;
            InitSensor();
        }

        /// <summary>
        /// <para>Switches the sensor into light sensor mode.</para>
        /// </summary>
        /// <param name="color">The floodlight color. The sensor will accept red, green and blue, or black and null for no floodlights.</param>
        public void SetLightSensorMode(Nxt2Color? color)
        {
            if (color != null && color != Nxt2Color.Red && color != Nxt2Color.Green && color != Nxt2Color.Blue && color != Nxt2Color.Black)
                throw new ArgumentException("The color must be red, green, blue, black or null.");

            switch (color)
            {
                case Nxt2Color.Red:
                    this.sensorType = NxtSensorType.COLORRED;
                    break;
                case Nxt2Color.Green:
                    this.sensorType = NxtSensorType.COLORGREEN;
                    break;
                case Nxt2Color.Blue:
                    this.sensorType = NxtSensorType.COLORBLUE;
                    break;
                case Nxt2Color.Black:
                case null:
                    this.sensorType = NxtSensorType.COLORNONE;
                    break;
            }

            this.sensorMode = Nxt2ColorSensorMode.LightSensor;
            InitSensor();
        }

        /// <summary>
        /// <para>Returns the color when the sensor is in color detector mode.</para>
        /// </summary>
        public Nxt2Color? Color
        {
            get
            {
                if (this.SensorMode == Nxt2ColorSensorMode.LightSensor) return null;

                if (pollData != null)
                    switch (pollData.Value.scaledValue)  // No real need to cast to a byte here.
                    {
                        case 1: return Nxt2Color.Black;
                        case 2: return Nxt2Color.Blue;
                        case 3: return Nxt2Color.Green;
                        case 4: return Nxt2Color.Yellow;
                        case 5: return Nxt2Color.Red;
                        case 6: return Nxt2Color.White;
                        default: return null;
                    }
                else
                    return null;
            }
        }

        /// <summary>
        /// <para>Returns the light intensity when the sensor is in light detector mode.</para>
        /// </summary>
        public byte? Intensity
        {
            get
            {
                if (this.SensorMode == Nxt2ColorSensorMode.ColorDetector) return null;

                if (pollData != null)
                    return (byte)pollData.Value.scaledValue;
                else
                    return null;
            }
        }

        #endregion

        #region NXT-G like events & NxtPollable overrides.

        private Nxt2Color minRange, maxRange;

        /// <summary>
        /// <para>Sets the minimum and the maximum of the color range.</para>
        /// </summary>
        /// <param name="minRange">Minimum</param>
        /// <param name="maxRange">Maximum</param>
        /// <seealso cref="OnInsideRange"/>
        /// <seealso cref="OnOutsideRange"/>
        public void SetColorRange(Nxt2Color minRange, Nxt2Color maxRange)
        {
            this.minRange = minRange;
            this.maxRange = maxRange;

            if (minRange >= maxRange)
            {
                throw new ArgumentException("The minimum-value must be less than the maximum-value.");
            }
        }

        /// <summary>
        /// <para>This event is fired whenever the detected color passes from outside of the color-range to inside.</para>
        /// </summary>
        /// <remarks>
        /// <para>The detector must be in color detector mode for the event to fire.</para>
        /// </remarks>
        /// <seealso cref="SetColorRange"/>
        /// <seealso cref="OnOutsideRange"/>
        public event NxtSensorEvent OnInsideRange;

        /// <summary>
        /// <para>This event is fired whenever the detected color passes from inside of the color-range to outside.</para>
        /// </summary>
        /// <remarks>
        /// <para>The detector must be in color detector mode for the event to fire.</para>
        /// </remarks>
        /// <seealso cref="SetColorRange"/>
        /// <seealso cref="OnInsideRange"/>
        public event NxtSensorEvent OnOutsideRange;

        private byte triggerIntensity;

        /// <summary>
        /// <para>Trigger intensity.</para>
        /// </summary>
        /// <seealso cref="OnAboveIntensity"/>
        public byte TriggerIntensity
        {
            get { return triggerIntensity; }
            set
            {
                triggerIntensity = value;
                if (triggerIntensity > 100) triggerIntensity = 100;
            }
        }

        /// <summary>
        /// <para>This event is fired whenever the measured light intensity passes above the trigger-intensity.</para>
        /// </summary>
        /// <remarks>
        /// <para>The detector must be in light sensor mode for the event to fire.</para>
        /// </remarks>
        /// <seealso cref="TriggerIntensity"/>
        public event NxtSensorEvent OnAboveIntensity;

        private object pollDataLock = new object();

        /// <summary>
        /// <para>Polls the sensor, and fires the NXT-G like events if appropriate.</para>
        /// </summary>
        /// <seealso cref="SetColorRange"/>
        /// <seealso cref="OnInsideRange"/>
        /// <seealso cref="OnOutsideRange"/>
        public override void Poll()
        {
            if (Brick.IsConnected)
            {
                if (SensorMode == Nxt2ColorSensorMode.ColorDetector)
                {
                    Nxt2Color? oldColor, newColor;
                    lock (pollDataLock)
                    {
                        oldColor = Color;
                        base.Poll();
                        newColor = Color;
                    }

                    if (oldColor != null & newColor != null)
                    {
                        // Is the new color inside the color-range?
                        if (this.minRange <= newColor && newColor <= this.maxRange)
                        {
                            // Was the old color outside the color-range?
                            if (oldColor < this.minRange || this.maxRange < oldColor)
                            {
                                if (OnInsideRange != null) OnInsideRange(this);
                            }
                        }

                        // Is the new color outside the color-range?
                        if (newColor < this.minRange || this.maxRange < newColor)
                        {
                            // Was the old color inside the color-range?
                            if (this.minRange <= oldColor && oldColor <= this.maxRange)
                            {
                                if (OnOutsideRange != null) OnOutsideRange(this);
                            }
                        }
                    }
                }
                else if (SensorMode == Nxt2ColorSensorMode.LightSensor)
                {
                    byte? oldIntensity, newIntensity;
                    lock (pollDataLock)
                    {
                        oldIntensity = Intensity;
                        base.Poll();
                        newIntensity = Intensity;
                    }

                    if (oldIntensity != null && newIntensity != null)
                    {
                        // Have the intensity passed over the trigger-intensity?
                        if (oldIntensity <= TriggerIntensity && TriggerIntensity < newIntensity)
                        {
                            if (OnAboveIntensity != null) OnAboveIntensity(this);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
