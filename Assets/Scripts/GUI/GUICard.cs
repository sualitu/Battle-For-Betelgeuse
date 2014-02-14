using UnityEngine;
using System.Collections;

public class GUICard : MonoBehaviour
{
	public System.Guid uniqueId;
	public GUISkin skin = null;
	GUISkin ownSkin;
	public Card Card { get; set; }
	public float Rotation = 0;
	public bool HandCard = true;
	Player Owner;
	Rect position;	
	float height = 337f;
	float width = 203f;
	int x = Screen.width;
	int y = Screen.height;
	float r = 0;
	int i = 0;

	Texture2D GetBaseTexture() {
		switch(Card.cardType) {
		case CardType.UNIT: return (Texture2D) Resources.Load("GUI/Cards/base/baseunitcard_" + TranslateFactionToColourString());
		case CardType.SPELL: return (Texture2D) Resources.Load("GUI/Cards/base/basecard_" + TranslateFactionToColourString());
		case CardType.BUILDING: return (Texture2D) Resources.Load("GUI/Cards/base/basebuildingcard_" + TranslateFactionToColourString());
		default: return ownSkin.button.normal.background;
		}
	}

	string TranslateFactionToColourString() {
		switch (Card.Faction) {
		case Faction.GOOD:
			return "blue";
		case Faction.EVIL:
			return "red";
		case Faction.CONTROL:
			return "green";
		default:
			return "grey";
		}
	}

	void UpdateTexture() {
		Texture2D texture = GetBaseTexture();

		Texture2D costTexture = (Texture2D) Resources.Load("GUI/Cards/costs/" + TranslateFactionToColourString() + "/cost" + Card.Cost);
		int costHeight = costTexture.height;
		int costWidth = costTexture.width;
		int fullHeight = Mathf.FloorToInt(texture.height);
		int fullWidth = Mathf.FloorToInt(texture.width);

		Texture2D newTexture = new Texture2D(fullWidth,fullHeight, texture.format, false);
		newTexture.SetPixels(texture.GetPixels(0,0,fullWidth,fullHeight));

		var cost = costTexture.GetPixels(0,0, costWidth, costHeight);
		newTexture.SetPixels(88,309,costWidth,costHeight,cost);

		switch(Card.cardType) {
			case CardType.UNIT: SetTextureStats(newTexture, (UnitCard) Card); break;
			case CardType.SPELL: SetTextureStats(newTexture, (SpellCard) Card); break;
			case CardType.BUILDING: SetTextureStats(newTexture, (BuildingCard) Card); break;
			default: break;
		}

		Texture2D image = (Texture2D) Resources.Load("GUI/Cards/images/" + Card.Image);
		var img = image.GetPixels(0,0, image.width,  image.height);
		newTexture.SetPixels(25,197,image.width, image.height,img);


		newTexture.Apply();
		SetTexture(newTexture);
	}

	Texture2D SetTextureStats(Texture2D texture, UnitCard card) {
		Texture2D healthTexture = (Texture2D) Resources.Load("GUI/Cards/health/" + TranslateFactionToColourString() + "/health" + card.Health);
		int healthHeight = healthTexture.height;
		int healthWidth = healthTexture.width;
		var health = healthTexture.GetPixels(0, 0, healthWidth, healthHeight);
		texture.SetPixels(154,165, healthWidth, healthHeight, health);
		Texture2D attackTexture = (Texture2D) Resources.Load("GUI/Cards/attack/" + TranslateFactionToColourString() + "/attack" + card.Attack);
		int attackHeight = attackTexture.height;
		int attackWidth = attackTexture.width;
		var attack = attackTexture.GetPixels(0, 0, attackWidth, attackHeight);
		texture.SetPixels(46,164, attackWidth, attackHeight, attack);
		Texture2D movementTexture = (Texture2D) Resources.Load("GUI/Cards/movement/" + TranslateFactionToColourString() + "/movement" + card.Movement);
		int movementHeight = movementTexture.height;
		int movementWidth = movementTexture.width;
		var movement = movementTexture.GetPixels(0, 0, movementWidth, movementHeight);
		texture.SetPixels(95,167, movementWidth, movementHeight, movement);
		return texture;
	}

	Texture2D SetTextureStats(Texture2D texture, BuildingCard card) {
		Texture2D healthTexture = (Texture2D) Resources.Load("GUI/Cards/health/" + TranslateFactionToColourString() + "/health" + card.Health);
		int healthHeight = healthTexture.height;
		int healthWidth = healthTexture.width;
		var health = healthTexture.GetPixels(0, 0, healthWidth, healthHeight);
		texture.SetPixels(154,165, healthWidth, healthHeight, health);
		Texture2D attackTexture = (Texture2D) Resources.Load("GUI/Cards/attack/" + TranslateFactionToColourString() + "/attack" + card.Attack);
		int attackHeight = attackTexture.height;
		int attackWidth = attackTexture.width;
		var attack = attackTexture.GetPixels(0, 0, attackWidth, attackHeight);
		texture.SetPixels(46,164, attackWidth, attackHeight, attack);
		return texture;
	}

	Texture2D SetTextureStats(Texture2D texture, SpellCard card) {
		return texture;
	}

	void SetTexture(Texture2D texture) {
		ownSkin.button.normal.background = texture;
		ownSkin.button.hover.background = texture;
		ownSkin.button.active.background = texture;
	}

	public bool selected = false;
	void Start() {
		uniqueId = System.Guid.NewGuid();
	}

	public void SetPosition(float x, float y) {
		SetPosition(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
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

	bool textureUpdated = false;
	
	public void Played() {
		x = Screen.width/3;
		y = Screen.height/4;
		Rotation = 0;
		i = 300;
	}

	public void Kill() {
		i = 1;
	}
	
	public bool IsMouseOver = false;
	
	public void ForcePlaceCard(float x, float y) {
		ForcePlaceCard(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
	}

	public void ForcePlaceCard(int x, int y) {
		this.x = x;
		this.y = y;
		position = new Rect(x, y, width,height);
	}
	
	public void OnGUI() {
		IsMouseOver = position.Contains(Event.current.mousePosition);
		if(Card != null) {
			if(!textureUpdated) {
				ownSkin = (GUISkin) Instantiate(skin);
				textureUpdated = true;
				UpdateTexture();
			}
			GUI.skin = ownSkin;
			if(position.Contains(Event.current.mousePosition) && HandCard) {
				position = iTween.RectUpdate(position, new Rect (x,y-height*1/2,width,height), 4);
				r = iTween.FloatUpdate(r,0,1);
				GUIUtility.RotateAroundPivot(r, position.center);
				GUI.depth = 0;
				
			} else {
				position = iTween.RectUpdate(position, new Rect (x,y,width,height), 4);	
				r = iTween.FloatUpdate(r,Rotation,1);	
				GUIUtility.RotateAroundPivot(r, position.center);
				GUI.depth = HandCard ? 1 : 0;
			}
			
			if(GUI.Button(position, Card.Name) && HandCard){
				
				if(!selected) {
					
					Owner.SelectCard(this);
				} else {
					Owner.DeselectCard();
					selected = false;
					MoveHorizontally(50);
				}
			}
			GUI.Label (new Rect(position.x+35, position.y+height/2+20, width/2+35 , height), Card.CardText);
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

