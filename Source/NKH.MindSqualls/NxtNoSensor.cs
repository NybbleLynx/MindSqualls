namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Class representing a port with no sensor attached.</para>
    /// </summary>
    public class NxtNoSensor : NxtSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        public NxtNoSensor()
            : base(NxtSensorType.NO_SENSOR, NxtSensorMode.RAWMODE)
        { }

        /// <summary>
        /// <para>This empty Pool-method effectively cancels polling on this port.</para>
        /// </summary>
        public override void Poll()
        { }
    }
}
