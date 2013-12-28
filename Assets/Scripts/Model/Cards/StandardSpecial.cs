using UnityEngine;
using System.Collections;

public class StandardSpecial
{
	public virtual int Value() { return 0; }
	public class Boost : StandardSpecial {
		public int Amount { get; set; }
		public Boost(int i) {
			Amount = i;
		}

		public override int Value () { return Amount; }
		
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
		public override int Value () { return 10; }
	}

	public class ForceField : StandardSpecial {
		public ForceField() {
		}

		public override string ToString ()
		{
			return "Force Field";
		}
		public override int Value () { return 10; }
	}

	public class DeathTouch : StandardSpecial {
		public DeathTouch() {
		}

		public override string ToString ()
		{
			return "Death Touch";
		}

		public override int Value () { return 10; }
	}
	
	public class Defenseless : StandardSpecial {
	}
}

