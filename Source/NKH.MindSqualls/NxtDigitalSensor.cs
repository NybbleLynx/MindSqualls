using System;
using System.Text;
using System.Threading;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Abstract class representing a Digital sensor, i.e. a sensor using the I<sup>2</sup>C protocol.</para>
    /// </summary>
    /// <remarks>
    /// <para>The "Device Memory Arrangement" for a Digital sensor is specified at p. 10 of the document:</para>
    /// <para>LEGO MINDSTORMS NXT,<br/>
    /// Hardware Developer Kit</para>
    /// <para>which can be downloaded at:<br/>
    /// <a href="http://mindstorms.lego.com/overview/NXTreme.aspx" target="_blank">http://mindstorms.lego.com/overview/NXTreme.aspx</a></para>
    /// <para>The Ultrasonic sensor protocol is specifed in appendix 7 of the HDK.</para>
    /// <para>"Digital I/O Communication Methods" is also explained in the Executable File Specification document, p. 70-72.</para>
    /// <para>.oOo.</para>
    /// <para>According to the LEGO MINDSTORMS NXT Hardware Developer Kit p. 7, sensors is divided into three types: active sensors (e.g. Robotics Invention Systems sensors), passive sensors (e.g. the NXT touch, light, and sound sensors), and digital sensors (e.g. the NXT ultrasonic sensor and the HiTechnic compass sensor). The three abstract classes NxtActiveSensor, NxtPassiveSensor, and NxtDigitalSensor reflect this.</para>
    /// </remarks>
    /// <seealso cref="NxtActiveSensor"/>
    /// <seealso cref="NxtPassiveSensor"/>
    public abstract class NxtDigitalSensor : NxtSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtDigitalSensor()
            : base(NxtSensorType.LOWSPEED_9V, NxtSensorMode.RAWMODE)
        {
        }

        /// <summary>
        /// <para>Initializes the digital sensor.</para>
        /// </summary>
        internal override void InitSensor()
        {
            if (Brick.IsConnected)
            {
                base.InitSensor();

                // Clear any garbage bytes from the digital sensor.
                byte? bytesReady;
                try
                {
                    // For an obscure reason there seems to be a tendency that LsGetStatus return an error the first time it is called...
                    bytesReady = Brick.CommLink.LsGetStatus(sensorPort);
                }
                catch (NxtCommunicationProtocolException)
                {
                    bytesReady = Brick.CommLink.LsGetStatus(sensorPort);
                }
                byte[] garbage = (bytesReady != null && bytesReady.Value > 0)
                    ? Brick.CommLink.LsRead(sensorPort)
                    : null;
            }
        }

        /// <summary>
        /// <para>Sends an I<sup>2</sup>C request to the Ultrasonic sensor, and receive the reply if applicable.</para>
        /// </summary>
        /// <param name="request">The I2C request</param>
        /// <param name="rxDataLength">Length of the expected reply</param>
        /// <returns>The reply from the sensor, or a null-value</returns>
        internal byte[] SendN(byte[] request, byte rxDataLength)
        {
            // Send the I2C request to the sensor.
            Brick.CommLink.LsWrite(sensorPort, request, rxDataLength);

            // Return null if no reply is expected.
            if (rxDataLength == 0) return null;

            // Wait until the reply is ready in the sensor.
            byte? bytesReady = 0;
            do
            {
                try
                {
                    Thread.Sleep(10);

                    // Query how many bytes are ready in the sensor.
                    bytesReady = Brick.CommLink.LsGetStatus(sensorPort);
                }
                catch (NxtCommunicationProtocolException ex)
                {
                    // The port is still busy. Try again.
                    if (ex.errorMessage == NxtErrorMessage.PendingCommunicationTransactionInProgress)
                    {
                        bytesReady = 0;
                        Thread.Sleep(10);

                        continue;
                    }

                    // Rethrow if the error is not a CommunicationBusError.
                    if (ex.errorMessage != NxtErrorMessage.CommunicationBusError) throw;

                    // Clears error condition - any LsWrite should do.
                    DoAnyLsWrite();

                    // Exit Send().
                    return null;
                }
            }
            while (bytesReady < rxDataLength);

            // Read, and return, the reply from the sensor.
            return Brick.CommLink.LsRead(sensorPort);
        }

        /// <summary>
        /// <para>Sends an I<sup>2</sup>C request to the Ultrasonic sensor, and receives the reply.</para>
        /// </summary>
        /// <remarks>
        /// <pare>Intended for the special case where a 1-byte reply is expexted.</pare>
        /// </remarks>
        /// <param name="request">The I2C request</param>
        /// <returns>The reply from the sensor</returns>
        internal byte[] Send1(byte[] request)
        {
            // Send the I2C request to the sensor.
            Brick.CommLink.LsWrite(sensorPort, request, 1);

            while (true)
            {
                LsReadDelay();

                try
                {
                    return Brick.CommLink.LsRead(sensorPort);
                }
                catch (NxtCommunicationProtocolException ex)
                {
                    if (ex.errorMessage == NxtErrorMessage.PendingCommunicationTransactionInProgress) continue;

                    if (ex.errorMessage == NxtErrorMessage.CommunicationBusError)
                    {
                        // Doing a LsWrite() clears the CommunicationBusError from the bus.
                        Brick.CommLink.LsWrite(sensorPort, request, 1);
                        continue;
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// <para>Delay-time (in milliseconds) from when a LsWrite() has been sent to a LsRead() is attempted.</para>
        /// </summary>
        /// <remarks>
        /// <para>Bluetooth connection: By waiting a few milliseconds, the chance of calling the I<sup>2</sup>C bus before it is ready is reduced. This reduces the times where an error-condition arises, and thereby increases the overall efficiency and throughput.</para>
        /// <para>USB connection: Not used.</para>
        /// </remarks>
        protected int lsReadDelayTimeMs = 22;  // To pause 22ms seems to be the optional period.

        private void LsReadDelay()
        {
            if (this.Brick.CommLink is NxtBluetoothConnection)
            {
                Thread.Sleep(lsReadDelayTimeMs);
            }
        }

        #region I2C protocol.

        private void DoAnyLsWrite()
        {
            // Doing this will clear the bus from errors.
            ReadByteFromAddress(0x42);
        }

        /// <summary>
        /// <para>Both the Ultrasonic sensor and the Compass plus Color sensors from HiTecnic uses 0x02 as the device address. However the documentation do not put any constraints to this value.</para>
        /// </summary>
        internal byte deviceAddress = 0x02;

        #region Constants.

        /// <summary>
        /// <para>Returns the version of the sensor.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public string ReadVersion()
        {
            byte[] request = new byte[] { deviceAddress, 0x00 };
            byte[] reply = SendN(request, 8);

            return (reply != null)
                ? Encoding.ASCII.GetString(reply, 0, reply.Length).TrimEnd('\0', '?', ' ')
                : null;
        }

        /// <summary>
        /// <para>Returns the name of the manufacturer, e.g. "LEGO".</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public string ReadProductId()
        {
            byte[] request = new byte[] { deviceAddress, 0x08 };
            byte[] reply = SendN(request, 8);

            return (reply != null)
                ? Encoding.ASCII.GetString(reply, 0, reply.Length).TrimEnd('\0', '?')
                : null;
        }

        /// <summary>
        /// <para>Returns the sensor type, e.g. "Sonar".</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public string ReadSensorType()
        {
            byte[] request = new byte[] { deviceAddress, 0x10 };
            byte[] reply = SendN(request, 8);

            return (reply != null)
                ? Encoding.ASCII.GetString(reply, 0, reply.Length).TrimEnd('\0', '?', ' ')
                : null;
        }

        #endregion

        #region Variables.
        #endregion

        #region Commands.
        #endregion

        #region Utilities.

        /// <summary>
        /// <para>Reads the value of the variable stored at the address.</para>
        /// </summary>
        /// <param name="address">The address of the variable</param>
        /// <returns>The value of the variable</returns>
        internal byte? ReadByteFromAddress(byte address)
        {
            byte[] request = new byte[] { deviceAddress, address };

            // v2.2: I think that it is very weird and frustrating that the Send1() optimization only works for the Bluetooth-case.
            byte[] reply = (this.Brick.CommLink is NxtBluetoothConnection)
                ? Send1(request)
                : SendN(request, 1);

            if (reply != null && reply.Length >= 1)
                return reply[0];
            else
                return null;
        }

        /// <summary>
        /// <para>Reads the value of the variable stored at the two consecutive bytes at the address.</para>
        /// </summary>
        /// <param name="address">The address of the variable</param>
        /// <returns>The value of the variable</returns>
        internal UInt16? ReadWordFromAdress(byte address)
        {
            byte[] request = new byte[] { deviceAddress, address };
            byte[] reply = SendN(request, 2);

            if (reply != null && reply.Length >= 2)
                return Util.GetUInt16(reply, 0);
            else
                return null;
        }

        internal void CommandToAddress(byte address, byte command)
        {
            byte[] request = new byte[] { deviceAddress, address, command };
            SendN(request, 0);
        }

        #endregion

        #endregion
    }
}
