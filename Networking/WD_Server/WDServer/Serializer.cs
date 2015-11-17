using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WDServer
{
    /// <summary>
    /// This class serializes Instructions that are passed between client and server.
    /// Functionality: Serialization and Deserialization of the Instructions.
    /// Note: Different than the client's serializer class (it uses different implementation of the JSON.)
    /// The Server uses .NET's implementation which is better and faster.
    /// </summary>
    /// <remarks>Authors: Jeff, Rosanna, Jens (Server team). Comments by Rosanna and Nadia.</remarks>
    /// <remarks>Updated by: NA</remarks>
    class Serializer
    {
        /// <summary>
        /// Converts Instruction to byte[].
        /// </summary>
        /// <param name="obj">The Instruction to be serialized.</param>
        /// <returns>A byte array of the Instruction.</returns>
        public static byte[] Serialize(Instruction obj)
        {
            return Encoding.Default.GetBytes(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Concerts byte[] to Instruction.
        /// </summary>
        /// <param name="arr">A byte array of the Instruction.</param>
        /// <returns>The Instruction to be serialized.</returns>
        public static Instruction DeSerialize(byte[] arr)
        {
            return JsonConvert.DeserializeObject<Instruction>(Encoding.Default.GetString(arr));
        }
    }
}

