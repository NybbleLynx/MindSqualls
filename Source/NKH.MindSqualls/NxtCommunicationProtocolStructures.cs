using System;

namespace NKH.MindSqualls
{
    #region Reply types.

    /// <summary>
    /// <para>Reply type for the FindFirst() and FindNext() functions.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.FindFirst"/>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.FindNext"/>
    public struct NxtFindFileReply
    {
        /// <summary>
        /// <para>Boolean indicating if a file was found or not.</para>
        /// </summary>
        public bool fileFound;

        /// <summary>
        /// <para>The file handle.</para>
        /// </summary>
        public byte handle;

        /// <summary>
        /// <para>The file name.</para>
        /// </summary>
        public string fileName;

        /// <summary>
        /// <para>The filesize.</para>
        /// </summary>
        public UInt32 fileSize;
    }

    /// <summary>
    /// <para>Reply type for the GetDeviceInfo() function.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.GetDeviceInfo"/>
    public struct NxtGetDeviceInfoReply
    {
        /// <summary>
        /// <para>The name of the NXT brick.</para>
        /// </summary>
        public string nxtName;

        /// <summary>
        /// <para>The bluetooth address.</para>
        /// </summary>
        public byte[] btAdress;

        /// <summary>
        /// <para>The bluetooth signal strength.</para>
        /// </summary>
        public UInt32 bluetoothSignalStrength;

        /// <summary>
        /// <para>The size of the free user flash.</para>
        /// </summary>
        public UInt32 freeUserFlash;
    }

    /// <summary>
    /// <para>Reply type for the GetFirmwareVersion() function.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.GetFirmwareVersion"/>
    public struct NxtGetFirmwareVersionReply
    {
        /// <summary>
        /// <para>The protocol version.</para>
        /// </summary>
        public Version protocolVersion;

        /// <summary>
        /// <para>The firmware version.</para>
        /// </summary>
        public Version firmwareVersion;
    }

    /// <summary>
    /// <para>Reply type for the GetInputValues() function.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.GetInputValues"/>
    public struct NxtGetInputValuesReply
    {
        /// <summary>
        /// <para>A boolean indicating if the returned result is valid or not.</para>
        /// </summary>
        public bool valid;

        /// <summary>
        /// <para>A boolean indicating if the returned result is calibrated or not.</para>
        /// </summary>
        public bool calibrated;

        /// <summary>
        /// <para>The sensor type.</para>
        /// </summary>
        public NxtSensorType sensorType;

        /// <summary>
        /// <para>The sensor mode.</para>
        /// </summary>
        public NxtSensorMode sensorMode;

        /// <summary>
        /// <para>The raw A/D value.</para>
        /// </summary>
        public UInt16 rawAdValue;

        /// <summary>
        /// <para>The normalized A/D value.</para>
        /// </summary>
        public UInt16 normalizedAdValue;

        /// <summary>
        /// <para>The scaled value.</para>
        /// </summary>
        public Int16 scaledValue;

        /// <summary>
        /// <para>The calibrated value.</para>
        /// </summary>
        public Int16 calibratedValue;
    }

    /// <summary>
    /// <para>Reply type for the GetOutputState() function.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.GetOutputState"/>
    public struct NxtGetOutputStateReply
    {
        /// <summary>
        /// <para>The motor power.</para>
        /// </summary>
        public sbyte power;

        /// <summary>
        /// <para>The motor mode.</para>
        /// </summary>
        public NxtMotorMode mode;

        /// <summary>
        /// <para>The regulation mode.</para>
        /// </summary>
        public NxtMotorRegulationMode regulationMode;

        /// <summary>
        /// <para>The turn ratio.</para>
        /// </summary>
        public sbyte turnRatio;

        /// <summary>
        /// <para>The run state.</para>
        /// </summary>
        public NxtMotorRunState runState;

        /// <summary>
        /// <para>The tacho limit in degrees, 0 means unlimited.</para>
        /// </summary>
        public UInt32 tachoLimit;

        /// <summary>
        /// <para>The tacho count.</para>
        /// </summary>
        public Int32 tachoCount;

