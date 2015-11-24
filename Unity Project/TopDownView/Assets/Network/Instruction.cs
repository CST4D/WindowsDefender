using System;

namespace WDClient
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    public class Instruction
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Type { JOIN, LEAVE, CMD, JOINED };
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public Type Command { get; set; }
        /// <summary>
        /// Gets or sets the arg1.
        /// </summary>
        /// <value>
        /// The arg1.
        /// </value>
        public string Arg1 { get; set; }
        /// <summary>
        /// Gets or sets the arg2.
        /// </summary>
        /// <value>
        /// The arg2.
        /// </value>
        public string Arg2 { get; set; }
        /// <summary>
        /// Gets or sets the arg3.
        /// </summary>
        /// <value>
        /// The arg3.
        /// </value>
        public string Arg3 { get; set; }
        /// <summary>
        /// Gets or sets the arg4.
        /// </summary>
        /// <value>
        /// The arg4.
        /// </value>
        public string Arg4 { get; set; }
    }
}
