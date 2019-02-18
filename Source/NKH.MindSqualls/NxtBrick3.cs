using System;
using System.Collections.Generic;

// The NxtBrick-class is spread over three files:
//
// NxtBrick1.cs - contains all the connectivity-related code.
// NxtBrick2.cs - contains all the sensor- and motor-related code.
// NxtBrick3.cs - contains all the general-purpose methods and properties of a NXT brick.

namespace NKH.MindSqualls
{
    public partial class NxtBrick
    {
        #region Files.

        /// <summary>
        /// <para>Lists files in the NXT brick according to the file mask.</para>
        /// </summary>
        /// <param name="fileMask">A fileMask</param>
        /// <returns>A list of file names</returns>
        private string[] FindFiles(string fileMask)
        {
            List<string> fileArr = new List<string>();

            NxtFindFileReply? reply = CommLink.FindFirst(fileMask);
            while (reply.HasValue && reply.Value.fileFound)
            {
                fileArr.Add(reply.Value.fileName);
                reply = CommLink.FindNext(reply.Value.handle);
            }

            return fileArr.ToArray();
        }

        #endregion

        #region Programs.

        /// <summary>
        /// <para>This property returns the name of the currently running program, or null if no program is running.</para>
        /// <para>Set the property to a program name to run the program, or to null to stop any running program.</para>
        /// </summary>
        public string Program
        {
            get
            {
                try
                {
                    return CommLink.GetCurrentProgramName();
                }
                catch (NxtCommunicationProtocolException ex)
                {
                    if (ex.errorMessage == NxtErrorMessage.NoActiveProgram)
                        return null;
                    else
                        throw;
                }
            }
            set
            {
                string fileName = value ?? "";

                fileName = fileName.Trim();

                if (fileName != "")
                    CommLink.StartProgram(fileName);
                else
                    CommLink.StopProgram();
            }
        }

        /// <summary>
        /// <para>The programs currently stored in the NXT brick.</para>
        /// </summary>
        public string[] Programs
        {
            get { return FindFiles("*.rxe"); }
        }

        #endregion

        #region Sounds and tones.

        /// <summary>
        /// <para>The sounds currently stored in the NXT brick.</para>
        /// </summary>
        public string[] Sounds
        {
            get { return FindFiles("*.rso"); }
        }

        /// <summary>
        /// <para>Plays a sound file stored in the NXT brick.</para>
        /// </summary>
        /// <param name="soundFile">The sound file</param>
        public void PlaySoundfile(string soundFile)
        {
            CommLink.PlaySoundfile(false, soundFile);
        }

        /// <summary>
        /// <para>Stops all playing sound; sound files and tones.</para>
        /// </summary>
        public void StopSound()
        {
            CommLink.StopSoundPlayback();
        }

        /// <summary>
        /// <para>Plays a tone.</para>
        /// </summary>
        /// <param name="frequency">Frequency for the tone, Hz</param>
        /// <param name="duration">Duration of the tone, ms</param>
        public void PlayTone(UInt16 frequency, UInt16 duration)
        {
            CommLink.PlayTone(frequency, duration);
        }

        #endregion

        #region Miscellaneous commands.

        /// <summary>
        /// <para>The name of the NXT brick.</para>
        /// </summary>
        /// <remarks>
        /// <para>For some reason only the first 8 characters is remembered when the NXT is turned off. This is with version 1.4 of the firmware, and it may be fixed with newer versions.</para>
        /// </remarks>
        public string Name
        {
            get
            {
                NxtGetDeviceInfoReply? reply = CommLink.GetDeviceInfo();
                if (reply.HasValue)
                    return reply.Value.nxtName;
                else
                    return null;
            }
            set { CommLink.SetBrickName(value); }
        }

        /// <summary>
        /// <para>The battery level of the NXT brick in millivolts.</para>
        /// </summary>
        public UInt16 BatteryLevel
        {
            get
            {
                ushort? reply = CommLink.GetBatteryLevel();
                if (reply.HasValue)
                    return reply.Value;
                else
                    return 0;
            }
        }

        #endregion
    }
}
