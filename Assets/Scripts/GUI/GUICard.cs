using UnityEngine;
using System.Collections;

public class GUICard : MonoBehaviour
{
	public System.Guid uniqueId;
	public GUISkin skin = null;
	public Card Card { get; set; }
	public float Rotation = 0;
	
	Player Owner;
	Rect position;	
	float height = 300f;
	float width = 186f;
	int x = Screen.width;
	int y = Screen.height;
	float r = 0;
	int i = 0;
	
	public bool selected = false;
	void Start() {
		uniqueId = System.Guid.NewGuid();
	}
	
	public void SetPosition(int x, int y) {
		this.x = x;
		this.y = y;
	}
	
	void MoveHorizontally(int i) {
		y += i;
	}
	
	void MoveVertically(int i) {
		x += i;
	}
	
	public void Select() {
		selected = true;
		MoveHorizontally(-50);
	}
	
	public void Deselect() {
		MoveHorizontally(50);
		selected = false;
	}
	
	public void SetInfo(Card card, Player owner) {
		Card = card;
		Owner = owner;
	}
	
	public void Played() {
		x = Screen.width/3;
		y = Screen.height/4;
		Rotation = 0;
		i = 300;
	}
	
	public bool IsMouseOver = false;
	
	public void ForcePlaceCard(int x, int y) {
		position = new Rect(x, y, width,height);
	}
	
	public void OnGUI() {
		IsMouseOver = position.Contains(Event.current.mousePosition);
		if(Card != null) {
			GUI.skin = skin;
			
			if(position.Contains(Event.current.mousePosition)) {
				position = iTween.RectUpdate(position, new Rect (x,y-height*1/2,width,height), 4);
				r = iTween.FloatUpdate(r,0,1);
				GUIUtility.RotateAroundPivot(r, position.center);
				GUI.depth = 0;
				
			} else {
				position = iTween.RectUpdate(position, new Rect (x,y,width,height), 4);	
				r = iTween.FloatUpdate(r,Rotation,1);	
				GUIUtility.RotateAroundPivot(r, position.center);
				GUI.depth = 1;
			}
			
			if(GUI.Button(position, Card.Name)){
				
				if(!selected) {
					
					Owner.SelectCard(this);
				} else {
					Owner.DeselectCard();
					selected = false;
					MoveHorizontally(50);;
				}
			}
			GUI.Label (new Rect(position.x+123, position.y+13, width , height), Card.Cost.ToString());
			GUI.Label (new Rect(position.x+35, position.y+height/2+10, width/2+30 , height), Card.CardText);
			if(typeof(EntityCard).IsAssignableFrom(Card.GetType())) {
				EntityCard eCard = (EntityCard) Card;
				GUI.Label (new Rect(position.x+140, position.y+265, width , height), eCard.Attack + " / " + eCard.Health);
				if(typeof(UnitCard).IsAssignableFrom(eCard.GetType())) {
					GUI.Label (new Rect(position.x+50, position.y+13, width , height), "Unit");
					GUI.Label (new Rect(position.x+20, position.y+265, width , height), ((UnitCard) Card).Movement.ToString());
				} else {
					GUI.Label (new Rect(position.x+50, position.y+13, width , height), "Building");
				}
			}	
			if(typeof(SpellCard).IsAssignableFrom(Card.GetType())) {
				GUI.Label (new Rect(position.x+50, position.y+13, width , height), "Spell");
			}
		}
		if(i > 0) {
			if(i == 100) {
				MoveVertically(-(Screen.width/2+300));
				Rotation = 90;
			}
			if(i == 1) {
				Destroy(this);
			}
			i--;
			
		}
	}
}

