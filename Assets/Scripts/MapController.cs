using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour {

	public List<List<SectorProperties>> sectors = new List<List<SectorProperties>>();

	public List<GameObject> routePoints = new List<GameObject>();
	public List<RouteData> routes = new List<RouteData>();

	// Use this for initialization
	void Start () {
		CreateSectors ();
	}

	void CreateSectors ()
	{
		for (int i = 0; i <= SharedVars.Inst.worldSize; i++) {
			List<SectorProperties> minisectors = new List<SectorProperties> ();
			for (int j = 0; j <= SharedVars.Inst.worldSize; j++) {
				minisectors.Add (new SectorProperties ());
				minisectors[j].SetIndexes(i,j);
			}
			sectors.Add (minisectors);
		}

	}

	public Vector3 RoutePointPositionNearBy(Vector3 pos)
	{
		float minDistance = 2f;
		Vector3 closestPoint = Vector3.zero;
		foreach(GameObject point in routePoints)
		{
			if(Vector3.Distance(pos,point.transform.position)<minDistance)
			{
				if(IsFreeFromRoutes(point))
				{
					minDistance = Vector3.Distance(pos,point.transform.position);
					closestPoint = point.transform.position;
				}
			}
		}
		return closestPoint;
	}

	public GameObject RoutePointOnPosition(Vector3 pos)
	{
		GameObject closestPoint = null;
		foreach(GameObject point in routePoints)
		{
			if(Vector3.Distance(pos,point.transform.position)<0.1f)
			{
					closestPoint = point;
			}
		}
		return closestPoint;
	}

	public List<RouteData> GetRoutesForManager(RouteManager rManager)
	{
		List<RouteData> routesForManager = new List<RouteData>();
		foreach (RouteData rData in routes)
		{
			if(rData.routeType!="internal" && rData.HasManager(rManager))
			{
				routesForManager.Add(rData);
			}
		}
		return routesForManager;
	}

	protected bool IsFreeFromRoutes(GameObject point)
	{
		bool isFree = true;
		foreach (RouteData route in routes)
		{
			if(route.routeType!="internal" && route.HasPoint(point)) 
			{
				isFree = false;
				return isFree;
			}
		}
		return isFree;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