        /// <summary>
        /// <para>The block tacho count.</para>
        /// </summary>
        public Int32 blockTachoCount;

        /// <summary>
        /// <para>The rotation count.</para>
        /// </summary>
        public Int32 rotationCount;

        /// <summary>
        /// <para>ToString()-override.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public override string ToString()
        {
            return string.Format("[P:{0,4}|M:{1,25}|RM:{2,27}|TR:{3,4}|RS:{4,24}|TL:{5,3}|TC:{6,4}|BTC:{7,4}|RC:{8,4}]",
                power,
                mode,
                regulationMode,
                turnRatio,
                runState,
                tachoLimit,
                tachoCount,
                blockTachoCount,
                rotationCount);
        }
    }

    /// <summary>
    /// <para>Reply type for the OpenAppendData() function.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.OpenAppendData"/>
    public struct NxtOpenAppendDataReply
    {
        /// <summary>
        /// <para>File handle.</para>
        /// </summary>
        public byte handle;

        /// <summary>
        /// <para>Available file size.</para>
        /// </summary>
        public UInt32 availableFilesize;
    }

    /// <summary>
    /// <para>Reply type for the OpenRead() function.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.OpenRead"/>
    public struct NxtOpenReadReply
    {
        /// <summary>
        /// <para>File handle.</para>
        /// </summary>
        public byte handle;

        /// <summary>
        /// <para>File size.</para>
        /// </summary>
        public UInt32 fileSize;
    }

    /// <summary>
    /// <para>Reply type for the RequestFirstModule() and RequestNextModule() functions.</para>
    /// </summary>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.RequestFirstModule"/>
    /// <seealso cref="M:NKH.MindSqualls.NxtCommunicationProtocol.RequestNextModule"/>
    public struct NxtRequestModuleReply
    {
        /// <summary>
        /// <para>Boolean indicating if a module was found, or not.</para>
        /// </summary>
        public bool ModuleFound;

        /// <summary>
        /// <para>Module handle.</para>
        /// </summary>
        public byte handle;

        /// <summary>
        /// <para>Module name.</para>
        /// </summary>
        public string moduleName;

        /// <summary>
        /// <para>Module ID.</para>
        /// </summary>
        public UInt32 moduleId;

        /// <summary>
        /// <para>Module ID, PP-value.</para>
        /// </summary>
        public byte moduleIdPP;

        /// <summary>
        /// <para>Module ID, TT-value.</para>
        /// </summary>
        public byte moduleIdTT;

        /// <summary>
        /// <para>Module ID, CC-value.</para>
        /// </summary>
        public byte moduleIdCC;

        /// <summary>
        /// <para>Module ID, FF-value.</para>
        /// </summary>
        public byte moduleIdFF;

        /// <summary>
        /// <para>Module size.</para>
        /// </summary>
        public UInt32 moduleSize;

        /// <summary>
        /// <para>Module IO-map size.</para>
        /// </summary>
        public UInt16 moduleIoMapSize;
    }

    #endregion

    #region Enumerations.

