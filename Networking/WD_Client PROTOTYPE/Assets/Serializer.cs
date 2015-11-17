namespace WDClient
{
    /// <summary>
    /// This class serializes Instructions that are passed between client and server.
    /// Functionality: Serialization and Deserialization of the Instructions.
    /// Note: Different than the server's serializer class (it uses different implementation of the JSON).
    /// The client uses Unity's version of a JSON serialization class because it cannot run the new
    /// one which is .NET 4.5 and Unity uses .NET 3.5.
    /// </summary>
    /// <remarks>Authors: Duy, Nadia, Joel (Client team). Comments by Rosanna and Nadia.</remarks>
    /// <remarks>Updated by: NA</remarks>
    public class Serializer
    {
        /// <summary>
        /// Converts Instruction to byte[].
        /// </summary>
        /// <param name="obj">The Instruction to be serialized.</param>
        /// <returns>A byte array of the Instruction.</returns>
        public static byte[] Serialize(Instruction obj)
        {
            return System.Text.Encoding.Default.GetBytes(
                Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(obj)
            );
        }

        /// <summary>
        /// Concerts byte[] to Instruction.
        /// </summary>
        /// <param name="arr">A byte array of the Instruction.</param>
        /// <returns>The Instruction to be serialized.</returns>
        public static Instruction DeSerialize(byte[] arr)
        {
            return Pathfinding.Serialization.JsonFx.JsonReader.Deserialize<Instruction>(
                System.Text.Encoding.Default.GetString(arr)
            );
        }
    }
}
