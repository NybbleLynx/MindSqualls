namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Abstract class representing a Active Sensor.</para>
    /// </summary>
    /// <remarks>
    /// <para>According to the LEGO MINDSTORMS NXT Hardware Developer Kit p. 7, sensors is divided into three types: active sensors (e.g. Robotics Invention Systems sensors), passive sensors (e.g. the NXT touch, light, and sound sensors), and digital sensors (e.g. the NXT ultrasonic sensor and the HiTechnic compass sensor). The three abstract classes NxtActiveSensor, NxtPassiveSensor, and NxtDigitalSensor reflect this.</para>
    /// </remarks>
    /// <seealso cref="NxtPassiveSensor"/>
    /// <seealso cref="NxtDigitalSensor"/>
    public abstract class NxtActiveSensor : NxtSensor
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="sensorType">Sensor type</param>
        /// <param name="sensorMode">Sensor mode</param>
        public NxtActiveSensor(NxtSensorType sensorType, NxtSensorMode sensorMode)
            : base(sensorType, sensorMode)
        {
        }
    }
}