    /// <summary>
    /// <para>Commands to the NXT brick.</para>
    /// </summary>
    /// <remarks>
    /// <para>Reference: BDK, Appendix 1 &amp; 2.</para>
    /// </remarks>
    public enum NxtCommand : byte
    {
        /// <summary>
        /// <para>BDK, Appendix 2, p. 5.</para>
        /// </summary>
        StartProgram = 0x00,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 5.</para>
        /// </summary>
        StopProgram = 0x01,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 5.</para>
        /// </summary>
        PlaySoundfile = 0x02,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 6.</para>
        /// </summary>
        PlayTone = 0x03,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 6.</para>
        /// </summary>
        SetOutputState = 0x04,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 7.</para>
        /// </summary>
        SetInputMode = 0x05,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 8.</para>
        /// </summary>
        GetOutputState = 0x06,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 8.</para>
        /// </summary>
        GetInputValues = 0x07,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 8.</para>
        /// </summary>
        ResetInputScaledValue = 0x08,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 9.</para>
        /// </summary>
        MessageWrite = 0x09,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 9.</para>
        /// </summary>
        ResetMotorPosition = 0x0A,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 9.</para>
        /// </summary>
        GetBatteryLevel = 0x0B,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 9.</para>
        /// </summary>
        StopSoundPlayback = 0x0C,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 10.</para>
        /// </summary>
        KeepAlive = 0x0D,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 10.</para>
        /// </summary>
        LsGetStatus = 0x0E,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 10.</para>
        /// </summary>
        LsWrite = 0x0F,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 10.</para>
        /// </summary>
        LsRead = 0x10,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 11.</para>
        /// </summary>
        GetCurrentProgramName = 0x11,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 11.</para>
        /// </summary>
        MessageRead = 0x13,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 7.</para>
        /// </summary>
        OpenRead = 0x80,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 7.</para>
        /// </summary>
        OpenWrite = 0x81,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 8.</para>
        /// </summary>
        Read = 0x82,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 8.</para>
        /// </summary>
        Write = 0x83,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 8.</para>
        /// </summary>
        Close = 0x84,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 9.</para>
        /// </summary>
        Delete = 0x85,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 9.</para>
        /// </summary>
        FindFirst = 0x86,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 10.</para>
        /// </summary>
        FindNext = 0x87,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 11.</para>
        /// </summary>
        GetFirmwareVersion = 0x88,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 11.</para>
        /// </summary>
        OpenWriteLinear = 0x89,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 12.</para>
        /// </summary>
        OpenReadLinear = 0x8A,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 12.</para>
        /// </summary>
        OpenWriteData = 0x8B,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 13.</para>
        /// </summary>
        OpenAppendData = 0x8C,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 19.</para>
        /// </summary>
        RequestFirstModule = 0x90,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 19.</para>
        /// </summary>
        RequestNextModule = 0x91,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 20.</para>
        /// </summary>
        CloseModuleHandle = 0x92,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 20.</para>
        /// </summary>
        ReadIoMap = 0x94,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 21.</para>
        /// </summary>
        WriteIoMap = 0x95,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 13.</para>
        /// </summary>
        Boot = 0x97,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 13.</para>
        /// </summary>
        SetBrickName = 0x98,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 14.</para>
        /// </summary>
        GetDeviceInfo = 0x9B,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 14.</para>
        /// </summary>
        DeleteUserFlash = 0xA0,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 15.</para>
        /// </summary>
        PollCommandLength = 0xA1,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 15.</para>
        /// </summary>
        PollCommand = 0xA2,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 15.</para>
        /// </summary>
        BluetoothFactoryReset = 0xA4
    }

    /// <summary>
    /// <para>ERROR MESSAGE BACK TO THE HOST</para>
    /// </summary>
    /// <remarks>
    /// <para>Reference: BDK, Appendix 1, p. 16.<br/>
    /// Reference: BDK, Appendix 2, p. 12.</para>
    /// </remarks>
    public enum NxtErrorMessage : byte
    {
        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        Succes = 0x00,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        PendingCommunicationTransactionInProgress = 0x20,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        SpecifiedMailboxQueueIsEmpty = 0x40,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        NoMoreHandles = 0x81,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        NoSpace = 0x82,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        NoMoreFiles = 0x83,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        EndOfFileExpected = 0x84,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        EndOfFile = 0x85,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        NotALinearFile = 0x86,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        FileNotFound = 0x87,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        HandleAllReadyClosed = 0x88,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        NoLinearSpace = 0x89,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        UndefinedError = 0x8A,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        FileIsBusy = 0x8B,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        NoWriteBuffers = 0x8C,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        AppendNotPossible = 0x8D,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        FileIsFull = 0x8E,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        FileExists = 0x8F,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        ModuleNotFound = 0x90,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        OutIfBoundary = 0x91,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        IllegalFileName = 0x92,

