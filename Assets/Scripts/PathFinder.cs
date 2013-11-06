using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PathFinder
{
	static Hex targetTile;
	public static List<Hex> Dijkstra(Hex fromHex, Hex toHex, List<List<Hex>> map, List<List<bool>> boolMap) {
		return null;
	}
	
	public static List<Hex> BreadthFirstSearch(Hex source, List<List<Hex>> map, int moves) {
		Queue<Hex> queue = new Queue<Hex>();
		List<Hex> found = new List<Hex>();
		List<Hex> visited = new List<Hex>();
		source.Adjacent(map).ForEach(h => queue.Enqueue(h));
		Hex current = null;
		
		while(queue.Count > 0) {
			current = queue.Dequeue();
			if(DepthFirstSearch(source, current, map, moves).Count > 0 && !visited.Contains(current) && (current.Unit == null || current.Unit.Team == 2)) {
				found.Add(current);
				current.Adjacent(map).ForEach(h => queue.Enqueue(h));
			}
			visited.Add(current);
		}
		
		return found;
	}
	
	public static  List<Hex> DepthFirstSearch(Hex fromHex, Hex toHex, List<List<Hex>> map, int moves) {
		targetTile = toHex;
		List<Hex> resultList = DFS (fromHex, toHex, map, moves, new List<Hex>());
		if(toHex.Unit != null && fromHex.Unit.Team == toHex.Unit.Team) {
			return resultList;
		}
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

