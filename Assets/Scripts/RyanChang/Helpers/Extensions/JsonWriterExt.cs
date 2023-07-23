using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Contains methods pertaining to Newtonsoft's JsonWriter.
/// </summary>
public static class JsonWriterExt
{
    /// <summary>
    /// Writes the property name followed by the value of the property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="writer">The JsonWriter.</param>
    /// <param name="name">Name of the property used to serialize it.</param>
    /// <param name="property">The property to write.</param>
    public static void WriteProperty<T>(this JsonWriter writer, string name,
        T property)
    {
        writer.WritePropertyName(name);
        writer.WriteValue(property);
    }

    /// <summary>
    /// Writes a raw value with the correct indentation. Adapted from <see
    /// href="https://stackoverflow.com/a/67360891"/>.
    /// </summary>
    /// <param name="writer">The JsonWriter.</param>
    /// <param name="jsonString">The json string.</param>
    /// <param name="dateParseHandling">How to handle dates? It's recommended
    /// that it be set to none so that dates aren't confused with
    /// numbers.</param>
    /// <param name="floatParseHandling">How to handle floats?</param>
    public static void WriteFormattedRawValue(this JsonWriter writer,
        string jsonString,
        DateParseHandling dateParseHandling = DateParseHandling.None,
        FloatParseHandling floatParseHandling = default)
    {
        if (jsonString == null)
        {
            writer.WriteRawValue(jsonString);
        }
        else
        {
            using (var reader = new JsonTextReader(new StringReader(jsonString))
            {
                DateParseHandling = dateParseHandling,
                FloatParseHandling = floatParseHandling
            })
            {
                writer.WriteToken(reader);
            }
        }
    }
}