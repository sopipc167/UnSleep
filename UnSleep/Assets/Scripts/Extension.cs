using System;
using System.Collections.Generic;


public static class ArrayExtension
{
    public static T[] Filter<T>(this T[] array, Func<T, bool> match)
    {
        List<T> list = new List<T>();
        foreach (T t in array)
        {
            if (match(t)) list.Add(t);
        }
        return list.ToArray();
    }

    public static T[] Map<T>(this T[] array, Func<T, T> func)
    {
        List<T> list = new List<T>();
        foreach (T t in array)
        {
            list.Add(func(t));
        }
        return list.ToArray();
    }

    public static int Count<T>(this T[] array, Func<T, bool> match)
    {
        int count = 0;
        foreach (T t in array)
        {
            if (match(t)) count++;
        }
        return count;
    }

    public static bool isEmtpy<T>(this T[] array)
    {
        return array.Length == 0;
    }

    public static bool isNotEmtpy<T>(this T[] array)
    {
        return array.Length > 0;
    }
}