        /// <summary>
        /// <para>BDK, Appendix 1, p. 16.</para>
        /// </summary>
        IllegalHandle = 0x93,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        RequestFailed = 0xBD, // i.e. specified file not found

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        UnknownCommandOpcode = 0xBE,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        InsanePacket = 0xBF,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        DataContainsOutOfRangeValues = 0xC0,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        CommunicationBusError = 0xDD,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        NoFreeMemoryInCommunicationBuffer = 0xDE,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        SpecifiedChannelOrConnectionIsNotValid = 0xDF,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        SpecifiedChannelOrConnectionNotConfiguredOrBusy = 0xE0,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        NoActiveProgram = 0xEC,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        IllegalSizeSpecified = 0xED,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        IllegalMailboxQueueIdSpecified = 0xEE,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        AttemptedToAccessInvalidFieldOfAStructure = 0xEF,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        BadInputOrOutputSpecified = 0xF0,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        InsufficientMemoryAvailable = 0xFB,

        /// <summary>
        /// <para>BDK, Appendix 2, p. 12.</para>
        /// </summary>
        BadArguments = 0xFF
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 6.</para>
    /// </summary>
    [Flags]
    public enum NxtMotorMode : byte
    {
        /// <summary>
        /// <para>Turn on the specified motor.</para>
        /// </summary>
        MOTORON = 0x01,

        /// <summary>
        /// <para>Use run/break instead of run/float in PWM.</para>
        /// </summary>
        BRAKE = 0x02,

        /// <summary>
        /// <para>Turns on the regulation.</para>
        /// </summary>
        REGULATED = 0x04
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 6.</para>
    /// </summary>
    /// <seealso cref="NxtSensorPort"/>
    public enum NxtMotorPort : byte
    {
        /// <summary>
        /// <para>Motor port A.</para>
        /// </summary>
        PortA,

        /// <summary>
        /// <para>Motor port B.</para>
        /// </summary>
        PortB,

        /// <summary>
        /// <para>Motor port C.</para>
        /// </summary>
        PortC,

        /// <summary>
        /// <para>All motor ports.</para>
        /// </summary>
        All = 0xFF
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 6.</para>
    /// </summary>
    public enum NxtMotorRegulationMode : byte
    {
        /// <summary>
        /// <para>No regulation will be enabled.</para>
        /// </summary>
        REGULATION_MODE_IDLE = 0x00,

        /// <summary>
        /// <para>Power control will be enabled on specified output.</para>
        /// </summary>
        REGULATION_MODE_MOTOR_SPEED = 0x01,

        /// <summary>
        /// <para>Syncronization will be enabled (Needs enabled on two output).</para>
        /// </summary>
        REGULATION_MODE_MOTOR_SYNC = 0x02
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 6.</para>
    /// </summary>
    [Flags]
    public enum NxtMotorRunState : byte
    {
        /// <summary>
        /// <para>Output will be idle.</para>
        /// </summary>
        MOTOR_RUN_STATE_IDLE = 0x00,

        /// <summary>
        /// <para>Output will ramp-up.</para>
        /// </summary>
        MOTOR_RUN_STATE_RAMPUP = 0x10,

        /// <summary>
        /// <para>Output will be running.</para>
        /// </summary>
        MOTOR_RUN_STATE_RUNNING = 0x20,

        /// <summary>
        /// <para>Output will ramp-down.</para>
        /// </summary>
        MOTOR_RUN_STATE_RAMPDOWN = 0x40
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 7.</para>
    /// </summary>
    public enum NxtSensorMode : byte
    {
        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        RAWMODE = 0x00,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        BOOLEANMODE = 0x20,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        TRANSITIONCNTMODE = 0x40,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        PERIODCOUNTERMODE = 0x60,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        PCTFULLSCALEMODE = 0x80,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        CELSIUSMODE = 0xA0,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        FAHRENHEITMODE = 0xC0,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        ANGLESTEPMODE = 0xE0
        // SLOPEMASK = 0x1F (?)
        // MODEMASK = 0xE0 (?) 
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 7.</para>
    /// </summary>
    /// <seealso cref="NxtMotorPort"/>
    public enum NxtSensorPort : byte
    {
        /// <summary>
        /// <para>Sensor port 1.</para>
        /// </summary>
        Port1,

        /// <summary>
        /// <para>Sensor port 2.</para>
        /// </summary>
        Port2,

        /// <summary>
        /// <para>Sensor port 3.</para>
        /// </summary>
        Port3,

