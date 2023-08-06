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
}