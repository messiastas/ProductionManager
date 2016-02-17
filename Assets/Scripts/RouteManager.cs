using UnityEngine;
using System.Collections;

public class RouteManager : MonoBehaviour {

	public GameObject[] routePoints;
	public string buildingType;
	// Use this for initialization
	void Start () {
		for (int i=0;i<routePoints.Length;i++)
		{
			MapController mapController = GameObject.FindObjectOfType<MapController>();
			mapController.routePoints.Add(routePoints[i]);
			if(i>0)
			{
				for (int j=0;j<i;j++)
				{
					RouteData route = new RouteData();
					route.SetParams(routePoints[i],routePoints[j], "internal");
					mapController.routes.Add(route);
				}
			}
		}
	}

	public void CheckNeedsOnNewRoute(RouteManager other)
	{
		if(other.buildingType=="truckstation")
		{
			foreach (string key in GetComponent<ImportExportManager>().needs.Keys)
			{
				other.GetComponent<TruckStationManager>().MakeOrder(key,this);
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
