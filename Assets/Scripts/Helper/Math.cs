using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Math
{
    // --------- List

    public static void StackList(this List<int> lst)
    {
        for (int i = 1; i < lst.Count; i++)
        {
            lst[i] += lst[i - 1];
        }
    }

    // --------- Enums
    public static int GetCount(this System.Type enumType)
    {
        return System.Enum.GetValues(enumType).Length;
    }

    // --------- Effects

    public interface IStackableBonus
    {
        int GetBonus();
        int GetLimit();
    }

    public static T GetRandomElement<T>(this List<T> lst)
    {
        return lst[Random.Range(0, lst.Count)];
    }

    public static T GetRandomElement<T>(this T[] lst)
    {
        return lst[Random.Range(0, lst.Length)];
    }

    public static int WeightedRandom(List<int> weights, int offset=0)
    {
        int choice = Random.Range(offset, weights.Last());
        for (int pos = 0; pos < weights.Count; pos++)
        {
            if (weights[pos] > choice)
            {
                return pos;
            }
        }
        return -1;
    }

    public static float GetStackableBonusModInverse(IEnumerable<IStackableBonus> lst)
    {
        return 1f / GetStackableBonusMod(lst);
    }

    public static float GetStackableBonusMod(IEnumerable<IStackableBonus> lst)
    {
        int mod = 0;
        foreach (IStackableBonus effect in lst)
        {
            Math.EffectLimit(ref mod, effect.GetBonus(), effect.GetLimit());
        }
        return 1 + (mod / 100f);
    }

    public static void EffectLimit(ref int val, int bonus, int max)
    {
        if (bonus > 0)
        {
            if (val < max)
            {
                val = Mathf.Min(val + bonus, max);
            }
        }
        else // Negative val (with negative max)
        {
            if (val > max)
            {
                val = Mathf.Max(val + bonus, max);
            }
        }
    }

    /**
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
    **/

    // --------- Math

    public static Vector3 GetPerpendicular(this Vector3 v)
    {
        return new Vector3(v.y, v.x, v.z);
    }

    public static Vector3 PiecewiseMult(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static Vector2 PiecewiseMult(this Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }

    public static Vector3 PiecewiseInverse(this Vector3 v)
    {
        try
        {
            return new Vector3(1 / v.x, 1 / v.y, 1 / v.z);
        }
        catch (System.DivideByZeroException e)
        {
            Debug.LogError("Piecewise inverse attempted on an invalid vector " + v.ToString());
            return v;
        }
    }

    public static Vector2 DegreeToVector2(this float f)
    {
        return RadToVector2(Mathf.Deg2Rad * f);
    }

    public static Vector2 RadToVector2(this float f)
    {
        return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
    }

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
