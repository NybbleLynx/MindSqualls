using System;
using System.Threading;

// The NxtBrick-class is spread over three files:
//
// NxtBrick1.cs - contains all the connectivity-related code.
// NxtBrick2.cs - contains all the sensor- and motor-related code.
// NxtBrick3.cs - contains all the general-purpose methods and properties of a NXT brick.

// TODO: Add support for an USB connection as an alternative to Bluetooth.

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Types of communication links usable for the NXT brick.</para>
    /// </summary>
    /// <remarks>
    /// <para>Introduced with v2.0 of the framework.</para>
    /// </remarks>
    public enum NxtCommLinkType
    {
        /// <summary>
        /// <para>Use Bluetooth for the communications link.</para>
        /// </summary>
        Bluetooth,

        /// <summary>
        /// <para>Use USB for the communications link.</para>
        /// </summary>
        USB
    }

    /// <summary>
    /// <para>Class representing the NXT brick.</para>
    /// </summary>    
    public partial class NxtBrick
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="serialPortNo">The COM port used by the Bluetooth link</param>
        [Obsolete("Use NxtBrick(NxtCommLinkType, byte) instead.")]
        public NxtBrick(byte serialPortNo)
            : this()
        {
            CommLink = new NxtBluetoothConnection(serialPortNo);
        }

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="serialPortName">The COM port used by the Bluetooth link</param>
        [Obsolete("Use NxtBrick(NxtCommLinkType, byte) instead.")]
        public NxtBrick(string serialPortName)
            : this()
        {
            CommLink = new NxtBluetoothConnection(serialPortName);
        }

        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <remarks>
        /// <para>Introduced with v2.0 of the framework.</para>
        /// </remarks>
        /// <param name="commLinkType">Indicates whether Bluetooth or USB should be used for the communication with the NXT brick.</param>
        /// <param name="serialPortNo">The COM port used. Only relevant for Bluetooth links.</param>
        public NxtBrick(NxtCommLinkType commLinkType, byte serialPortNo)
            : this()
        {
            switch (commLinkType)
            {
                case NxtCommLinkType.Bluetooth:
                    CommLink = new NxtBluetoothConnection(serialPortNo);
                    break;
                case NxtCommLinkType.USB:
                    CommLink = new NxtUsbConnection();
                    break;
                default:
                    throw new ArgumentException("Unknown commLinkType.");
            }
        }

        /// <summary>
        /// <para>Private constructor.</para>
        /// </summary>
        private NxtBrick()
        {
            // Set keep-alive heartbeat to 1 minute.
            uint keepAlivePeriod = 60 * 1000;

            // Start keep-alive heartbeat.
            keepAliveTimer = new Timer(keepAliveTimer_Callback, null, keepAlivePeriod, keepAlivePeriod);
        }

        #region Communication link to the NXT.

        private NxtCommunicationProtocol commLink;

        /// <summary>
        /// <para>The communication link to the NXT brick.</para>
        /// </summary>
        /// <remarks>
        /// <para>The current version of the API only supports bluetooth links. Possibly a future version will support USB as well.</para>
        /// </remarks>
        /// <exception cref="NxtConnectionException">Throws a NxtConnectionException if not connected.</exception>
        /// <seealso cref="Connect"/>
        /// <seealso cref="Disconnect"/>
        /// <seealso cref="IsConnected"/>
        public NxtCommunicationProtocol CommLink
        {
            get { return commLink; }
            private set { commLink = value; }
        }

        /// <summary>
        /// <para>Connect to the NXT brick.</para>
        /// </summary>
        /// <seealso cref="CommLink"/>
        /// <seealso cref="Disconnect"/>
        /// <seealso cref="IsConnected"/>
        public void Connect()
        {
            // Only if not already connected.
            if (!IsConnected)
                commLink.Connect();

            InitSensors();
            EnableAutoPoll();
        }

        /// <summary>
        /// <para>Disconnect from the NXT brick.</para>
        /// </summary>
        /// <seealso cref="CommLink"/>
        /// <seealso cref="Connect"/>
        /// <seealso cref="IsConnected"/>
        public void Disconnect()
        {
            DisableAutoPoll();

            // Disconnect, if connected.
            if (IsConnected)
                commLink.Disconnect();
        }

        /// <summary>
        /// <para>Indicates if the NXT brick is connected.</para>
        /// </summary>
        /// <seealso cref="CommLink"/>
        /// <seealso cref="Connect"/>
        /// <seealso cref="Disconnect"/>
        public bool IsConnected
        {
            get { return commLink.IsConnected; }
        }

        #endregion

        #region Keep-alive heartbeat (timer).

        private Timer keepAliveTimer = null;

        private void keepAliveTimer_Callback(object state)
        {
            if (IsConnected)
                CommLink.KeepAlive();
        }

        #endregion
    }
}
