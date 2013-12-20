using UnityEngine;
using System.Collections.Generic;

public static class ListExtensions
{
	public static T RandomElement<T>(this List<T> list) {
		if(list.Count < 1) {

		} else if(list.Count == 1) {
			return list[0];
		} else {
			return list[Random.Range (0, list.Count)];
		}
		return default(T);
	}
}

