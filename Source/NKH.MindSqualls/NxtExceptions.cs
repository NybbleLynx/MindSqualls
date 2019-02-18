using System;

namespace NKH.MindSqualls
{
    /// <summary>
    /// <para>Generic NXT exception.</para>
    /// </summary>
    public class NxtException : ApplicationException
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="Message">The exception message.</param>
        public NxtException(string Message) : base(Message) { }
    }

    /// <summary>
    /// <para>This exception is thrown when there is no connection to the NXT brick.</para>
    /// </summary>
    public class NxtConnectionException : NxtException
    {
        /// <summary>
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="Message"></param>
        public NxtConnectionException(string Message)
            : base(Message)
        { }
    }

    /// <summary>
    /// <para>Exception thrown when there is an error in the communication to the NXT brick.</para>
    /// </summary>
    public class NxtCommunicationProtocolException : NxtException
    {
        /// <summary>
        /// <para>Constructor.</para>
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="errorMessage">The error code</param>
        /// <param name="Message">The exception message</param>
        public NxtCommunicationProtocolException(NxtCommand command, NxtErrorMessage errorMessage, string Message)
            : base(Message.Trim())
        {
            this.command = command;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// <para>The NxtCommand that resulted in the exception.</para>
        /// </summary>
        internal NxtCommand command;

        /// <summary>
        /// <para>The NxtErrorMessage that was returned from the NXT brick.</para>
        /// </summary>
        internal NxtErrorMessage errorMessage;

        /// <summary>
        /// <para>ToString() override.</para>
        /// </summary>
        /// <returns>... TBD ...</returns>
        public override string ToString()
        {
            return string.Format("NxtCommunicationProtocolException - Command: {0}; Error: {1}; {2}", command, errorMessage, Message);
        }
    }
}