        /// <summary>
        /// <para>Sensor port 4.</para>
        /// </summary>
        Port4
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 7.</para>
    /// </summary>
    /// <remarks>
    /// <para>Explantion of the values is found on p. 46 of the Executable File Specification document.</para>
    /// </remarks>
    public enum NxtSensorType : byte
    {
        /// <summary>
        /// <para>No sensor configured.</para>
        /// </summary>
        NO_SENSOR = 0x00,

        /// <summary>
        /// <para>NXT or RCX touch sensor.</para>
        /// </summary>
        SWITCH = 0x01,

        /// <summary>
        /// <para>RCX temperature sensor.</para>
        /// </summary>
        TEMPERATURE = 0x02,

        /// <summary>
        /// <para>RCX light sensor.</para>
        /// </summary>
        REFLECTION = 0x03,

        /// <summary>
        /// <para>RCX rotation sensor.</para>
        /// </summary>
        ANGLE = 0x04,

        /// <summary>
        /// <para>NXT light sensor with floodlight enabled.</para>
        /// </summary>
        LIGHT_ACTIVE = 0x05,

        /// <summary>
        /// <para>NXT light sensor with floodlight disabled.</para>
        /// </summary>
        LIGHT_INACTIVE = 0x06,

        /// <summary>
        /// <para>NXT sound sensor; dB scaling.</para>
        /// </summary>
        SOUND_DB = 0x07,

        /// <summary>
        /// <para>NXT sound sensor; dBA scaling.</para>
        /// </summary>
        SOUND_DBA = 0x08,

        /// <summary>
        /// <para>Unused in NXT programs.</para>
        /// </summary>
        CUSTOM = 0x09,

        /// <summary>
        /// <para>I<sup>2</sup>C digital sensor.</para>
        /// </summary>
        LOWSPEED = 0x0A,

        /// <summary>
        /// <para>I<sup>2</sup>C digital sensor; 9V power.</para>
        /// </summary>
        LOWSPEED_9V = 0x0B,

        /// <summary>
        /// <para>Unused in NXT programs.</para>
        /// </summary>
        HIGHSPEED = 0x0C,

        #region NXT 2.0 sensor types

        /// <summary>
        /// <para>NXT color sensor in color detector mode.</para>
        /// </summary>
        /// <remarks>
        /// <para>NXT 2.0 only.</para>
        /// </remarks>
        COLORFULL = 0x0D,

        /// <summary>
        /// <para>NXT color sensor in light sensor mode with red light.</para>
        /// </summary>
        /// <remarks>
        /// <para>NXT 2.0 only.</para>
        /// </remarks>
        COLORRED = 0x0E,

        /// <summary>
        /// <para>NXT color sensor in light sensor mode with green light.</para>
        /// </summary>
        /// <remarks>
        /// <para>NXT 2.0 only.</para>
        /// </remarks>
        COLORGREEN = 0x0F,

        /// <summary>
        /// <para>NXT color sensor in light sensor mode with blue light.</para>
        /// </summary>
        /// <remarks>
        /// <para>NXT 2.0 only.</para>
        /// </remarks>
        COLORBLUE = 0x10,

        /// <summary>
        /// <para>NXT color sensor in light sensor mode with no light.</para>
        /// </summary>
        /// <remarks>
        /// <para>NXT 2.0 only.</para>
        /// </remarks>
        COLORNONE = 0x11

        #endregion
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 9.</para>
    /// </summary>
    public enum NxtMailbox : byte
    {
        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box0,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box1,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box2,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box3,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box4,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box5,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box6,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box7,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box8,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box9
    }

    /// <summary>
    /// <para>Reference: BDK, Appendix 2, p. 9.<br/>
    /// Reference: BDK, Appendix 2, p. 11. (see MessageRead() function).</para>
    /// </summary>
    public enum NxtMailbox2 : byte
    {
        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box0,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box1,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box2,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box3,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box4,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box5,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box6,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box7,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box8,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box9,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box10,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box11,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box12,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box13,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box14,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box15,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box16,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box17,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box18,

        /// <summary>
        /// <para>... TBD ...</para>
        /// </summary>
        Box19
    }

    #endregion
}
