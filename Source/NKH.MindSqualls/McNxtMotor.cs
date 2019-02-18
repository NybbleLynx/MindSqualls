namespace NKH.MindSqualls.MotorControl
{
    /// <summary>
    /// <para>Class representing a motor.</para>
    /// <para>It uses the MotorControl-program to achieve higher precision when driving a NXT motor.</para>
    /// </summary>
    /// <remarks>
    /// <para>It requires that MotorControl is downloaded to the NXT block and is running.</para>
    /// </remarks>
    public class McNxtMotor : NxtMotor
    {
        /// <summary>
        /// <para>Run the motor.</para>
        /// </summary>
        /// <param name="power">The power for the motor</param>
        /// <param name="tachoLimit">The duration in degrees, 0 = unlimited</param>
        public override void Run(sbyte power, uint tachoLimit)
        {
            // Translate motor-port to MotorControl-format.

            MotorControlMotorPort portMotorControl;
            switch (Port)
            {
                case NxtMotorPort.PortA:
                    portMotorControl = MotorControlMotorPort.PortA;
                    break;
                case NxtMotorPort.PortB:
                    portMotorControl = MotorControlMotorPort.PortB;
                    break;
                case NxtMotorPort.PortC:
                    portMotorControl = MotorControlMotorPort.PortC;
                    break;
                default:
                    throw new NxtException("Unknown port.");
            }

            // Translate power to MotorControl-format.

            if (power > 100) power = 100;
            if (power < -100) power = -100;

            int powerTemp = power;
            if (powerTemp < 0) powerTemp = 100 - powerTemp;

            string powerMotorControl = powerTemp.ToString().PadLeft(3, '0');

            // Translate tachoLimit to MotorControl-format.

            string tachoLimitMotorControl = tachoLimit.ToString().PadLeft(6, '0');

            // Call MotorControl.

            if (tachoLimit != 0)
            {
                char mode = '7';
                MotorControlProxy.CONTROLLED_MOTORCMD(this.Brick.CommLink, portMotorControl, powerMotorControl, tachoLimitMotorControl, mode);
            }
            else
            {
                char speedRegulation = '1';
                MotorControlProxy.CLASSIC_MOTORCMD(this.Brick.CommLink, portMotorControl, powerMotorControl, tachoLimitMotorControl, speedRegulation);
            }
        }

        /// <summary>
        /// <para>Run the motor and end coasting (spinning freely).</para>
        /// </summary>
        /// <param name="power">The power for the motor</param>
        /// <param name="tachoLimit">The duration in degrees, 0 = unlimited</param>
        public void RunAndCoast(sbyte power, uint tachoLimit)
        {
            // Translate motor-port to MotorControl-format.

            MotorControlMotorPort portMotorControl;
            switch (Port)
            {
                case NxtMotorPort.PortA:
                    portMotorControl = MotorControlMotorPort.PortA;
                    break;
                case NxtMotorPort.PortB:
                    portMotorControl = MotorControlMotorPort.PortB;
                    break;
                case NxtMotorPort.PortC:
                    portMotorControl = MotorControlMotorPort.PortC;
                    break;
                default:
                    throw new NxtException("Unknown port.");
            }

            // Translate power to MotorControl-format.

            if (power > 100) power = 100;
            if (power < -100) power = -100;

            int powerTemp = power;
            if (powerTemp < 0) powerTemp = 100 - powerTemp;

            string powerMotorControl = powerTemp.ToString().PadLeft(3, '0');

            // Translate tachoLimit to MotorControl-format.

            string tachoLimitMotorControl = tachoLimit.ToString().PadLeft(6, '0');

            // Call MotorControl.

            char speedRegulation = '1';
            MotorControlProxy.CLASSIC_MOTORCMD(this.Brick.CommLink, portMotorControl, powerMotorControl, tachoLimitMotorControl, speedRegulation);
        }
    }
}
