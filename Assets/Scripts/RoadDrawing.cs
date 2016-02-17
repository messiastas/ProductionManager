using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadDrawing : MonoBehaviour {
	
	public GameObject road;
	public bool isDrawingMode = true;
	public bool isDrawing = false;

	protected List<GameObject> roads = new List<GameObject>();
	protected List<Vector3> currentPositions = new List<Vector3>();
	protected MapController mapController;
	protected bool isFinishFound = false;

	void Start () {
		mapController = GetComponent<MapController>();

	}



	void Update () {
		if(isDrawingMode)
		{
			if ( Input.GetMouseButtonDown (0)) {
				CheckStartPoint(Camera.main.ScreenToWorldPoint (Input.mousePosition));
			} else if(isDrawing && Input.GetMouseButton(0))
			{
				ContinueRoad (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			} else if(Input.GetMouseButtonUp(0))
			{				
				if(isDrawing)
				{
					if(!isFinishFound)
					{
						GameObject removeThis = roads[roads.Count-1];
						roads.RemoveAt(roads.Count-1);
						Destroy(removeThis);
					} else 
					{
						RouteData route = new RouteData();
						route.SetParams(mapController.RoutePointOnPosition(currentPositions[0]),mapController.RoutePointOnPosition(currentPositions[1]), "external");
						mapController.routes.Add(route);
					}
					isDrawing = false;
					isFinishFound = false;
				}

			}
		}

	}

	protected void CheckStartPoint(Vector3 point)
	{
		
		Vector3 fixedPoint = GetClosestPoint(point);
		if(fixedPoint!=Vector3.zero)
		{
			isDrawing = true;
			CreateRoad(fixedPoint);
		}
	}

	Vector3 GetClosestPoint(Vector3 point)
	{
		float pointX =point.x;
		float pointY = point.y;
		Vector3 startPoint = new Vector3(pointX,pointY,0);

		return mapController.RoutePointPositionNearBy(startPoint);
	}

	void CreateRoad(Vector3 point)
	{
		currentPositions = new List<Vector3>();
		GameObject trackSphere = Instantiate(road, point,Quaternion.identity) as GameObject;
		trackSphere.transform.parent = transform;
		roads.Add(trackSphere);
		currentPositions.Add(point);
		currentPositions.Add(point);
		trackSphere.GetComponent<LineRenderer>().SetPosition(0,currentPositions[currentPositions.Count-1]);
		//sectors[sectorX][sectorY].roads.Add(roads.Count);
	}

	void ContinueRoad(Vector3 point)
	{
		Vector3 closestPoint = GetClosestPoint(point);
		if(closestPoint==Vector3.zero)
		{
			int sectorX = Mathf.RoundToInt (point.x);
			int sectorY = Mathf.RoundToInt (point.y);
			closestPoint = new Vector3(sectorX, sectorY, 0f);
			isFinishFound = false;
		} else 
		{
			isFinishFound = true;
		}

		if(Vector3.Distance(closestPoint,currentPositions[0])>=1f)
		{
			currentPositions[1] = closestPoint;
			roads[roads.Count-1].GetComponent<LineRenderer>().SetVertexCount(currentPositions.Count);
			for (int i = 0; i < currentPositions.Count; i++){
				roads[roads.Count-1].GetComponent<LineRenderer>().SetPosition(i, currentPositions[i]);
			}
			//roads[roads.Count-1].GetComponent<LineRenderer>().SetVertexCount(currentPositions.Count);
			//roads[roads.Count-1].GetComponent<LineRenderer>().SetPosition(currentPositions.Count-1,currentPositions[currentPositions.Count-1]);
		}
	}
}
