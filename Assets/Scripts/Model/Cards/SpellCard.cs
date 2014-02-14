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
	
	public virtual int MockOnPlay(MockUnit mo, HexEvaluator he) {
		Debug.LogError("MockOnPlay attempted called on card without it implemented. This probably means an AI is trying to use a card that is not meant for AI");
		return 0;
	}

	
	public int TargetlessMockOnPlay() {
		if(IsTargetless) {
			return MockOnPlay(null, null);
		} else {
			Debug.LogError("MockOnPlay for targetless spell called for a spell that was not targetless!");
			return 0;
		}
	}

	
	public override void OnPlay(StateObject s) {
		SpellAnimation(s);
	}
	
	protected void DoDelayedEffect(StateObject s, float delay) {
		SpellCardCallBack cardCallBack = ((GameObject) Object.Instantiate(((GameObject) Resources.Load("Utils/SpellCardCallBack")))).GetComponent<SpellCardCallBack>();
		cardCallBack.Setup(s, this, delay);
	}

	protected void StandardAnimation(Vector3 place) {
		Object.Instantiate((GameObject) Resources.Load ("Effects/Heal"), place, Quaternion.identity);
	}
	
	public virtual void SpellAnimation(StateObject s) {
		StandardAnimation(s.MainHex.collider.bounds.center);
		SpellEffect(s);
	}
	
	public abstract void SpellEffect(StateObject s);
}

