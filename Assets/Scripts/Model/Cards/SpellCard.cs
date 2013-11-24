using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellCard : Card
{
	public SpellCard() : base() {
		cardType = CardType.SPELL;
	}
	
	public override List<Hex> Targets(StateObject s) {
		return new List<Hex>();
	}
	
	public virtual MockUnit MockOnPlay(MockUnit mo) {
		Debug.LogError("MockOnPlay attempted called on card without it implemented. This probably means an AI is trying to use a card that is not meant for AI");
		return mo;
	}
	
	public override void OnPlay(StateObject s) {
		SpellAnimation(s);
	}
	
	protected void DoDelayedEffect(StateObject s, float delay) {
		SpellCardCallBack cardCallBack = ((GameObject) Object.Instantiate(((GameObject) Resources.Load("Utils/SpellCardCallBack")))).GetComponent<SpellCardCallBack>();
		cardCallBack.Setup(s, this, delay);
	}
	
	public virtual void SpellAnimation(StateObject s) {
		Object.Instantiate((GameObject) Resources.Load ("Effects/Heal"), s.TargetUnit.transform.localPosition, Quaternion.identity);
		SpellEffect(s);
	}
	
	public abstract void SpellEffect(StateObject s);
}

