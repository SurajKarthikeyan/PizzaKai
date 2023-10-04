using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Contains methods that extend game objects or other unity objects.
/// </summary>
public static class UnityObjectExt
{
    #region Query
    #region Query Single

    #region In Self
    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this GameObject gameObject,
        bool includeInactive = false)
    {
        return !gameObject.GetComponent<T>().IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this Component self,
        bool includeInactive = false)
    {
        return self.gameObject.HasComponent<T>(includeInactive);
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this GameObject gameObject,
        out T component)
    {
        component = gameObject.GetComponent<T>();
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool HasComponent<T>(this Component self, out T component)
    {
        component = self.GetComponent<T>();
        return !component.IsNullOrUnityNull();
    }
    #endregion

    #region In Parent
    /// <summary>
    /// Checks if gameObject has the specified component in its parent.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// parent.</returns>
    public static bool HasComponentInParent<T>(this GameObject gameObject,
        bool includeInactive = false)
    {
        return !gameObject.GetComponentInParent<T>(includeInactive)
            .IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// children.</returns>
    public static bool HasComponentInParent<T>(this Component self,
        bool includeInactive = false)
    {
        return self.gameObject.HasComponentInParent<T>(includeInactive);
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its parent.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// parent.</returns>
    public static bool HasComponentInParent<T>(this GameObject gameObject,
        out T component, bool includeInactive = false)
    {
        component = gameObject.GetComponentInParent<T>(includeInactive);
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// children.</returns>
    public static bool HasComponentInParent<T>(this Component self,
        out T component, bool includeInactive = false)
    {
        return self.gameObject.HasComponentInParent<T>(out component);
    }
    #endregion

    #region In Children
    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// children.</returns>
    public static bool HasComponentInChildren<T>(this GameObject gameObject,
        bool includeInactive = false)
    {
        return !gameObject.GetComponentInChildren<T>(includeInactive)
            .IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// children.</returns>
    public static bool HasComponentInChildren<T>(this Component self,
        bool includeInactive = false)
    {
        return self.gameObject.HasComponentInChildren<T>(includeInactive);
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// children.</returns>
    public static bool HasComponentInChildren<T>(this GameObject gameObject,
        out T component, bool includeInactive = false)
    {
        component = gameObject.GetComponentInChildren<T>(includeInactive);
        return !component.IsNullOrUnityNull();
    }

    /// <summary>
    /// Checks if gameObject has the specified component in its children.
    /// </summary>
    /// <typeparam name="T">Type of the component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component in its
    /// children.</returns>
    public static bool HasComponentInChildren<T>(this Component self,
        out T component, bool includeInactive = false)
    {
        return self.gameObject.HasComponentInChildren<T>(out component,
            includeInactive);
    }
    #endregion

    #region In Scene
    /// <summary>
    /// Attempts to locate a gameobject with the specified tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="gameObject"></param>
    /// <returns>True if gameobject was found, false otherwise.</returns>
    public static bool ExistsWithTag(string tag, out GameObject gameObject)
    {
        try
        {
            gameObject = GameObject.FindWithTag(tag);

            return gameObject != null;
        }
        catch (UnityException e)
        {
            Debug.LogError($"Tag {tag} is not defined!");
            throw e;
        }
    }
    #endregion

    #endregion

    #region Query Multiple
    /// <summary>
    /// Returns an array of all objects with a certain type.
    /// </summary>
    /// <typeparam name="T">The component to look for.</typeparam>
    /// <param name="array">Array of GameObjects to look through.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>An array of GameObjects with only the type of components
    /// specified.</returns>
    public static IEnumerable<GameObject> WithComponent<T>(
        this IEnumerable<GameObject> array,
        bool includeInactive = false)
    {
        return array.Where(go => go.HasComponent<T>(includeInactive));
    }

    /// <summary>
    /// Returns an array of all objects with a certain tag.
    /// </summary>
    /// <param name="array">Array of GameObjects to look through.</param>
    /// <param name="tagName">Name of tag to search for.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>An array of GameObjects with only the tags.</returns>
    public static IEnumerable<GameObject> WithTag(
        this IEnumerable<GameObject> array,
        string tagName, bool includeInactive = false)
    {
        return array.Where(go => go.CompareTag(tagName));
    }
    #endregion
    #endregion

    #region Autoadd
    #region Add If Missing
    /// <returns>The component that was added.</returns>
    /// <inheritdoc cref="AddComponentIfMissing{T}(GameObject, out T)"/>
    public static T AddComponentIfMissing<T>(this GameObject gameObject)
        where T : Component
    {
        if (!gameObject.HasComponent<T>(out T component))
        {
            return gameObject.AddComponent<T>();
        }
        else
        {
            return component;
        }
    }

    /// <summary>
    /// Adds a component of type T if none are found in <paramref
    /// name="target"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target">The gameobject to add the component to.</param>
    /// <param name="component">Either the newly added component or an existing
    /// one.</param>
    /// <returns>True if the gameobject has the component, false if it had to be
    /// added.</returns>
    public static bool AddComponentIfMissing<T>(this GameObject target,
        out T component) where T : Component
    {
        T thing = target.GetComponent<T>();
        if (thing.IsNullOrUnityNull())
        {
            component = target.AddComponent<T>();
            return false;
        }
        else
        {
            component = thing;
            return true;
        }
    }
    #endregion

    #region Require Component
    #region In Self
    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">Component whose GameObject will be used to
    /// search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponent<T>(
        this Component self,
        out T component,
        bool doError = true)
    {
        return self.RequireComponent(out component, typeof(T).ToString(),
            doError);
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject used to search for the
    /// component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponent<T>(
        this GameObject gameObject,
        out T component,
        bool doError = true)
    {
        return gameObject.RequireComponent(out component, typeof(T).ToString(),
            doError);
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking
    /// for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponent<T>(
        this Component self,
        out T component,
        string name,
        bool doError = true)
    {
        return self.gameObject.RequireComponent(out component, name, doError);
    }

    /// <summary>
    /// Checks if gameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking
    /// for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponent<T>(
        this GameObject gameObject,
        out T component,
        string name,
        bool doError = true)
    {
        if (gameObject.HasComponent(out component))
        {
            return true;
        }
        else
        {
            string errorMessage =
                $"{gameObject} is missing required component {name}.";

            if (doError)
                Debug.LogError(errorMessage);
            else
                Debug.LogWarning(errorMessage);

            return false;
        }
    }
    #endregion

    #region In Children
    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">Component whose GameObject will be used to
    /// search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponentInChildren<T>(
        this Component self,
        out T component,
        bool doError = true,
        bool includeInactive = false)
    {
        return self.RequireComponentInChildren(out component,
            typeof(T).ToString(), doError, includeInactive);
    }

    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject used to search for the
    /// component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject has the specified component.</returns>
    public static bool RequireComponentInChildren<T>(
        this GameObject gameObject,
        out T component,
        bool doError = true,
        bool includeInactive = false)
    {
        return gameObject.RequireComponentInChildren(out component,
            typeof(T).ToString(), doError, includeInactive);
    }

    /// <summary>
    /// Checks if self's gameObject or its children has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking
    /// for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject or its children has the specified
    /// component.</returns>
    public static bool RequireComponentInChildren<T>(
        this Component self,
        out T component,
        string name,
        bool doError = true,
        bool includeInactive = false)
    {
        return self.gameObject.RequireComponentInChildren(out component,
            name, doError, includeInactive);
    }

    /// <summary>
    /// Checks if gameObject or its children has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">GameObject to search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking
    /// for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <param name="includeInactive">If true, include inactive GameObjects when
    /// searching. Otherwise, do not include them.</param>
    /// <returns>True if gameObject or its children has the specified
    /// component.</returns>
    public static bool RequireComponentInChildren<T>(
        this GameObject gameObject,
        out T component,
        string name,
        bool doError = true,
        bool includeInactive = false)
    {
        if (gameObject.HasComponentInChildren(out component, includeInactive))
        {
            return true;
        }
        else
        {
            string errorMessage =
                $"{gameObject} is missing required component {name} in children.";

            if (doError)
                Debug.LogError(errorMessage);
            else
                Debug.LogWarning(errorMessage);

            return false;
        }
    }
    #endregion
    #endregion

    #region Add To Manager Index
    /// <summary>
    /// If a MonoBehavior of type T already exists within managerCache, return
    /// that MonoBehavior. Otherwise, attempts to find the specified manager in
    /// self or its children. If found, instantiates a singleton for that
    /// manager. Unlike <see cref="InstantiateSingleton{T}(T, ref T, bool)"/>,
    /// this does not throw errors.
    /// </summary>
    ///
    /// <typeparam name="T">The type of manager. Must be a
    /// MonoBehaviour.</typeparam>
    /// <param name="self">The manager container who is the parent of
    /// manager.</param>
    /// <param name="manager">Will be set to the value of the manager.</param>
    /// <param name="managerCache">The cache to put the manager once it is
    /// instantiated, if dontDestroyOnLoad is true. <see
    /// cref="AddToManagerIndex{T}(MonoBehaviour, out T, ref Dictionary{Type,
    /// MonoBehaviour}, bool)"/> uses this cache to locate existing managers
    /// listed as DontDestroyOnLoad. It is recommended that this cache be static
    /// so it is not deleted on scene load.
    /// </param>
    /// <param name="dontDestroyOnLoad">If true, add manager to cache and set it
    /// to DontDestroyOnLoad.</param>
    public static void AddToManagerIndex<T>(this MonoBehaviour self,
        out T manager, ref Dictionary<Type, MonoBehaviour> managerCache,
        bool dontDestroyOnLoad) where T : MonoBehaviour
    {
        self.RequireComponentInChildren(out T tempManager);

        if (dontDestroyOnLoad)
        {
            managerCache ??= new();

            if (managerCache.ContainsKey(typeof(T)) &&
                managerCache[typeof(T)] != null)
            {
                // Set manager from the managerCache.
                manager = (T)managerCache[typeof(T)];

                if (tempManager != null)
                {
                    // However, we seem to already have another manager in self.
                    // Delete this manager.
                    Debug.LogWarning($"Found another instance of {typeof(T)}. " +
                        "Deleting...");

                    GameObject.Destroy(tempManager.gameObject);
                }
                return;
            }

            // We have a brand new manager.
            manager = tempManager;

            // Set as DoNotDestroyOnLoad
            manager.transform.Orphan();
            GameObject.DontDestroyOnLoad(manager.gameObject);

            // Now set the value in the cache to the new manager.
            managerCache[typeof(T)] = manager;
        }
        else
        {
            // Managers that are destroyed on load won't cause problems, as they
            // can't reload.
            manager = tempManager;
        }
    }
    #endregion

    #region Autofill
    /// <summary>
    /// Checks if self's GameObject has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="self">Component whose GameObject will be used to search for
    /// the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking
    /// for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <returns>True if gameObject has the specified component or if the
    /// component is already specified.</returns>
    public static bool AutofillComponent<T>(
        this Component self,
        ref T component,
        string name,
        bool doError = true
        ) where T : Component
    {
        if (component)
            return true;
        else
        {
            return self.RequireComponent(out component, name, doError);
        }
    }

    /// <summary>
    /// Checks if self has the specified component.
    /// </summary>
    /// <typeparam name="T">Type of component to get.</typeparam>
    /// <param name="gameObject">Component whose GameObject will be used to
    /// search for the component.</param>
    /// <param name="component">Set to the component if found.</param>
    /// <param name="name">Name of the component that we are looking
    /// for.</param>
    /// <param name="doError">If true, log as an error. Otherwise, log as a
    /// warning.</param>
    /// <returns>True if gameObject has the specified component or if the
    /// component is already specified.</returns>
    public static bool AutofillComponent<T>(
        this GameObject gameObject,
        ref T component,
        string name,
        bool doError = true
        ) where T : Component
    {
        if (component)
            return true;
        else
        {
            return gameObject.RequireComponent(out component, name, doError);
        }
    }

    /// <inheritdoc cref="AutofillComponent{T}(Component, ref T, string,
    /// bool)"/>
    public static bool AutofillComponent<T>(
        this Component self,
        ref T component,
        bool doError = true
        ) where T : Component
    {
        return self.AutofillComponent(
            ref component,
            typeof(T).ToString(),
            doError
        );
    }

    /// <inheritdoc cref="AutofillComponent{T}(GameObject, ref T, string,
    /// bool)"/>
    public static bool AutofillComponent<T>(
        this GameObject self,
        ref T component,
        bool doError = true
        ) where T : Component
    {
        return self.AutofillComponent(
            ref component,
            typeof(T).ToString(),
            doError
        );
    }
    #endregion
    #endregion

    #region Instantiation
    /// <summary>
    /// Instantiates the gameobject belonging to reference, then returns the
    /// instantiated reference.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reference">The prefab or other reference to instantiate.
    /// This will be unchanged.</param>
    /// <returns>The newly instantiated component of type <typeparamref
    /// name="T"/>.</returns>
    public static T InstantiateComponent<T>(this T reference) where T : Component
    {
        GameObject.Instantiate(reference.gameObject).RequireComponent(out T instance);
        return instance;
    }

    /// <inheritdoc cref="InstantiateComponent{T}(T)"/>
    /// <inheritdoc cref="UnityEngine.Object.Instantiate(UnityEngine.Object,
    /// Transform, bool)"/>
    public static T InstantiateComponent<T>(this T reference, Transform parent,
        bool instantiateInWorldSpace) where T : Component
    {
        GameObject.Instantiate(
            reference.gameObject,
            parent,
            instantiateInWorldSpace
        ).RequireComponent(out T instance);
        return instance;
    }

    /// <inheritdoc cref="InstantiateComponent{T}(T)"/>
    /// <inheritdoc cref="UnityEngine.Object.Instantiate(UnityEngine.Object,
    /// Vector3, Quaternion)"/>
    public static T InstantiateComponent<T>(this T reference, Vector3 position,
        Quaternion rotation) where T : Component
    {
        GameObject.Instantiate(
            reference.gameObject,
            position,
            rotation
        ).RequireComponent(out T instance);
        return instance;
    }

    /// <inheritdoc cref="InstantiateComponent{T}(T)"/>
    /// <inheritdoc cref="UnityEngine.Object.Instantiate(UnityEngine.Object,
    /// Vector3, Quaternion, Transform)"/>
    public static T InstantiateComponent<T>(this T reference, Vector3 position,
        Quaternion rotation, Transform parent) where T : Component
    {
        GameObject.Instantiate(
            reference.gameObject,
            position,
            rotation,
            parent
        ).RequireComponent(out T instance);
        return instance;
    }
    #endregion

    #region Resource Load
    /// <summary>
    /// Loads <paramref name="obj"/> from <paramref name="path"/>.
    /// </summary>
    /// <inheritdoc cref="LoadIfMissing{T}(T, string)"/>
    public static void LoadResource<T>(this T obj, string path)
            where T : UnityEngine.Object
    {
        obj = Resources.Load<T>(path);
    }

    /// <summary>
    /// Loads <paramref name="obj"/> from <paramref name="path"/> if <paramref
    /// name="obj"/> is not set to some value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object to load.</param>
    /// <param name="path">The path of the resource from a Resources
    /// folder.</param>
    /// <returns>True if <paramref name="obj"/> is already set to some value,
    /// false otherwise.</returns>
    public static bool LoadIfMissing<T>(this T obj, string path)
        where T : UnityEngine.Object
    {
        if (!obj)
        {
            obj = Resources.Load<T>(path);
            return false;
        }

        return true;
    }
    #endregion

    #region Other
    /// <summary>
    /// Checks if an object either <br/>
    /// - is null <br/>
    /// - is a UnityEngine.Object that is == null, meaning that's invalid - ie.
	/// Destroyed, not assigned, or created with new. <br/>
    ///
    /// Unity overloads the == operator for UnityEngine.Object, and returns true
	/// for a == null both if a is null, or if it doesn't exist in the c++
	/// engine. This method is for checking for either of those being the case
	/// for objects that are not necessarily UnityEngine.Objects. This is useful
	/// when you're using interfaces, since == is a static method, so if you
	/// check if a member of an interface == null, it will hit the default C# ==
	/// check instead of the overridden Unity check.
    /// 
    /// Source:
	/// https://forum.unity.com/threads/when-a-rigid-body-is-not-attached-component-getcomponent-rigidbody-returns-null-as-a-string.521633/
    /// </summary>
    /// <param name="obj">Object to check</param>
    /// <returns>True if the object is null, or if it's a UnityEngine.Object
    /// that has been destroyed</returns>
    public static bool IsNullOrUnityNull(this object obj)
    {
        if (obj == null)
        {
            return true;
        }

        if (obj is UnityEngine.Object @object)
        {
            if (@object == null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Copies a component to <paramref name="target"/>. Adapted from
    /// http://answers.unity.com/answers/1118416/view.html
    /// </summary>
    /// <typeparam name="T">A component.</typeparam>
    /// <param name="original">Reference to the component to copy.</param>
    public static void CopyComponentTo<T>(this T original, T target)
        where T : Component
    {
        Type type = original.GetType();
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic)
                continue;

            field.SetValue(target, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name")
                continue;

            try
            {
                prop.SetValue(target, prop.GetValue(original, null), null);
            }
            catch (System.Exception)
            {
                continue;
            }
        }
    }

    /// <summary>
    /// Copies a component and adds it to destination. Adapted from
    /// http://answers.unity.com/answers/1118416/view.html
    /// </summary>
    /// <typeparam name="T">A component.</typeparam>
    /// <param name="original">Reference to the component to copy.</param>
    /// <param name="destination">Where to add the component.</param>
    /// <returns></returns>
    public static T CopyComponent<T>(this T original, GameObject destination)
        where T : Component
    {
        Type type = original.GetType();
        var dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic)
                continue;

            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name")
                continue;

            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst;
    }

    /// <summary>
    /// Instantiates a singleton (aka an instance). Also checks if singleton is
    /// already set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">The monobehavior to instantiate the singleton
    /// on.</param>
    /// <param name="singleton">The static singleton to set.</param>
    /// <param name="dontDestroyOnLoad">If true, then call DontDestroyOnLoad on
    /// the gameobject. Also orphans the gameobject.</param>
    public static void InstantiateSingleton<T>(this T self, ref T singleton,
        bool dontDestroyOnLoad = true) where T : MonoBehaviour
    {
        if (singleton)
        {
            GameObject.Destroy(self.gameObject);
            Debug.LogError($"Multiple instances of {typeof(T)}.");
        }
        else
        {
            singleton = self;

            if (dontDestroyOnLoad)
            {
                self.transform.Orphan();
                GameObject.DontDestroyOnLoad(self.gameObject);
            }
        }
    }

    /// <summary>
    /// Detects if Unity is running as an editor or application, then chooses
    /// the appropriate destruction method to use to destroy <paramref
    /// name="gameObject"/>.
    /// </summary>
    /// <param name="gameObject">The GameObject to destroy.</param>
    /// <exception cref="ArgumentException"></exception>
    public static void AutoDestroy(this GameObject gameObject)
    {
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorUtility.IsPersistent(gameObject))
            {
                throw new ArgumentException(
                    "Trying to destroy a persistent object!",
                    nameof(gameObject)
                );
            }

            GameObject.DestroyImmediate(gameObject, false);
#endif
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
    #endregion
}