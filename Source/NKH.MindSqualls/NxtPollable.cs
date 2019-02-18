using System.Threading;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Delegate used when a motor or sensor is polled.</para>
    /// </summary>
    /// <param name="polledItem">The motor/sensor.</param>
    public delegate void Polled(NxtPollable polledItem);

    /// <summary>
    /// <para>Abstract class used as superclass for the NxtMotor and NxtSensor classes.</para>
    /// </summary>
    /// <remarks>
    /// <para>This class has been modified from using System.Timers.Timer (in versions 1.0 and 1.1) to using System.Threading.Timer in order to be able to function with the compact framework (i.e. to run on a PDA). A special thanks to Giorgio T. for his suggestion.</para>
    /// </remarks>
    public abstract class NxtPollable
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtPollable()
        {
            autoPollTimer = new Timer(autoPollTimer_Callback, null, Timeout.Infinite, Timeout.Infinite);
        }

        #region Brick.

        /// <summary>
        /// <para>The NXT brick that the sensor/motor is attached to.</para>
        /// </summary>
        private NxtBrick brick = null;

        /// <summary>
        /// <para>The NXT brick that the sensor/motor is attached to.</para>
        /// </summary>
        internal protected NxtBrick Brick
        {
            get
            {
                if (brick == null)
                {
                    if (this is NxtSensor)
                        throw new NxtConnectionException("The sensor has not been connected to a NXT brick.");
                    else if (this is NxtMotor)
                        throw new NxtConnectionException("The motor has not been connected to a NXT brick.");
                    else
                        throw new NxtConnectionException("Has not been connected to a NXT brick.");  // This should never happen.
                }

                return brick;
            }
            set { brick = value; }
        }

        #endregion

        #region Auto-polling.

        private int pollInterval = 0;

        /// <summary>
        /// <para>Indicates if the sensor, or motor, is auto-polled or not.</para>
        /// </summary>
        /// <value>
        /// <para>A positive value means that the sensor is auto-pooled, and with the indicated interval i milliseconds.</para>
        /// <para>Zero, or a negative value, means that the sensor is not auto-polled.</para>
        /// </value>
        public int PollInterval
        {
            get { return (autoPollTimer != null) ? pollInterval : 0; }
            set
            {
                pollInterval = value;
                if (pollInterval > 0)
                {
                    if (brick != null && brick.IsConnected)
                        EnableAutoPoll();
                    else
                        DisableAutoPoll();
                }
                else
                {
                    pollInterval = 0;
                    DisableAutoPoll();
                }
            }
        }

        internal void EnableAutoPoll()
        {
            if (pollInterval > 0)
            {
                autoPollTimer.Change(pollInterval, pollInterval);
            }
        }

        internal void DisableAutoPoll()
        {
            autoPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// <para>The auto-poll timer.</para>
        /// </summary>
        private Timer autoPollTimer = null;

        /// <summary>
        /// <para>This event is triggered by the auto-poll timer.</para>
        /// </summary>
        private void autoPollTimer_Callback(object state)
        {
            try { Poll(); }
            catch (NxtConnectionException) { }
        }

        #endregion

        #region Polling.

        /// <summary>
        /// <para>This event is fired when the sensor, or motor, is polled.</para>
        /// </summary>
        public event Polled OnPolled;

        /// <summary>
        /// <para>Poll the sensor/motor.</para>
        /// </summary>
        public virtual void Poll()
        {
            if (OnPolled != null) OnPolled(this);
        }

        #endregion
    }
}
