using System;

namespace WDClient
{
    /// <summary>
    /// This class defines the type of instruction sent between client and server.
    /// Functionality: It contains either Join, Leave, Command, or Joined commands.
    /// </summary>
    /// <remarks>Authors: Duy, Nadia, Joel (Client Team). Comments by Nadia and Rosanna.</remarks>
    /// <remarks>Updated by: NA</remarks>
    [Serializable()]
    public class Instruction
    {
        public enum Type { JOIN, LEAVE, CMD, JOINED };
        public Type Command { get; set; }
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
        public string Arg3 { get; set; }
        public string Arg4 { get; set; }
    }
}
