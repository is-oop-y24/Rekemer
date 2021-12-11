using System;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BackupsExtra.Algorithms.LogInterface;


namespace BackupsExtra.Save
{
    public static class SaveSystem
    {
        public static string PathOfSaving { get; set; }

        public static void Save(SaveData saveData)
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            using (FileStream fs = new FileStream(PathOfSaving, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, saveData);
                string message = SaveDataIsCreated();
                Log.Instance.Log(message);
            }
        }

        private static string SaveDataIsCreated()
        {
            string message = "";
            message += $"saveData is succesfully created in {PathOfSaving}";
            return message;
        }

        public static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();
            Vector3SerializationSurrogate surrogate = new Vector3SerializationSurrogate();
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), surrogate);
            formatter.SurrogateSelector = selector;
            return formatter;
        }

        public static SaveData Load()
        {
            if (File.Exists(PathOfSaving))
            {
                BinaryFormatter formatter = GetBinaryFormatter();

                FileStream stream = File.Open(PathOfSaving, FileMode.Open);
                //var saveData1 = formatter.Deserialize(stream) as Job;
                var saveData = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                return saveData;
            }
            else
            {
                throw new Exception($"Loading is failed, since file {PathOfSaving} doesnt exist");
            }
        }
    }
}