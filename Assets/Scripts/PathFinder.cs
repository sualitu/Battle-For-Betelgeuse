using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PathFinder
{
	static Hex targetTile;
	public static List<Hex> Dijkstra(Hex fromHex, Hex toHex, List<List<Hex>> map, List<List<bool>> boolMap) {
		return null;
	}
	
	public static  List<Hex> DepthFirstSearch(Hex fromHex, Hex toHex, List<List<Hex>> map, int moves) {
		targetTile = toHex;
		List<Hex> resultList = DFS (fromHex, toHex, map, moves, new List<Hex>());
		if(toHex.Unit != null && fromHex.Unit.Team == toHex.Unit.Team) {
			return resultList;
		}
		resultList.ForEach(h => h.renderer.material.color = Color.green);
		return resultList;
	}
	
	static List<Hex> DFS(Hex thisHex, Hex toHex, List<List<Hex>> map, int moves, List<Hex> acc) {
		if(thisHex.GridPosition != toHex.GridPosition) {
			if(moves < 1) {
				return new List<Hex>();
			} else {
				List<Hex> adj = thisHex.Adjacent(map).FindAll(h => (h.Unit == null || h.GridPosition == targetTile.GridPosition));
				Hex nextHex = null;
				foreach(Hex h in adj) {
						if(nextHex != null) {
							float oldD = Mathf.Sqrt(
											Mathf.Pow(nextHex.GridPosition.x - toHex.GridPosition.x, 2) +
											Mathf.Pow(nextHex.GridPosition.y - toHex.GridPosition.y, 2)
								);
							float newD = Mathf.Sqrt(
											Mathf.Pow(h.GridPosition.x - toHex.GridPosition.x, 2) +
											Mathf.Pow(h.GridPosition.y - toHex.GridPosition.y, 2)
								);
							
							if(newD < oldD) {
								nextHex = h;
							} 
						} else {
							nextHex = h;
						}
				}
				acc.Add(nextHex);
				return DFS (nextHex, toHex, map, moves-1, acc);
			}
		} else { return acc; }
	}
}

