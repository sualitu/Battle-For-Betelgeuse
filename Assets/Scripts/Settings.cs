using UnityEngine;
using System.Collections;

public class Settings
{
	// Constants
	public const string version = "alpha 0.2";
	public static Vector2 NativeResolution = new Vector2(1920, 1080);
	//// User Settings
	// Camera Settings
	public static int ScrollSpeed = 25;
	public static int LevelArea = 60;
	public static int ScrollArea = 25;
    public static int DragSpeed = 80;
	public static int ZoomMax = 40;
    public static int ZoomSpeed = 25;
	public static int ZoomMin = 10;
    public static int PanSpeed = 50;
	public static int PanAngleMin = 35;
    public static int PanAngleMax = 60;
	// Gameplay Settings
	public static int MaxMana = 15;
	public static int FlagBaseValue = 7;
	public static int FlagMultValue = 2;
	public static int StartingHandCount = 3;
	public static int MaxHandCount = 7;	
	public static int VictoryRequirement = 1000;
	// Tile Colours
	public static Color MouseOverTileColour = Color.red; 
	public static Color MovableTileColour = new Color(0.95f, 0.47f, 0.13f); // Dark Orange
	public static Color OwnedFlagTileColour = Color.green;
	public static Color EnemyFlagTileColour = new Color(0.66f, 0f, 0.2f);
	public static Color NeutralFlagTileColour = Color.gray;
	public static Color StandardTileColour = Color.white;
	public static Color SelectedTileColour = Color.red;
	// Temp	
}

