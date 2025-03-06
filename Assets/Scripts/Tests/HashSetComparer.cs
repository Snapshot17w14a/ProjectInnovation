using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashSetComparer : IEqualityComparer<HashSet<string>>
{
    public static readonly HashSetComparer Instance = new HashSetComparer();

    public bool Equals(HashSet<string> x, HashSet<string> y)
    {
        return x.SetEquals(y);
    }

    public int GetHashCode(HashSet<string> obj)
    {
        int hash = 0;

        foreach (var  item in obj)
        {
            hash ^= item.GetHashCode();
        }
        return hash;
    }
}
