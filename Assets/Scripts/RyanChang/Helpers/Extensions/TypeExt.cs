using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Contains functions that extend types and reflection.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class TypeExt
{
    /// <summary>
    /// Returns all types that can be derived from <typeparamref name="T"/>
    /// based on the provided assembly.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="assembly">The assembly.</param>
    /// <returns></returns>
    public static IEnumerable<Type> FindAllDerivedTypes<T>(Assembly assembly)
    {
        var baseType = typeof(T);
        return assembly
            .GetTypes()
            .Where(t =>
                t != baseType &&
                baseType.IsAssignableFrom(t)
                );
    }

    /// <summary>
    /// Returns all types that can be derived from <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> FindAllDerivedTypes<T>() 
    {
        return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
    }

    /// <summary>
    /// Finds and returns the field value for <paramref name="obj"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The instance of the object.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="flags">Any flags.</param>
    /// <returns></returns>
    public static T GetFieldValue<T>(this object obj, string fieldName,
        BindingFlags flags)
    {
        var field = obj.GetType().GetField(fieldName);

        return (T)field.GetValue(obj);
    }

    public static bool TryGetFieldValue<T>(this object obj, string fieldName,
        BindingFlags flags, out T fieldValue)
    {
        var field = obj.GetType().GetField(fieldName, flags);

        if (field == null)
        {
            fieldValue = default;
            return false;
        }

        try
        {
            fieldValue = (T)field.GetValue(obj);
            return true;
        }
        catch (System.InvalidCastException)
        {
            fieldValue = default;
            return false;
        }
    }
}