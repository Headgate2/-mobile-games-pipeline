using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class EnumExtensions
{
	public static T[] GetValues<T>()
	{
		ValidateEnum<T>();
		return (T[])Enum.GetValues(typeof(T));
	}

	public static int ValueCount<T>()
	{
		ValidateEnum<T>();
		return Enum.GetValues(typeof(T)).Length;
	}

	public static T GetRandomValue<T>()
	{
		T[] vals = GetValues<T>();
		return vals[UnityEngine.Random.Range(0, vals.Length)];
	}

	public static void ValidateEnum<T>()
	{
		// Can't use type constraints on value types, so have to do check like this
		if (typeof(T).BaseType != typeof(Enum))
			throw new ArgumentException("T must be of type System.Enum");
	}
}
