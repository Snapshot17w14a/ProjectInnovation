using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Service : MonoBehaviour
{
    // Base class for all services
}

public static class ServiceLocator
{
    public static Dictionary<Type, object> services = new();

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
        services[service.GetType()] = null;
        return true;
    }

    /// <summary>
    /// Returns the requested service of type T
    /// </summary>
    /// <typeparam name="T">The type of the service to be requested</typeparam>
    /// <returns>The reference to the service of type T. Returns null if the service could not be located.</returns>
    public static T GetService<T>() where T : Service
    {
        if (services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        return null;
    }
}
