using System;
using System.Collections.Generic;


public static class ArrayExtension
{
    public static T[] filter<T>(this T[] array, Func<T, bool> match)
    {
        List<T> list = new List<T>();
        foreach (T t in array)
        {
            if (match(t)) list.Add(t);
        }
        return list.ToArray();
    }
}
