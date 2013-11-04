using UnityEngine;
using System.Collections;

public class StandardSpecial
{
	public class Boost : StandardSpecial {
		public int Amount { get; set; }
		public Boost(int i) {
			Amount = i;
		}
		
		public override string ToString ()
		{
			return "Boost " + Amount;
		}
	}
	
	public class Ranged : StandardSpecial {
		public int Range { get; set; }
		public Ranged(int i) {
			Range = i;
		}
		
		public override string ToString ()
		{
			return "Ranged " + Range;
		}
	}
	
	public class Defenseless : StandardSpecial {
	}
}

