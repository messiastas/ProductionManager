using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectorProperties {
	public List<int> roads = new List<int>();
	public bool isForbidden = false;
	public int indexX;
	public int indexY;

	public void SetIndexes(int iX, int iY)
	{
		indexX = iX;
		indexY = iY;
	}
}
