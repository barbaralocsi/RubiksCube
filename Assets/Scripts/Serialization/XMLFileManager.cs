using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class XMLFileManager
{
    
    /// <summary>
    /// Serializes the data in XML format to the given path
    /// </summary>
    public void Save<T>(string path, T data)
    {
        var serializer = new XmlSerializer(typeof(T));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Deserializes the binary file at the given path to type T
    /// </summary>
    public T Load<T>(string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            return (T)serializer.Deserialize(fileStream);
        }
    }

    public bool Exists(string path)
    {
        return File.Exists(path);
    }
}
