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

	public class ForceField : StandardSpecial {
		public ForceField() {
		}

		public override string ToString ()
		{
			return "Force Field";
		}
	}

	public class DeathTouch : StandardSpecial {
		public DeathTouch() {
		}

		public override string ToString ()
		{
			return "Death Touch";
		}
	}
	
	public class Defenseless : StandardSpecial {
	}
}

