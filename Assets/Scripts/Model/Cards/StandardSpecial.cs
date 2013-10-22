using UnityEngine;
using System.Collections;

public class StandardSpecial
{
	public class Boost : StandardSpecial {
		public int Amount { get; set; }
		public Boost(int i) {
			Amount = i;
		}
	}
}

