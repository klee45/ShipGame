using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    // --------- Effects

    public static int MaxBonus(int initial, int bonus, int initialMax, int bonusMax)
    {
        int sum = initial + bonus;
        if (bonusMax >= initialMax || initial < bonusMax)
        {
            return Mathf.Min(bonusMax, sum);
        }
        else
        {
            return initial;
        }
    }

    public static int MinBonus(int initial, int bonus, int initialMin, int bonusMin)
    {
        int sum = initial + bonus;
        if (bonusMin <= initialMin || initial > bonusMin)
        {
            return Mathf.Max(bonusMin, sum);
        }
        else
        {
            return initial;
        }
    }

    // --------- Math

    public static float ToDegree(this float f)
    {
        return f * 180f / Mathf.PI;
    }

    public static float ToRadian(this float f)
    {
        return f * Mathf.PI / 180f;
    }
    
    public static int Sum(this IEnumerable<int> lst)
    {
        int result = 0;
        foreach (int i in lst)
        {
            result += i;
        }
        return result;
    }

    public static float Sum(this IEnumerable<float> lst)
    {
        float result = 0;
        foreach (float i in lst)
        {
            result += i;
        }
        return result;
    }

    public static int Product(this IEnumerable<int> lst)
    {
        int result = 0;
        foreach (int i in lst)
        {
            result *= i;
        }
        return result;
    }

    public static float Product(this IEnumerable<float> lst)
    {
        float result = 0;
        foreach (float i in lst)
        {
            result *= i;
        }
        return result;
    }

    public static int GetParts(this float f, out float remainder)
    {
        remainder = f % 1.0f;
        return (int)f;
    }
}
