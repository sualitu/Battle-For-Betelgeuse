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
	
	public List<List<Hex>> Map { get; set; }
	public List<List<bool>> boolMap;
	private Vector2 HexExt;
	//private GameControl gameControl;
	
	
	void GetHexProperties(){
		GameObject Inst = (GameObject) Instantiate (HexGrid, Vector3.zero, Quaternion.identity);
		HexExt = new Vector2(Inst.gameObject.collider.bounds.extents.x, Inst.gameObject.collider.bounds.extents.z);
		Destroy(Inst);
	}	
	
	List<List<bool>> BuildFromExtFile() {
		StreamReader reader = new StreamReader(Application.dataPath + "/../map.txt");
		string file = reader.ReadToEnd();
		reader.Close();
		boolMap = new List<List<bool>>();
		var lines = file.Split("\n"[0]);
		System.Array.Reverse(lines);
		
		foreach(string line in lines) {
			List<bool> row = new List<bool>();
			var types = line.Split(" "[0]);
			foreach(string type in types) {
				row.Add(int.Parse (type) > 0);
			}
			boolMap.Add(row);
		}
		MapSize = new Vector2(boolMap[0].Count,boolMap.Count);
		return boolMap;
	}
	
	List<List<bool>> BuildBoolMap() {
		boolMap = new List<List<bool>>();		
		string mapstring = mapfile.text;		
		using (StringReader reader = new StringReader(mapstring))
		{
		    string line;
			int z = 0;
		    while ((line = reader.ReadLine()) != null)
		    {
		        string[] tokens = line.Split(" "[0]);
				
				List<bool> row = new List<bool>();
				
				for(int i = 0; i < tokens.Length; i++) {
					string s = tokens[i];
					if(s.Length > 0) {
						int type = int.Parse(s);
						row.Add(type > 0);
					}
				}
				
				boolMap.Add(row);
				z++;
		    }
			return boolMap;
		}
	}
	
	// Inspired by FX_HexGrid
	void GenerateMap() {
		try {
			boolMap = BuildFromExtFile();
		} catch {
			boolMap = BuildBoolMap();
		}
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
		//BuildFromExtFile();
		GetHexProperties();
		GenerateMap();
		//gameControl = GetComponent<GameControl>();
	}
}

