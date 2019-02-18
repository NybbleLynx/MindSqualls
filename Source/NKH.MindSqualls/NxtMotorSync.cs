using System;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Class representing a synchronized pair of motors.</para>
    /// </summary>
    /// <seealso cref="NxtMotor"/>
    public class NxtMotorSync
    {
        /// <summary>
        /// <para>The first motor of the synchronized pair.</para>
        /// </summary>
        protected NxtMotor motorX;

        /// <summary>
        /// <para>The second motor of the synchronized pair.</para>
        /// </summary>
        protected NxtMotor motorY;

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="motorX">The 1. motor of the synchronized pair</param>
        /// <param name="motorY">The 2. motor of the synchronized pair</param>
        public NxtMotorSync(NxtMotor motorX, NxtMotor motorY)
        {
            if (motorX == null || motorY == null)
                throw new ArgumentException("One of the motors was null.");

            this.motorX = motorX;
            this.motorY = motorY;
        }

        #region Run the syncronized motors.

        /// <summary>
        /// <para>Run the motors (in sync).</para>
        /// </summary>
        /// <param name="power">The poser</param>
        /// <param name="tachoLimit">The tacho limit in degrees, 0 means unlimited</param>
        /// <param name="turnRatio">The turn ratio</param>
        public virtual void Run(sbyte power, UInt16 tachoLimit, sbyte turnRatio)
        {
            sbyte powerX = power;
            if (motorX.reversed) powerX *= -1;

            sbyte powerY = power;
            if (motorY.reversed) powerY *= -1;

            // Reset tachometers.
            ResetMotorPosition(false);

            motorX.Brick.CommLink.SetOutputState(
                motorX.Port,
                powerX,
                NxtMotorMode.BRAKE | NxtMotorMode.MOTORON | NxtMotorMode.REGULATED,
                NxtMotorRegulationMode.REGULATION_MODE_MOTOR_SYNC,
                turnRatio,
                NxtMotorRunState.MOTOR_RUN_STATE_RUNNING,
                tachoLimit
                );

            motorY.Brick.CommLink.SetOutputState(
                motorY.Port,
                powerY,
                NxtMotorMode.BRAKE | NxtMotorMode.MOTORON | NxtMotorMode.REGULATED,
                NxtMotorRegulationMode.REGULATION_MODE_MOTOR_SYNC,
                turnRatio,
                NxtMotorRunState.MOTOR_RUN_STATE_RUNNING,
                tachoLimit
                );
        }

        #endregion

        #region Various ways to halt the motors.

        /// <summary>
        /// <para>Calls ResetMotorPosition() on both motors.</para>
        /// </summary>
        /// <param name="relative">True if the reset is relative to the last movement, false if the absolute position</param>
        /// <seealso cref="M:NKH.MindSqualls.NxtMotor.ResetMotorPosition"/>
        public void ResetMotorPosition(bool relative)
        {
            motorX.ResetMotorPosition(relative);
            motorY.ResetMotorPosition(relative);
        }

        /// <summary>
        /// <para>Coasts the motors.</para>
        /// </summary>
        public void Coast()
        {
            motorX.Coast();
            motorY.Coast();
        }

        /// <summary>
        /// <para>Brakes the motors.</para>
        /// </summary>
        public void Brake()
        {
            motorX.Brake();
            motorY.Brake();
        }

        /// <summary>
        /// <para>Puts the motors into the idle state.</para>
        /// </summary>
        public void Idle()
        {
            motorX.Idle();
            motorY.Idle();
        }

        #endregion
    }
}
