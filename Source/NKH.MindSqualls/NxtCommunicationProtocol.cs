using System;
using System.Text;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Abstract class representing the NXT communication protocol.</para>
    /// </summary>
    /// <remarks>
    /// <para>The communication protocol for the NXT brick is specified in the documents:</para>
    /// 
    /// <para>Lego Mindstorms NXT,<br/>
    /// Bluetooth Developer Kit,<br/>
    /// Appendix 1: LEGO MINDSTORMS NXT Communication protocol<br/>
    /// - and:<br/>
    /// Appendix 2: LEGO MINDSTORMS NXT Direct commands</para>
    /// 
    /// <para>- which can be downloaded at:<br/>
    /// <a href="http://mindstorms.lego.com/overview/NXTreme.aspx" target="_blank">http://mindstorms.lego.com/overview/NXTreme.aspx</a></para>
    /// 
    /// <para>For NXT 2.0 specific informaton:<br/>
    /// <a href="http://www.ni.com/pdf/manuals/372574c.pdf" target="_blank">http://www.ni.com/pdf/manuals/372574c.pdf</a></para>
    /// 
    /// <para>A special thanks goes to Bram Fokke for his NXT# project:<br/>
    /// <a href="http://nxtsharp.fokke.net/" target="_blank">http://nxtsharp.fokke.net/</a></para>
    /// 
    /// <para>Without his work, I doubt very much if my own work would have gotten off the ground. I have even st... ehem... borrowed a bit of his code here and there. However the major part of the code is my own.</para>
    /// </remarks>
    public abstract class NxtCommunicationProtocol
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtCommunicationProtocol()
            : base()
        { }

        #region Connection.

        /// <summary>
        /// <para>Flag indicating if a reply should always be recieved.</para>
        /// </summary>
        public bool ReplyRequired = false;

        /// <summary>
        /// <para>Connect to the NXT brick.</para>
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// <para>Disconnect from the NXT brick.</para>
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// <para>Returns a boolean indicating if the NXT brick is connected.</para>
        /// </summary>
        public abstract bool IsConnected
        {
            get;
        }

        /// <summary>
        /// <para>Send a request to the NXT brick, and return a reply.</para>
        /// </summary>
        /// <param name="request">The request to the NXT brick</param>
        /// <returns>The reply from the NXT brick, or null</returns>
        protected abstract byte[] Send(byte[] request);

        #endregion

        #region Protocol.

        /// <summary>
        /// <para>STARTPROGRAM</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 5.</para>
        /// </remarks>
        /// <param name="fileName">File name</param>
        public void StartProgram(string fileName)
        {
            ValidateFilename(fileName);

            byte[] fileNameByteArr = Encoding.ASCII.GetBytes(fileName);

            byte[] request = new byte[22];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.StartProgram;
            fileNameByteArr.CopyTo(request, 2);

            Send(request);
        }

        /// <summary>
        /// <para>STOPPROGRAM</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 5.</para>
        /// </remarks>
        public void StopProgram()
        {
            byte[] request = new byte[] {
                (byte) (ReplyRequired ? 0x00 : 0x80),
                (byte) NxtCommand.StopProgram
            };

            Send(request);
        }

        /// <summary>
        /// <para>PLAYSOUNDFILE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 5.</para>
        /// </remarks>
        /// <param name="loop">Loop sound file indefinately?</param>
        /// <param name="fileName">File name</param>
        public void PlaySoundfile(bool loop, string fileName)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[23];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.PlaySoundfile;
            request[2] = (byte)(loop ? 0xFF : 0x00);
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 3);

            Send(request);
        }

        /// <summary>
        /// <para>PLAYTONE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 6.</para>
        /// </remarks>
        /// <param name="frequency">Frequency for the tone, Hz</param>
        /// <param name="duration">Duration of the tone, ms</param>
        public void PlayTone(UInt16 frequency, UInt16 duration)
        {
            if (frequency < 200) frequency = 200;
            if (frequency > 14000) frequency = 14000;

            byte[] request = new byte[6];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.PlayTone;
            Util.SetUInt16(frequency, request, 2);
            Util.SetUInt16(duration, request, 4);

            Send(request);
        }

        /// <summary>
        /// <para>SETOUTPUTSTATE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 6.</para>
        /// </remarks>
        /// <param name="motorPort">Motor port</param>
        /// <param name="power">Power set point</param>
        /// <param name="mode">Mode</param>
        /// <param name="regulationMode">Regulation mode</param>
        /// <param name="turnRatio">Turn ratio</param>
        /// <param name="runState">Run state</param>
        /// <param name="tachoLimit">Tacho limit, 0: run forever</param>
        public void SetOutputState(NxtMotorPort motorPort, sbyte power, NxtMotorMode mode, NxtMotorRegulationMode regulationMode, sbyte turnRatio, NxtMotorRunState runState, UInt32 tachoLimit)
        {
            if (power < -100) power = -100;
            if (power > 100) power = 100;

            if (turnRatio < -100) turnRatio = -100;
            if (turnRatio > 100) turnRatio = 100;

            byte[] request = new byte[12];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.SetOutputState;
            request[2] = (byte)motorPort;
            request[3] = (byte)power;
            request[4] = (byte)mode;
            request[5] = (byte)regulationMode;
            request[6] = (byte)turnRatio;
            request[7] = (byte)runState;
            Util.SetUInt32(tachoLimit, request, 8);

            Send(request);
        }

        /// <summary>
        /// <para>SETINPUTMODE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 7.</para>
        /// </remarks>
        /// <param name="sensorPort">Input Port</param>
        /// <param name="sensorType">Sensor Type</param>
        /// <param name="sensorMode">Sensor Mode</param>
        public void SetInputMode(NxtSensorPort sensorPort, NxtSensorType sensorType, NxtSensorMode sensorMode)
        {
            byte[] request = new byte[] {
                (byte) (ReplyRequired ? 0x00 : 0x80),
                (byte) NxtCommand.SetInputMode,
                (byte) sensorPort,
                (byte) sensorType,
                (byte) sensorMode
            };

            Send(request);
        }

        /// <summary>
        /// <para>GETOUTPUTSTATE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 8.</para>
        /// </remarks>
        /// <param name="motorPort">Ourput Port</param>
        /// <returns>Returns a parsed NxtGetOutputStateReply with the reply</returns>
        public NxtGetOutputStateReply? GetOutputState(NxtMotorPort motorPort)
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.GetOutputState,
                (byte) motorPort
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte motorPortOut = reply[3];
            if (motorPortOut != (byte)motorPort)
                throw new NxtException(string.Format("Output motor port, {0}, was different from input motor port, {1}.", motorPortOut, motorPort));

            NxtGetOutputStateReply result;
            result.power = (sbyte)reply[4];
            result.mode = (NxtMotorMode)reply[5];
            result.regulationMode = (NxtMotorRegulationMode)reply[6];
            result.turnRatio = (sbyte)reply[7];
            result.runState = (NxtMotorRunState)reply[8];
            result.tachoLimit = Util.GetUInt32(reply, 9);
            result.tachoCount = Util.GetInt32(reply, 13);
            result.blockTachoCount = Util.GetInt32(reply, 17);
            result.rotationCount = Util.GetInt32(reply, 21);

            return result;
        }

        /// <summary>
        /// <para>GETINPUTVALUES</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 8.</para>
        /// <para>Used with passive sensors like the Touch, Light and Sound sensors.</para>
        /// </remarks>
        /// <param name="sensorPort">Input Port</param>
        /// <returns>Returns a NxtGetInputValues with the parsed reply</returns>
        public NxtGetInputValuesReply? GetInputValues(NxtSensorPort sensorPort)
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.GetInputValues,
                (byte) sensorPort
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte sensorPortOut = reply[3];
            if (sensorPortOut != (byte)sensorPort)
                throw new NxtException(string.Format("Output sensor port, {0}, was different from input sensor port, {1}.", sensorPortOut, sensorPort));

            NxtGetInputValuesReply result;
            result.valid = (reply[4] != 0x00);
            result.calibrated = (reply[5] != 0x00);
            result.sensorType = (NxtSensorType)reply[6];
            result.sensorMode = (NxtSensorMode)reply[7];
            result.rawAdValue = Util.GetUInt16(reply, 8);
            result.normalizedAdValue = Util.GetUInt16(reply, 10);
            result.scaledValue = Util.GetInt16(reply, 12);
            result.calibratedValue = Util.GetInt16(reply, 14);

            return result;
        }

        /// <summary>
        /// <para>RESETINPUTSCALEDVALUE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 8.</para>
        /// </remarks>
        /// <param name="sensorPort">Input Port</param>
        public void ResetInputScaledValue(NxtSensorPort sensorPort)
        {
            byte[] request = new byte[] {
                (byte) (ReplyRequired ? 0x00 : 0x80),
                (byte) NxtCommand.ResetInputScaledValue,
                (byte) sensorPort
            };

            Send(request);
        }

        /// <summary>
        /// <para>MESSAGEWRITE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 9.</para>
        /// </remarks>
        /// <param name="inBox">Inbox number</param>
        /// <param name="messageData">Message data</param>
        public void MessageWrite(NxtMailbox inBox, string messageData)
        {
            if (!messageData.EndsWith("\0"))
                messageData += '\0';

            int messageSize = messageData.Length;
            if (messageSize > 59)
                throw new ArgumentException("Message may not exceed 59 characters.");

            byte[] request = new byte[4 + messageSize];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.MessageWrite;
            request[2] = (byte)inBox;
            request[3] = (byte)messageSize;
            Encoding.ASCII.GetBytes(messageData).CopyTo(request, 4);

            Send(request);
        }

        /// <summary>
        /// <para>MESSAGEWRITE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 9.</para>
        /// </remarks>
        /// <param name="inBox">Inbox number</param>
        /// <param name="messageData">Message data</param>
        public void MessageWrite(NxtMailbox inBox, byte[] messageData)
        {
            int messageSize = messageData.Length + 1;  // Add 1 for the 0-byte at the end.
            if (messageSize > 59)
                throw new ArgumentException("Message may not exceed 59 characters.");

            byte[] request = new byte[4 + messageSize];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.MessageWrite;
            request[2] = (byte)inBox;
            request[3] = (byte)messageSize;
            messageData.CopyTo(request, 4);
            request[request.Length - 1] = 0;

            Send(request);
        }

        /// <summary>
        /// <para>RESETMOTORPOSITION</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 9.</para>
        /// </remarks>
        /// <param name="motorPort">Output port</param>
        /// <param name="relative">Relative? True: position relative to last movement, False: absolute position</param>
        public void ResetMotorPosition(NxtMotorPort motorPort, bool relative)
        {
            byte[] request = new byte[] {
                (byte) (ReplyRequired ? 0x00 : 0x80),
                (byte) NxtCommand.ResetMotorPosition,
                (byte) motorPort,
                (byte) (relative ? 0xFF : 0x00)
            };

            Send(request);
        }

        /// <summary>
        /// <para>GETBATTERYLEVEL</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 9.</para>
        /// </remarks>
        /// <returns>Voltage in millivolts</returns>
        public UInt16? GetBatteryLevel()
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.GetBatteryLevel
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            UInt16 voltage = Util.GetUInt16(reply, 3);
            return voltage;
        }

        /// <summary>
        /// <para>STOPSOUNDPLAYBACK</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 9.</para>
        /// </remarks>
        public void StopSoundPlayback()
        {
            byte[] request = new byte[] {
                (byte) (ReplyRequired ? 0x00 : 0x80),
                (byte) NxtCommand.StopSoundPlayback
            };

            Send(request);
        }

        /// <summary>
        /// <para>KEEPALIVE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 10.</para>
        /// </remarks>
        /// <returns>Current sleep time limit, ms</returns>
        public UInt32? KeepAlive()
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.KeepAlive
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            UInt32 currentSleepLimit = Util.GetUInt32(reply, 3);

            return currentSleepLimit;
        }

        /// <summary>
        /// <para>LSGETSTATUS</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 10.</para>
        /// <para>Returns the number of bytes ready in the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.</para>
        /// </remarks>
        /// <param name="sensorPort">Sensor port</param>
        /// <returns>Bytes Ready (count of available bytes to read)</returns>
        /// <seealso cref="LsRead"/>
        /// <seealso cref="LsWrite"/>
        public byte? LsGetStatus(NxtSensorPort sensorPort)
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.LsGetStatus,
                (byte) sensorPort
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte bytesReady = reply[3];

            return bytesReady;
        }

        /// <summary>
        /// <para>LSWRITE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 10.</para>
        /// <para>Writes data to the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.</para>
        /// </remarks>
        /// <param name="sensorPort">Sensor port</param>
        /// <param name="txData">Tx Data</param>
        /// <param name="rxDataLength">Rx Data Length</param>
        /// <seealso cref="LsRead"/>
        /// <seealso cref="LsGetStatus"/>
        public void LsWrite(NxtSensorPort sensorPort, byte[] txData, byte rxDataLength)
        {
            byte txDataLength = (byte)txData.Length;
            if (txDataLength == 0)
                throw new ArgumentException("No data to send.");

            if (txDataLength > 16)
                throw new ArgumentException("Tx data may not exceed 16 bytes.");

            if (rxDataLength < 0 || 16 < rxDataLength)
                throw new ArgumentException("Rx data length should be in the interval 0-16.");

            byte[] request = new byte[5 + txDataLength];
            request[0] = (byte)(ReplyRequired ? 0x00 : 0x80);
            request[1] = (byte)NxtCommand.LsWrite;
            request[2] = (byte)sensorPort;
            request[3] = txDataLength;
            request[4] = rxDataLength;
            txData.CopyTo(request, 5);

            Send(request);
        }

        /// <summary>
        /// <para>LSREAD</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 10.</para>
        /// <para>Reads data from the LowSpeed port. Used with digital sensors like the Ultrasonic sensor.</para>
        /// </remarks>
        /// <param name="sensorPort">The sensor port</param>
        /// <returns>The data read from the port</returns>
        /// <seealso cref="LsGetStatus"/>
        /// <seealso cref="LsWrite"/>
        public byte[] LsRead(NxtSensorPort sensorPort)
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.LsRead,
                (byte) sensorPort
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte bytesRead = reply[3];

            byte[] rxData = new byte[bytesRead];
            Array.Copy(reply, 4, rxData, 0, bytesRead);

            return rxData;
        }

        /// <summary>
        /// <para>GETCURRENTPROGRAMNAME</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 11.</para>
        /// </remarks>
        /// <returns>File name of the running program</returns>
        public string GetCurrentProgramName()
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.GetCurrentProgramName
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            string fileName = Encoding.ASCII.GetString(reply, 3, 20).TrimEnd('\0');
            return fileName;
        }

        /// <summary>
        /// <para>MESSAGEREAD</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 2, p. 11.</para>
        /// </remarks>
        /// <param name="remoteInboxNo">Remote Inbox number</param>
        /// <param name="localInboxNo">Local Inbox number</param>
        /// <param name="remove">Remove? True: clears message from Remote Inbox</param>
        /// <returns>Message data</returns>
        public string MessageRead(NxtMailbox2 remoteInboxNo, NxtMailbox localInboxNo, bool remove)
        {
            byte[] request = new byte[] {
                0x00,
                (byte) NxtCommand.MessageRead,
                (byte) remoteInboxNo,
                (byte) localInboxNo,
                (byte) (remove ? 0xFF : 0x00)
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte localInboxNoOut = reply[3];  // TODO: Validate on this?

            byte messageSize = reply[4];

            string message = Encoding.ASCII.GetString(reply, 5, messageSize).TrimEnd('\0');
            return message;
        }

        /// <summary>
        /// <para>OPEN READ COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 7.</para>
        /// <para>Close the handle after use. The handle is automatically closed if an error occurs.</para>
        /// </remarks>
        /// <param name="fileName">The name of the file</param>
        /// <returns>A NxtOpenReadReply with the result of the request</returns>
        public NxtOpenReadReply? OpenRead(string fileName)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.OpenRead;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);

            byte[] reply = Send(request);

            if (reply == null) return null;

            NxtOpenReadReply result;
            result.handle = reply[3];
            result.fileSize = Util.GetUInt32(reply, 4);
            return result;
        }

        /// <summary>
        /// <para>OPEN WRITE COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 7.</para>
        /// <para>Close the handle after use. The handle is automatically closed if an error occurs.</para>
        /// </remarks>
        /// <param name="fileName">The file name</param>
        /// <param name="fileSize">The file size</param>
        /// <returns>A handle to the file</returns>
        public byte? OpenWrite(string fileName, UInt32 fileSize)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[26];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.OpenWrite;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);
            Util.SetUInt32(fileSize, request, 22);

            byte[] reply = Send(request);

            if (reply == null) return null;

            return reply[3];
        }

        /// <summary>
        /// <para>READ COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 8.</para>
        /// </remarks>
        /// <param name="handle">A handle to the file</param>
        /// <param name="bytesToRead">Number of data bytes to be read</param>
        /// <returns>A byte array with the read data</returns>
        public byte[] Read(byte handle, UInt16 bytesToRead)
        {
            byte[] request = new byte[5];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.Read;
            request[2] = handle;
            Util.SetUInt16(bytesToRead, request, 3);

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte handleOut = reply[3];
            if (handleOut != handle)
                throw new NxtException(string.Format("Output handle, {0}, was different from input handle, {1}.", handleOut, handle));

            UInt16 bytesRead = Util.GetUInt16(reply, 4);

            byte[] respons = new byte[bytesRead];
            Array.Copy(reply, 6, respons, 0, bytesRead);

            return respons;
        }

        /// <summary>
        /// <para>WRITE COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 8.</para>
        /// </remarks>
        /// <param name="handle">File handle to write to</param>
        /// <param name="data">Data to write</param>
        /// <returns>Number of bytes written</returns>
        public UInt16? Write(byte handle, byte[] data)
        {
            byte[] request = new byte[3 + data.Length];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.Write;
            request[2] = handle;
            data.CopyTo(request, 3);

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte handleOut = reply[3];
            if (handleOut != handle)
                throw new NxtException(string.Format("Output handle, {0}, was different from input handle, {1}.", handleOut, handle));

            UInt16 bytesWritten = Util.GetUInt16(reply, 4);

            return bytesWritten;
        }

        /// <summary>
        /// <para>CLOSE COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 8.</para>
        /// </remarks>
        /// <param name="handle">File handle to close</param>
        public void Close(byte handle)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.Close,
                handle
            };

            byte[] reply = Send(request);

            if (reply == null) return;

            byte handleOut = reply[3];
            if (handleOut != handle)
                throw new NxtException(string.Format("Output handle, {0}, was different from input handle, {1}.", handleOut, handle));
        }

        /// <summary>
        /// <para>DELETE COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 9.</para>
        /// </remarks>
        /// <param name="fileName">The file name</param>
        public void Delete(string fileName)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.Delete;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);

            byte[] reply = Send(request);

            if (reply == null) return;

            string fileNameOut = Encoding.ASCII.GetString(reply, 3, 20);
            if (fileNameOut != fileName)
                throw new NxtException(string.Format("The file reported as deleted, '{0}', was different from the file requested, '{1}'.", fileNameOut, fileName));
        }

        /// <summary>
        /// <para>FIND FIRST</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 9.</para>
        /// <para>Close the handle after use. The handle is automatically closed if an error occurs.</para>
        /// </remarks>
        /// <param name="fileName">Filename or -mask to search</param>
        /// <returns>A NxtFindFileReply containing the result for the search</returns>
        /// <seealso cref="FindNext"/>
        public NxtFindFileReply? FindFirst(string fileName)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.FindFirst;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);

            return SendAndParseNxtFindFileReply(request);
        }

        /// <summary>
        /// <para>FIND NEXT</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 10.</para>
        /// <para>Close the handle after use. The handle is automatically closed if an error occurs.</para>
        /// </remarks>
        /// <param name="handle">Handle from the previous found file or from the FindFirst command</param>
        /// <returns>A NxtFindFileReply containing the result for the search</returns>
        /// <seealso cref="FindFirst"/>
        public NxtFindFileReply? FindNext(byte handle)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.FindNext,
                handle
            };

            return SendAndParseNxtFindFileReply(request);
        }

        /// <summary>
        /// <para>Sends a FindFirst- or FindNext-request and parses the result as a NxtFindFileReply.</para>
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>A NxtFindFileReply containing the parsed result</returns>
        /// <seealso cref="FindFirst"/>
        /// <seealso cref="FindNext"/>
        private NxtFindFileReply? SendAndParseNxtFindFileReply(byte[] request)
        {
            byte[] reply;
            NxtFindFileReply result;

            try
            {
                reply = Send(request);

                if (reply == null) return null;
            }
            catch (NxtCommunicationProtocolException ex)
            {
                if (ex.errorMessage == NxtErrorMessage.FileNotFound)
                {
                    result.fileFound = false;
                    result.handle = 0;
                    result.fileName = "";
                    result.fileSize = 0;
                    return result;
                }

                // Rethrow if not a FileNotFound error.
                throw;
            }

            result.fileFound = true;
            result.handle = reply[3];
            result.fileName = Encoding.ASCII.GetString(reply, 4, 20).TrimEnd('\0');
            result.fileSize = Util.GetUInt32(reply, 24);
            return result;
        }

        /// <summary>
        /// <para>GET FIRMWARE VERSION</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 11.</para>
        /// </remarks>
        /// <returns>A NxtGetFirmwareVersionReply with the protocol-, and the firmware versions.</returns>
        public NxtGetFirmwareVersionReply? GetFirmwareVersion()
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.GetFirmwareVersion
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            NxtGetFirmwareVersionReply result;
            result.protocolVersion = new Version(reply[4], reply[3]);
            result.firmwareVersion = new Version(reply[6], reply[5]);
            return result;
        }

        /// <summary>
        /// <para>OPEN WRITE LINEAR COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 11.</para>
        /// </remarks>
        /// <param name="fileName">The file name</param>
        /// <param name="fileSize">The file size</param>
        /// <returns>A handle to the file</returns>
        public byte? OpenWriteLinear(string fileName, UInt32 fileSize)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[26];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.OpenWriteLinear;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);  // NOTE: I'm not sure if the documentation is 100%
            Util.SetUInt32(fileSize, request, 22);  // ... correct here, since it do not allow space for the null terminator.

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte handle = reply[3];
            return handle;
        }

        /// <summary>
        /// <para>OPEN READ LINEAR COMMAND (INTERNAL COMMAND)</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 12.</para>
        /// </remarks>
        /// <param name="fileName">The file name</param>
        /// <returns>Pointer to linear memory segment</returns>
        public UInt32? OpenReadLinear(string fileName)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.OpenReadLinear;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);

            byte[] reply = Send(request);

            if (reply == null) return null;

            UInt32 pointer = Util.GetUInt32(reply, 3);
            return pointer;
        }

        /// <summary>
        /// <para>OPEN WRITE DATA COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 12.</para>
        /// </remarks>
        /// <param name="fileName">The file name</param>
        /// <param name="fileSize">The file size</param>
        /// <returns>A handle to the file</returns>
        public byte? OpenWriteData(string fileName, UInt32 fileSize)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[26];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.OpenWriteData;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);  // NOTE: I'm not sure if the documentation is 100%
            Util.SetUInt32(fileSize, request, 22);  // ... correct here, since it do not allow space for the null terminator.

            byte[] reply = Send(request);

            if (reply == null) return null;

            byte handle = reply[3];
            return handle;
        }

        /// <summary>
        /// <para>OPEN APPEND DATA COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 13.</para>
        /// </remarks>
        /// <param name="fileName">The file name</param>
        /// <returns>A NxtOpenAppendDataReply withe the parsed reply</returns>
        public NxtOpenAppendDataReply? OpenAppendData(string fileName)
        {
            ValidateFilename(fileName);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.OpenAppendData;
            Encoding.ASCII.GetBytes(fileName).CopyTo(request, 2);

            byte[] reply = Send(request);

            if (reply == null) return null;

            NxtOpenAppendDataReply result;
            result.handle = reply[3];
            result.availableFilesize = Util.GetUInt32(reply, 4);
            return result;
        }

        /// <summary>
        /// <para>REQUEST FIRST MODULE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 19.</para>
        /// </remarks>
        /// <param name="resourceName">Modulename or -mask to search</param>
        /// <returns>A NxtRequestModuleReply containing the result for the search</returns>
        /// <seealso cref="RequestNextModule"/>
        /// <seealso cref="CloseModuleHandle"/>
        public NxtRequestModuleReply? RequestFirstModule(string resourceName)
        {
            ValidateFilename(resourceName);

            byte[] request = new byte[22];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.RequestFirstModule;
            Encoding.ASCII.GetBytes(resourceName).CopyTo(request, 2);

            return SendAndParseNxtRequestModuleReply(request);
        }

        /// <summary>
        /// <para>REQUEST NEXT MODULE</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 19.</para>
        /// </remarks>
        /// <param name="handle">Handle number from the previous Request Next Module command or from the very first Request First Module command</param>
        /// <returns>A NxtRequestModuleReply containing the result for the search</returns>
        /// <seealso cref="RequestFirstModule"/>
        /// <seealso cref="CloseModuleHandle"/>
        public NxtRequestModuleReply? RequestNextModule(byte handle)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.RequestNextModule,
                handle
            };

            return SendAndParseNxtRequestModuleReply(request);
        }

        /// <summary>
        /// <para>Sends a RequestFirstModule- or RequestNextModule-request and parses the result as a NxtRequestModule.</para>
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>A NxtRequestModuleReply containing the parsed result</returns>
        /// <seealso cref="RequestFirstModule"/>
        /// <seealso cref="RequestNextModule"/>
        private NxtRequestModuleReply? SendAndParseNxtRequestModuleReply(byte[] request)
        {
            byte[] reply;
            NxtRequestModuleReply result;

            try
            {
                reply = Send(request);

                if (reply == null) return null;
            }
            catch (NxtCommunicationProtocolException ex)
            {
                if (ex.errorMessage == NxtErrorMessage.NoMoreHandles || ex.errorMessage == NxtErrorMessage.ModuleNotFound)
                {
                    result.ModuleFound = false;
                    result.handle = 0;
                    result.moduleName = "";
                    result.moduleId = 0;
                    result.moduleIdCC = 0;
                    result.moduleIdFF = 0;
                    result.moduleIdPP = 0;
                    result.moduleIdTT = 0;
                    result.moduleSize = 0;
                    result.moduleIoMapSize = 0;
                    return result;
                }

                // Rethrow if not a NoMoreHandles- or a ModuleNotFound error.
                throw;
            }

            result.ModuleFound = true;
            result.handle = reply[3];
            result.moduleName = Encoding.ASCII.GetString(reply, 4, 20).TrimEnd('\0');
            result.moduleId = Util.GetUInt32(reply, 24);
            // NOTE: There seems to be some inconsistency in the documentation about PP TT CC FF, Appendix 1, p. 18.
            result.moduleIdFF = 0; // reply[24];
            result.moduleIdCC = 0; // reply[25];
            result.moduleIdTT = reply[26]; // reply[26];
            result.moduleIdPP = reply[24]; // reply[27]
            result.moduleSize = Util.GetUInt32(reply, 28);
            result.moduleIoMapSize = Util.GetUInt16(reply, 32);
            return result;
        }

        /// <summary>
        /// <para>CLOSE MODULE HANDLE COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 20.</para>
        /// </remarks>
        /// <param name="handle">Handle number</param>
        /// <seealso cref="RequestFirstModule"/>
        /// <seealso cref="RequestNextModule"/>
        public void CloseModuleHandle(byte handle)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.CloseModuleHandle,
                handle
            };

            Send(request);
        }

        /// <summary>
        /// <para>READ IO MAP COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 20.</para>
        /// </remarks>
        /// <param name="moduleId">The module ID</param>
        /// <param name="offset">The offset to read from</param>
        /// <param name="bytesToRead">Number of bytes to be read</param>
        /// <returns>IO-map content</returns>
        public byte[] ReadIoMap(UInt32 moduleId, UInt16 offset, UInt16 bytesToRead)
        {
            byte[] request = new byte[10];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.ReadIoMap;
            Util.SetUInt32(moduleId, request, 2);
            Util.SetUInt16(offset, request, 6);
            Util.SetUInt16(bytesToRead, request, 8);

            byte[] reply = Send(request);

            if (reply == null) return null;

            UInt32 moduleIdOut = Util.GetUInt32(reply, 3);
            if (moduleIdOut != moduleId)
                throw new NxtException(string.Format("Output module Id, {0}, was different from input module Id, {1}.", moduleIdOut, moduleId));

            UInt16 bytesRead = Util.GetUInt16(reply, 7);

            byte[] ioMapContent = new byte[bytesRead];
            Array.Copy(reply, 9, ioMapContent, 0, bytesRead);

            return ioMapContent;
        }

        /// <summary>
        /// <para>WRITE IO MAP COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 21.</para>
        /// </remarks>
        /// <param name="moduleId">The module ID</param>
        /// <param name="offset">The offset to write to</param>
        /// <param name="ioMapContent">IO-map content to be stored in IO-map[index]...IO-map[index+N]</param>
        /// <returns>The number of data that have been written</returns>
        public UInt16? WriteIoMap(UInt32 moduleId, UInt16 offset, byte[] ioMapContent)
        {
            UInt16 bytesToWrite = (UInt16)ioMapContent.Length;

            byte[] request = new byte[10 + ioMapContent.Length];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.WriteIoMap;
            Util.SetUInt32(moduleId, request, 2);
            Util.SetUInt16(offset, request, 6);
            Util.SetUInt16(bytesToWrite, request, 8);
            ioMapContent.CopyTo(request, 10);

            byte[] reply = Send(request);

            if (reply == null) return null;

            UInt32 moduleIdOut = Util.GetUInt32(reply, 3);
            if (moduleIdOut != moduleId)
                throw new NxtException(string.Format("Output module Id, {0}, was different from input module Id, {1}.", moduleIdOut, moduleId));

            UInt16 bytesWritten = Util.GetUInt16(reply, 7);
            return bytesWritten;
        }

        /// <summary>
        /// <para>BOOT COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 13.</para>
        /// <para>This command can only be accepted by USB.</para>
        /// </remarks>
        public void Boot()
        {
            byte[] request = new byte[21];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.Boot;
            Encoding.ASCII.GetBytes("Let's dance: SAMBA").CopyTo(request, 2);

            byte[] reply = Send(request);

            if (reply == null) return;

            string result = Encoding.ASCII.GetString(reply, 3, 4).TrimEnd('\0');
            if (result != "Yes")
                throw new NxtException("The reply, '{0}', was not the expected, 'Yes'.");
        }

        /// <summary>
        /// <para>SET BRICK NAME COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 13.</para>
        /// <para>For some reason only the first 8 characters is remembered when the NXT is turned off. This is with version 1.4 of the firmware, and it may be fixed with newer versions.</para>
        /// </remarks>
        /// <param name="brickName">The new name of the NXT brick</param>
        public void SetBrickName(string brickName)
        {
            if (brickName.Length > 15)
                brickName = brickName.Substring(0, 15);

            byte[] request = new byte[18];
            request[0] = 0x01;
            request[1] = (byte)NxtCommand.SetBrickName;
            Encoding.ASCII.GetBytes(brickName).CopyTo(request, 2);

            Send(request);
        }

        /// <summary>
        /// <para>GET DEVICE INFO</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 14.</para>
        /// <para>For some reason only the first 8 characters of the BXT name is remembered when the NXT is turned off. This is with version 1.4 of the firmware, and it may be fixed with newer versions.</para>
        /// </remarks>
        /// <returns>A NxtGetDeviceInfoReply containing the parsed result</returns>
        public NxtGetDeviceInfoReply? GetDeviceInfo()
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.GetDeviceInfo
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            NxtGetDeviceInfoReply result;

            result.nxtName = Encoding.ASCII.GetString(reply, 3, 15).TrimEnd('\0');

            result.btAdress = new byte[7];
            Array.Copy(reply, 18, result.btAdress, 0, 7);

            result.bluetoothSignalStrength = Util.GetUInt32(reply, 25);

            result.freeUserFlash = Util.GetUInt32(reply, 29);

            return result;
        }

        /// <summary>
        /// <para>DELETE USER FLASH</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 14.</para>
        /// </remarks>
        public void DeleteUserFlash()
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.DeleteUserFlash
            };

            Send(request);
        }

        /// <summary>
        /// <para>POLL COMMAND LENGTH</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 15.</para>
        /// </remarks>
        /// <param name="bufferNo">Buffer Number: 0x00 = Poll Buffer, 0x01 = High Speed buffer</param>
        /// <returns>Number of bytes for the command ready in the buffer</returns>
        public byte? PollCommandLength(byte bufferNo)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.PollCommandLength,
                bufferNo
            };

            byte[] reply = Send(request);

            if (reply == null) return null;

            // NOTE: My guess is that the documentation has switched meaning between bytes 2 and 3.
            byte bufferNoOut = reply[3];
            if (bufferNoOut != bufferNo)
                throw new NxtException(string.Format("Output buffer no-, {0}, was different from input buffer no., {1}.", bufferNoOut, bufferNo));

            byte bytesReady = reply[4];
            return bytesReady;
        }

        /// <summary>
        /// <para>POLL COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 15.</para>
        /// </remarks>
        /// <param name="bufferNo">Buffer Number: 0x00 = Poll Buffer, 0x01 = High Speed buffer</param>
        /// <param name="commandLength">Command length</param>
        public void PollCommand(byte bufferNo, byte commandLength)
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.PollCommand,
                bufferNo,
                commandLength
            };

            byte[] reply = Send(request);

            if (reply == null) return;

            // NOTE: My guess is that the documentathion has switched meaning between bytes 2 and 3.
            byte bufferNoOut = reply[3];
            if (bufferNoOut != bufferNo)
                throw new NxtException(string.Format("Output buffer no-, {0}, was different from input buffer no., {1}.", bufferNoOut, bufferNo));

            // TODO: Parse the reply, and return the result.
        }

        /// <summary>
        /// <para>BLUETOOTH FACTORY RESET COMMAND</para>
        /// </summary>
        /// <remarks>
        /// <para>Reference: BDK, Appendix 1, p. 15.</para>
        /// <para>This command cannot be transmitted via Bluetooth because all Bluetooth functionality is reset by this command!</para>
        /// </remarks>
        public void BluetoothFactoryReset()
        {
            byte[] request = new byte[] {
                0x01,
                (byte) NxtCommand.BluetoothFactoryReset
            };

            Send(request);
        }

        #endregion

        #region Utilities.

        /// <summary>
        /// <para>This function validates that the filename is correct for the NXT brick.</para>
        /// </summary>
        /// <param name="fileName">The file name</param>
        private void ValidateFilename(string fileName)
        {
            if (fileName.Length > 19)
                throw new ArgumentException("File name is to long. Maximum length is 19 characters (15.3).");
        }

        #endregion
    }
}
