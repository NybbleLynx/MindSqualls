namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Class representing the Sound sensor.</para>
    /// </summary>
    public class NxtSoundSensor : NxtPassiveSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtSoundSensor()
            : base(NxtSensorType.SOUND_DB, NxtSensorMode.PCTFULLSCALEMODE)
        {
            ThresholdIntensity = 50;
        }

        #region Sensor readings.

        /// <summary>
        /// <para>Indicating if the sensor should return results as dB or as dBA (adjusted for the human ear).</para>
        /// </summary>
        public bool dBA
        {
            get { return (sensorType == NxtSensorType.SOUND_DBA); }
            set
            {
                sensorType = (value) ? NxtSensorType.SOUND_DBA : NxtSensorType.SOUND_DB;
                InitSensor();
            }
        }

        /// <summary>
        /// <para>The measured sound intensity.</para>
        /// </summary>
        /// <seealso cref="dBA"/>
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
        /// <para>Threshold sound intensity.</para>
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
        /// <para>This event is fired whenever the measured sound intensity passes above the threshold-level.</para>
        /// </summary>
        /// <seealso cref="ThresholdIntensity"/>
        /// <seealso cref="OnBelowThresholdIntensity"/>
        /// <seealso cref="Poll"/>
        public event NxtSensorEvent OnAboveThresholdIntensity;

        /// <summary>
        /// <para>This event is fired whenever the measured sound intensity passes below the threshold-level.</para>
        /// </summary>
        /// <seealso cref="ThresholdIntensity"/>
        /// <seealso cref="OnAboveThresholdIntensity"/>
        /// <seealso cref="Poll"/>
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
