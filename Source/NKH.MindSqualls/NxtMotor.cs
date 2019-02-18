using System;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Delegate used for the NXT-G like events implemented in the motor.</para>
    /// </summary>
    /// <param name="motor">The motor</param>
    public delegate void NxtMotorEvent(NxtMotor motor);

    /// <summary>
    /// <para>Class representing a motor.</para>
    /// </summary>
    /// <seealso cref="NxtMotorSync"/>
    public class NxtMotor : NxtPollable
    {
        internal readonly bool reversed = false;

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtMotor() : this(false) { }

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="reverse">Indicates if the motor-direction should be reversed.</param>
        public NxtMotor(bool reverse)
        {
            this.reversed = reverse;
        }

        /// <summary>
        /// <para>The port on the NXT brick that the motor is attached to.</para>
        /// </summary>
        private NxtMotorPort motorPort;

        /// <summary>
        /// <para>The port on the NXT brick that the motor is attached to.</para>
        /// </summary>
        internal protected NxtMotorPort Port
        {
            get { return motorPort; }
            set { motorPort = value; }
        }

        #region Run the motor.

        /// <summary>
        /// <para>Run the motor.</para>
        /// </summary>
        /// <param name="power">The power for the motor</param>
        /// <param name="tachoLimit">The duration in degrees, 0 = unlimited</param>
        public virtual void Run(sbyte power, UInt32 tachoLimit)
        {
            if (reversed) power *= -1;

            if (Brick != null)
            {
                Brick.CommLink.SetOutputState(
                    Port,
                    power,
                    NxtMotorMode.MOTORON | NxtMotorMode.REGULATED,
                    NxtMotorRegulationMode.REGULATION_MODE_MOTOR_SPEED,
                    0,
                    NxtMotorRunState.MOTOR_RUN_STATE_RUNNING,
                    tachoLimit);
            }
        }

        #endregion

        #region Various ways to halt the motor.

        /// <summary>
        /// <para>Coast the motor.</para>
        /// </summary>
        public void Coast()
        {
            if (Brick != null)
            {
                Brick.CommLink.SetOutputState(
                    Port,
                    0,
                    NxtMotorMode.MOTORON | NxtMotorMode.REGULATED,
                    NxtMotorRegulationMode.REGULATION_MODE_IDLE,
                    0,
                    NxtMotorRunState.MOTOR_RUN_STATE_RUNNING,
                    0);
            }
        }

        /// <summary>
        /// <para>Brake the motor.</para>
        /// </summary>
        public void Brake()
        {
            if (Brick != null)
            {
                Brick.CommLink.SetOutputState(
                    Port,
                    0,
                    NxtMotorMode.MOTORON | NxtMotorMode.REGULATED | NxtMotorMode.BRAKE,
                    NxtMotorRegulationMode.REGULATION_MODE_IDLE,
                    0,
                    NxtMotorRunState.MOTOR_RUN_STATE_RUNNING,
                    0
                    );
            }
        }

        /// <summary>
        /// <para>Sets the motor into the idle state.</para>
        /// </summary>
        public void Idle()
        {
            Brick.CommLink.SetOutputState(
                Port,
                0,
                0,
                NxtMotorRegulationMode.REGULATION_MODE_IDLE,
                0,
                NxtMotorRunState.MOTOR_RUN_STATE_IDLE,
                0);
        }

        #endregion

        #region Tachometer.

        /// <summary>
        /// <para>Returns the current tacho count of the motor (in degrees).</para>
        /// </summary>
        public Int32? TachoCount
        {
            get
            {
                if (pollData.HasValue)
                    return pollData.Value.tachoCount;
                else
                    return null;
            }
        }

        /// <summary>
        /// <para>Resets the motors tachometer.</para>
        /// </summary>
        /// <param name="relative">True if the reset is relative to the last movement, false if the absolute position</param>
        /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.ResetMotorPosition"/>
        public void ResetMotorPosition(bool relative)
        {
            Brick.CommLink.ResetMotorPosition(Port, relative);
        }

        #endregion

        #region NXT-G like events & NxtPollable overrides.
        // Actually the NXG-G programming-language don't have anything equivalent, but this part is inspired
        // from the NxtUltrasonicSensor-class.

        private byte triggerTachoCount = byte.MaxValue;

        /// <summary>
        /// <para>Trigger value for the tachocount.</para>
        /// </summary>
        /// <seealso cref="OnBelowTachoCount"/>
        /// <seealso cref="OnAboveTachoCount"/>
        public byte TriggerTachoCount
        {
            get { return triggerTachoCount; }
            set { triggerTachoCount = value; }
        }

        /// <summary>
        /// <para>This event is fired when the tachocount of the motor goes below the trigger-value.</para>
        /// </summary>
        /// <seealso cref="TriggerTachoCount"/>
        /// <seealso cref="OnAboveTachoCount"/>
        /// <seealso cref="Poll"/>
        public event NxtMotorEvent OnBelowTachoCount;

        /// <summary>
        /// <para>This event is fired when the tachocount of the motor goes above the trigger-value.</para>
        /// </summary>
        /// <seealso cref="TriggerTachoCount"/>
        /// <seealso cref="OnBelowTachoCount"/>
        /// <seealso cref="Poll"/>
        public event NxtMotorEvent OnAboveTachoCount;

        /// <summary>
        /// <para>The data from the previous poll of the motor.</para>
        /// </summary>
        protected NxtGetOutputStateReply? pollData;

        private object pollLock = new object();

        /// <summary>
        /// <para>Poll the motor.</para>
        /// </summary>
        public override void Poll()
        {
            if (Brick.IsConnected)
            {
                int? oldTachoCount, newTachoCount;
                lock (pollLock)
                {
                    oldTachoCount = TachoCount;
                    pollData = Brick.CommLink.GetOutputState(Port);
                    base.Poll();
                    newTachoCount = TachoCount;
                }

                if (oldTachoCount != null && newTachoCount != null)
                {
                    if (oldTachoCount <= TriggerTachoCount && TriggerTachoCount < newTachoCount)
                    {
                        if (OnAboveTachoCount != null) OnAboveTachoCount(this);
                    }

                    if (newTachoCount < TriggerTachoCount && TriggerTachoCount <= oldTachoCount)
                    {
                        if (OnBelowTachoCount != null) OnBelowTachoCount(this);
                    }
                }
            }
        }

        #endregion
    }
}
