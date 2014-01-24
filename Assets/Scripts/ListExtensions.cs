using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class ListExtensions
{
	public static T RandomElement<T>(this List<T> list) {
		if(list.Count < 1) {

		} else if(list.Count == 1) {
			return list[0];
		} else {
			return list[UnityEngine.Random.Range (0, list.Count)];
		}
		return default(T);
	}
	
	public static IEnumerable<IEnumerable<T>> PowerSet<T>(this List<T> list) {
		return  from m in Enumerable.Range(0, 1 << list.Count)
			   	select
				from i in Enumerable.Range(0, list.Count)
				where (m & (1 << i)) != 0
				select list[i];
	}

	public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> list)
	{
		if (list.Count() == 1)
		return new List<IEnumerable<T>> { list };
		
		return list.Select(
			(a, i1) => Permute(list.Where((b, i2) => i2 != i1)).Select(
				b => (new List<T> { a }).Union(b))
			).SelectMany(c => c);
	}
}

