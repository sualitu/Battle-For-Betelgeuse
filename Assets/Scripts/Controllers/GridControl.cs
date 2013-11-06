using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(GameControl))]
public class GridControl : MonoBehaviour
{
	public Vector2 MapSize;
	public GameObject HexGrid;
	public TextAsset mapfile;
	public Vector2 Base1;
	public Vector2 Base2;
	
	public List<List<Hex>> Map { get; set; }
	public List<List<bool>> boolMap;
	private Vector2 HexExt;
	//private GameControl gameControl;
	
	
	void GetHexProperties(){
		GameObject Inst = (GameObject) Instantiate (HexGrid, Vector3.zero, Quaternion.identity);
		HexExt = new Vector2(Inst.gameObject.collider.bounds.extents.x, Inst.gameObject.collider.bounds.extents.z);
		Destroy(Inst);
	}	
	
	/// <summary>
	/// Builds boolean map from external file.
	/// </summary>
	/// <returns>
	/// The boolean map.
	/// </returns>
	List<List<bool>> BuildFromExtFile() {
		StreamReader reader = new StreamReader(Application.dataPath + "/../map.txt");
		string file = reader.ReadToEnd();
		reader.Close();
		boolMap = new List<List<bool>>();
		var lines = file.Split("\n"[0]);
		System.Array.Reverse(lines);
		int z = 0;
		int i;
		foreach(string line in lines) {
			i = 0;
			List<bool> row = new List<bool>();
			var types = line.Split(" "[0]);
			foreach(string s in types) {
				int type = int.Parse(s);
				// TODO Prettify
				if(type == 2) {
					Base1 = new Vector2(i, z);
				} else if (type == 3) {
					Base2 = new Vector2(i, z);
				}
				row.Add(type > 0);
				i++;
			}
			z++;
			boolMap.Add(row);
		}
		MapSize = new Vector2(boolMap[0].Count,boolMap.Count);
		return boolMap;
	}
	
	string ReverseString( string s )
	{
	    char[] charArray = s.ToCharArray();
	    System.Array.Reverse( charArray );
	    return new string( charArray );
	}
	
	/// <summary>
	/// Builds boolean map.
	/// </summary>
	/// <returns>
	/// The bool map.
	/// </returns>
	List<List<bool>> BuildBoolMap() {
		boolMap = new List<List<bool>>();		
		string file = mapfile.text;
		var lines = file.Split("\n"[0]);
		System.Array.Reverse(lines);
		int z = 0;
		int i;
		foreach(string line in lines) {
			i = 0;
			List<bool> row = new List<bool>();
			var types = line.Split(" "[0]);
			foreach(string s in types) {
				int type = int.Parse(s);
				// TODO Prettify
				if(type == 2) {
					Base1 = new Vector2(i, z);
				} else if (type == 3) {
					Base2 = new Vector2(i, z);
				}
				row.Add(type > 0);
				i++;
			}
			z++;
			boolMap.Add(row);
		}
		MapSize = new Vector2(boolMap[0].Count,boolMap.Count);
		return boolMap;
	}
	
	// Inspired by FX_HexGrid
	void GenerateMap() {
		boolMap = BuildBoolMap();
		Map = new List<List<Hex>>();
		for (int k = 0; k < MapSize.x; k++){
			List<Hex> row = new List<Hex>();
			for(int j = 0; j < MapSize.y; j++){
				row.Add(null);
			}
			Map.Add(row);
		}
		GameObject HexMap = new GameObject("HexMap");
		HexMap.transform.position = Vector3.zero;
		bool odd;
		for (int h = 0; h < MapSize.y; h++){
			odd = (h % 2) == 0;
			for(int w = 0; w < MapSize.x; w++){
				if(boolMap[h][w]) {
					GameObject HexTile = (GameObject) Instantiate (HexGrid, Vector3.zero, Quaternion.identity);
					HexTile.transform.position = new Vector3(w * ((HexExt.x * 2f)) + (odd ? 0f : (HexExt.x)), 0.05f, (h * HexExt.y) * 1.5f);
					HexTile.GetComponent<Hex>().GridPosition = new Vector2(w, h);
					Map[Mathf.FloorToInt(w)][Mathf.FloorToInt(h)] = HexTile.GetComponent<Hex>();
					HexTile.transform.parent = HexMap.transform;
				}
			}
		}		
		HexMap.transform.position = new Vector3(1f,0f,0f);
	}
		
	void Start ()
	{		
		GetHexProperties();
		GenerateMap();
	}
}

