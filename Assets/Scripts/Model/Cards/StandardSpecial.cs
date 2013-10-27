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
	
	public class Ranged : StandardSpecial {
		public int Range { get; set; }
		public Ranged(int i) {
			Range = i;
		}
	}
	
	public class Defenseless : StandardSpecial {
	}
}

