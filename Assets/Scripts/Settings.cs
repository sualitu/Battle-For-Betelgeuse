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
	public static int LevelArea = 30;
	public static int ScrollArea = 25;
    public static int DragSpeed = 80;
	public static int ZoomMax = 40;
    public static int ZoomSpeed = 25;
	public static int ZoomMin = 10;
    public static int PanSpeed = 50;
	public static int PanAngleMin = 35;
    public static int PanAngleMax = 60;
	// Gameplay Settings
	public static int StartingHandCount = 3;
	public static int MaxHandCount = 5;	
	// Temp	
}

