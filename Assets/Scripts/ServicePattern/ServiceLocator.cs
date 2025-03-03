using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Service : MonoBehaviour
{
    // Base class for all services
    protected virtual void Awake()
    {
    }

    public void DestroyServiceObject()
    {
        DestroyImmediate(gameObject);
    }
}

#nullable enable
public static class ServiceLocator
{
    private static Dictionary<Type, Service> services = new();

    /// <summary>
    /// Register a service, the service will be held and can be accessed anywhere with <see cref="GetService{T}(T)"/>
    /// </summary>
    /// <typeparam name="T">T of Type <see cref="Service"/></typeparam>
    /// <param name="service">The service reference</param>
    /// <returns>Return true if service registration was successful.</returns>
    public static bool RegisterService<T>(T service) where T : Service
    {
        Debug.Log($"Registering Service: {typeof(T)}");
        if (services.ContainsKey(service.GetType())) return false;
        services[service.GetType()] = service;
        return true;
    }

    /// <summary>
    /// Unregisters the service, this will destroy the service class if not instantiated with a <see cref="GameObject"/>
    /// </summary>
    /// <typeparam name="T">T of type <see cref="Service"/></typeparam>
    /// <param name="service">The servce reference</param>
    /// <returns>Returns true if the service unregistration was succesful.</returns>
    public static bool UnregisterService<T>(T service) where T : Service
    {
        if (!services.ContainsKey(service.GetType())) return false;
        services.Remove(service.GetType());
        //service.DestroyServiceObject();
        return true;
    }

    public static bool UnregisterService<T>() where T : Service
    {
        if (!services.ContainsKey(typeof(T))) return false;
        services.Remove(typeof(T));
        return true;
    }

    /// <summary>
    /// Returns the requested service of type T
    /// </summary>
    /// <typeparam name="T">The type of the service to be requested</typeparam>
    /// <returns>The reference to the service of type T. Returns null if the service could not be located.</returns>
    public static T? GetService<T>() where T : Service
    {
        if (services.TryGetValue(typeof(T), out var service)) return (T)service;
        return null;
    }

    public static bool DoesServiceExist<T>() where T : Service
    {
        return services.ContainsKey(typeof(T));
    }

    public static bool CompareService<T>(T service) where T : Service
    {
        return services[typeof(T)] == service;
    }
}
