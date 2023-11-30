using System;
using UnityEngine;

//22/02/2021

public class Helper
{
    public static Color SetColor(string colorCode)
    {
        Color newCol;
        if (ColorUtility.TryParseHtmlString(colorCode, out newCol))
            return newCol;

        return newCol;
    }

    public static string FormatNumber(float value)
    {
        if (value >= 1000000)
            return RoundUp(value / 1000000, 2) + "M";

        if (value >= 1000)
            return RoundUp(value / 1000, 1) + "K";

        return value.ToString();
    }

    public static double RoundUp(double value, int places)
    {
        double multiplier = Math.Pow(10, Convert.ToDouble(places));
        return Math.Round(value * multiplier) / multiplier;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}