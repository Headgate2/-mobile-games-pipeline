using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public static class GeneralExtensions
{
	public static void Shuffle<T>(this IList<T> list)
	{
		System.Random rng = new System.Random();
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

	public static T RandomObject<T>(this IList<T> list)
	{
		if (list.Count > 0)
			return list[UnityEngine.Random.Range(0, list.Count)];
		else
			return default(T);
	}

	/// <summary>
	/// Calls an action on all pairings of items in the list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="action">action called on a pair</param>
	/// <param name="includeSelfPair">Include pairing of items with themselves</param>
	public static void UniquePairs<T>(this IList<T> list, System.Action<T, T> action, bool includeSelfPair = false)
	{
		for(int x = 0; x < list.Count; x++)
		{
			for(int y = includeSelfPair ? x : x + 1; y < list.Count; y++)
				action(list[x], list[y]);
		}
	}

	public static List<T> SelectUnique<T>(this IEnumerable<T> list, System.Func<T, System.Object> uniqueSelector)
	{
		List<T> unique = new List<T>();
		foreach(T item in list)
		{
			if (!unique.Exists(u => uniqueSelector(item) == uniqueSelector(u)))
				unique.Add(item);
		}
		return unique;
	}

	//Returns whether all items based on a selector from the list matche the given value
	public static bool AllMatch<T, K>(this IEnumerable<T> list, System.Func<T, K> selector, K value) where K : System.IComparable
	{
		foreach(T item in list)
		{
			if (selector(item).CompareTo(value) != 0)
				return false;
		}
		return true;
	}

	public static T Find<T>(this IEnumerable<T> list, System.Func<T, bool> selector)
	{
		if (list != null)
		{
			foreach (T item in list)
			{
				if (selector(item))
					return item;
			}
		}
		return default(T);
	}

	public static List<T> FindAll<T>(this IEnumerable<T> list, System.Func<T, bool> selector)
	{
		List<T> items = new List<T>();
		foreach(T item in list)
		{
			if (selector(item))
				items.Add(item);
		}
		return items;
	}

    public static string ToHumanTime(uint seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);
        string str = "";
        if(ts.Minutes > 0)
            str += ts.Minutes.ToString() + " Minute" + (ts.Minutes != 1 ? "s" : " ");
        if (ts.Seconds > 0)
            str += ts.Seconds.ToString() + " Second" + (ts.Seconds != 1 ? "s" : " ");
        return str;
    }

    public static string ToMMSSTime(float seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);
        return ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
    }

	public static string RemoveNonAlphanumeric(this string text)
	{
		StringBuilder sb = new StringBuilder(text.Length);

		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9')
				sb.Append(text[i]);
		}

		return sb.ToString();
	}

	public static void ForEach<T>(this T[,] list, Action<T> action)
	{
		for (int x = 0; x < list.GetLength(0); x++)
			for (int y = 0; y < list.GetLength(1); y++)
				action(list[x, y]);
	}
}
