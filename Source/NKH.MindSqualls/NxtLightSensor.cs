namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Class representing the Light sensor.</para>
    /// </summary>
    public class NxtLightSensor : NxtPassiveSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtLightSensor()
            : base(NxtSensorType.LIGHT_INACTIVE, NxtSensorMode.PCTFULLSCALEMODE)
        {
            ThresholdIntensity = 50;
        }

        #region Sensor readings.

        /// <summary>
        /// <para>Indicates if the sensor should supply its own light.</para>
        /// </summary>
        public bool GenerateLight
        {
            get { return (sensorType == NxtSensorType.LIGHT_ACTIVE); }
            set
            {
                sensorType = (value) ? NxtSensorType.LIGHT_ACTIVE : NxtSensorType.LIGHT_INACTIVE;
                InitSensor();
            }
        }

        /// <summary>
        /// <para>The intensity of the measured light.</para>
        /// </summary>
        public byte? Intensity
        {
            get
            {
                if (pollData != null)
                    return (byte)pollData.Value.scaledValue;
                else
                    return null;
            }
        }

        #endregion

        #region NXT-G like events & NxtPollable overrides.

        private byte thresholdIntensity;

        /// <summary>
        /// <para>Threshold intensity.</para>
        /// </summary>
        /// <seealso cref="OnAboveThresholdIntensity"/>
        /// <seealso cref="OnBelowThresholdIntensity"/>
        public byte ThresholdIntensity
        {
            get { return thresholdIntensity; }
            set
            {
                thresholdIntensity = value;
                if (thresholdIntensity > 100) thresholdIntensity = 100;
            }
        }

        /// <summary>
        /// <para>This event is fired whenever the measured light intensity passes above the threshold-intensity.</para>
        /// </summary>
        /// <seealso cref="ThresholdIntensity"/>
        /// <seealso cref="OnBelowThresholdIntensity"/>
        public event NxtSensorEvent OnAboveThresholdIntensity;

        /// <summary>
        /// <para>This event is fired whenever the measured light intensity passes below the threshold-intensity.</para>
        /// </summary>
        /// <seealso cref="ThresholdIntensity"/>
        /// <seealso cref="OnAboveThresholdIntensity"/>
        public event NxtSensorEvent OnBelowThresholdIntensity;

        private object pollDataLock = new object();

        /// <summary>
        /// <para>Polls the sensor, and fires the NXT-G like events if appropriate.</para>
        /// </summary>
        /// <seealso cref="ThresholdIntensity"/>
        /// <seealso cref="OnAboveThresholdIntensity"/>
        /// <seealso cref="OnBelowThresholdIntensity"/>
        public override void Poll()
        {
            if (Brick.IsConnected)
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
                    // Have the intensity passed over the threshold-intensity?
                    if (oldIntensity <= ThresholdIntensity && ThresholdIntensity < newIntensity)
                    {
                        if (OnAboveThresholdIntensity != null) OnAboveThresholdIntensity(this);
                    }

                    // Have the intensity passed below the threshold-intensity?
                    if (newIntensity < ThresholdIntensity && ThresholdIntensity <= oldIntensity)
                    {
                        if (OnBelowThresholdIntensity != null) OnBelowThresholdIntensity(this);
                    }
                }
            }
        }

        #endregion
    }
}
