using UnityEngine;
using System.Collections;

public class DromonCard : UnitCard
{
	public override int Attack {
		get {
			return 1;
		}
	}
	
	public override int Cost {
		get {
			return 3;
		}
	}
	
	public override int Health {
		get {
			return 4;
		}
	}
	
	public override int Movement {
		get {
			return 5;
		}
	}
	
	public override string Name {
		get {
			return "Dromon";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/Dromon";
		}
	}

	public override string Image {
		get {
			return "dromon";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}
	
	public DromonCard() {
		CardText += "Gains one attack when attacking.";
	}
	
	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		return;
	}
	
	public override void OnAttack (StateObject s)
	{
		AddBuff (s.MainHex.Unit);
	}

	string buffName = "Hot Streak";

	void AddBuff(Unit u) {
		StackableBuff buff = (StackableBuff) u.Buffs.Find(b => b.Name.Contains(buffName));
		if(buff != null) {
			buff.IncreaseStack(u);
		} else {
			u.AddBuff(new StackableBuff(buffName, onRemove: OnRemove, onApplication: OnAdd, duration: -1));
		}
	}
	
	void OnAdd(Unit u) {
		u.Attack += 1;
	}
	
	void OnRemove(Unit u) {
		u.Attack -= 1;
	}
	
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

