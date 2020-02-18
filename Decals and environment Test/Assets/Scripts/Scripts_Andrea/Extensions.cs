using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Extensions
{
    static System.Random rng;

    static Extensions()
    {
        rng = new System.Random();
    }

    //allows us to shuffle a list in a sufficiently random way, used to scatter notes
    public static void ShuffleList<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
