﻿namespace WDClient
{
    /// <summary>
    /// 
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// Converts Instruction to byte[]
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static byte[] Serialize(Instruction obj)
        {
            return System.Text.Encoding.Default.GetBytes(
                Pathfinding.Serialization.JsonFx.JsonWriter.Serialize(obj)
            );
        }

        /// <summary>
        /// Concerts byte[] to Instruction
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <returns></returns>
        public static Instruction DeSerialize(byte[] arr)
        {
            return Pathfinding.Serialization.JsonFx.JsonReader.Deserialize<Instruction>(
                System.Text.Encoding.Default.GetString(arr)
            );
        }
    }
}
