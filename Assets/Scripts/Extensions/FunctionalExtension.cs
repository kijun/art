using System;
using System.Collections;
using System.Collections.Generic;

public static class FunctionalExtension {
	public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
	{
	  foreach (T item in value)
	  {
		action(item);
	  }
	}
}
