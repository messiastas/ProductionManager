using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouteData  {

	public GameObject point1;
	public GameObject point2;
	public RouteManager manager1;
	public RouteManager manager2;
	public string routeType;
	public bool hasCar = false;

	public void SetParams(GameObject p1, GameObject p2, string rT)
	{
		point1 = p1;
		manager1 = point1.GetComponentInParent<RouteManager>();
		point2 = p2;
		manager2 = point2.GetComponentInParent<RouteManager>();
		Debug.Log("Route manager 1: "+ manager1.buildingType);
		Debug.Log("Route manager 2: "+ manager2.buildingType);
		routeType = rT;
		if(routeType!="internal")
		{
			TellToManagersRoadCreated();
		}

	}

	public bool HasPoint(GameObject point)
	{
		if(point == point1 || point == point2)
		{
			return true;
		}
		return false;
	}

	public bool HasManager(RouteManager manager)
	{
		if(manager == manager1 || manager == manager2)
		{
			return true;
		}
		return false;
	}

	public void TellToManagersRoadCreated()
	{
		manager1.CheckNeedsOnNewRoute(manager2);
		manager2.CheckNeedsOnNewRoute(manager1);
	}

	public RouteManager GetManagerWithProductAtStore(string product, RouteManager orderFrom)
	{
		Debug.Log("Manager with product searching:  " + product);
		RouteManager currentManager = null;
		if(orderFrom==manager1)
		{
			currentManager = manager2;
		} else if(orderFrom==manager2)
		{
			currentManager = manager1;
		} else 
		{
			return null;
		}
		Dictionary<string, int> store = currentManager.GetComponent<ImportExportManager>().store;
		if(store.ContainsKey(product) && store[product]>0)
		{
			Debug.Log("Manager with product found");
			return currentManager;
		}
		return null;
	}
}
