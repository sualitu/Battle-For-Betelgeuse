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
		public Ranged() {
		}
		
		public override string ToString ()
		{
			return "Ranged";
		}
	}
	
	public class Defenseless : StandardSpecial {
	}
}

