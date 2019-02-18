using System.Threading;

namespace NKH.MindSqualls.MotorControl
{
    /// <summary>
    /// <para>Class representing the NXT brick.</para>
    /// <para>It adds methods that can be used to control the running of the MotorControl-program.</para>
    /// </summary>
    public class McNxtBrick : NxtBrick
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <remarks>
        /// <para>It requires that MotorControl is downloaded to the NXT block and is running.</para>
        /// </remarks>
        /// <param name="commLinkType">Indicates whether Bluetooth or USB should be used for the communication with the NXT brick.</param>
        /// <param name="serialPortNo">The COM port used. Only relevant for Bluetooth links.</param>
        public McNxtBrick(NxtCommLinkType commLinkType, byte serialPortNo):
            base(commLinkType, serialPortNo)
        {
        }

        /// <summary>
        /// <para>Queries if the MotorControl-program is on the NXT</para>
        /// </summary>
        /// <returns>True if the MotorControl-program is on the NXT, false if not</returns>
        public bool IsMotorControlOnNxt()
        {
            return MotorControlProxy.IsMotorControlOnNxt(CommLink);
        }

        /// <summary>
        /// <para>Queries if the MotorControl-program is currently running on the NXT.</para>
        /// </summary>
        /// <returns>True if the MotorControl-program is running, false if not</returns>
        public bool IsMotorControlRunning()
        {
            return MotorControlProxy.IsMotorControlRunningOnNxt(CommLink);
        }

        /// <summary>
        /// <para>Starts the MotorControl-program.</para>
        /// </summary>
        public void StartMotorControl()
        {
            MotorControlProxy.StartMotorControl(CommLink);
            Thread.Sleep(500);

            InitSensors();
        }

        /// <summary>
        /// <para>Stops the MotorControl-program.</para>
        /// </summary>
        /// <remarks>
        /// <para>Stops any running program, even it the program is not the MotorControl-program.</para>
        /// </remarks>
        public void StopMotorControl()
        {
            MotorControlProxy.StopMotorControl(CommLink);
            Thread.Sleep(500);

            InitSensors();
        }
    }
}
