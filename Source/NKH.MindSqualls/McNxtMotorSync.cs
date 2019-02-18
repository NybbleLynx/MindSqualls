namespace NKH.MindSqualls.MotorControl
{
    /// <summary>
    /// <para>Class representing a synchronized pair of motors.</para>
    /// <para>It uses the MotorControl-program to achieve higher precision when driving a synchronized NXT motor-pair.</para>
    /// </summary>
    /// <remarks>
    /// <para>It requires that MotorControl is downloaded to the NXT block and is running.</para>
    /// </remarks>
    /// <seealso cref="NxtMotorSync"/>
    public class McNxtMotorSync : NxtMotorSync
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="motorX">The 1. motor of the synchronized pair</param>
        /// <param name="motorY">The 2. motor of the synchronized pair</param>
        public McNxtMotorSync(NxtMotor motorX, NxtMotor motorY)
            : base(motorX, motorY)
        {
        }

        /// <summary>
        /// <para>Run the motors (in sync).</para>
        /// </summary>
        /// <param name="power">The poser</param>
        /// <param name="tachoLimit">The tacho limit in degrees, 0 means unlimited</param>
        /// <param name="turnRatio">Turn ratio is not upported by MotorControl and is ignored</param>
        public override void Run(sbyte power, ushort tachoLimit, sbyte turnRatio)
        {
            MotorControlMotorPort motorControlPort;
            if (motorX.Port == NxtMotorPort.PortA && motorY.Port == NxtMotorPort.PortB)
            {
                motorControlPort = MotorControlMotorPort.PortsAB;
            }
            else if (motorX.Port == NxtMotorPort.PortA && motorY.Port == NxtMotorPort.PortC)
            {
                motorControlPort = MotorControlMotorPort.PortsAC;
            }
            else if (motorX.Port == NxtMotorPort.PortB && motorY.Port == NxtMotorPort.PortC)
            {
                motorControlPort = MotorControlMotorPort.PortsBC;
            }
            else
                throw new NxtException("Unknown port combination of ports.");

            // Translate power to MotorControl-format.

            if (power > 100) power = 100;
            if (power < -100) power = -100;

            int powerTmp = power;
            if (powerTmp < 0) powerTmp = 100 - powerTmp;

            char mode = '7';

            MotorControlProxy.CONTROLLED_MOTORCMD(
                this.motorX.Brick.CommLink,
                motorControlPort,
                powerTmp.ToString().PadLeft(3, '0'),
                tachoLimit.ToString().PadLeft(6, '0'),
                mode);
        }
    }
}
