using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TruckStationManager : MonoBehaviour {

	// Use this for initialization
	public GameObject carClass;

	protected int carsMoving = 0;
	protected ImportExportManager ieManager;
	protected RouteManager rManager;
	public Dictionary<string, RouteManager> orders =
		new Dictionary<string, RouteManager>(){};

	void Start () {
		ieManager = GetComponent<ImportExportManager>();
		rManager = GetComponent<RouteManager>();
		InvokeRepeating("StartCar",1,1);
	}

	public void MakeOrder(string product, RouteManager rManager)
	{
		if (product!="worker")
		{
			if(!orders.ContainsKey(product))
			{
				orders.Add(product,rManager);
				Debug.Log("Get order: "+product);
			}
		}
	}

	public void RemoveOrder(string product)
	{
		if (product!="worker")
		{
			if(!orders.ContainsKey(product))
			{
				orders.Remove(product);
			}
		}
	}
	
	// Update is called once per frame
	void StartCar () {
		
		if(carsMoving<ieManager.needsGet["worker"])
		{
			foreach (string order in orders.Keys)
			{
				if(ieManager.store.ContainsKey(order))
				{
					if(ieManager.store[order]>0)
					{
						Debug.Log("START CAR");
						GameObject car = Instantiate(carClass,this.transform.position,Quaternion.identity) as GameObject;
						car.transform.parent = this.transform;


						carsMoving++;
						int cargoGet = 0;
						if(ieManager.store[order]>car.GetComponent<Car>().cargo)
						{
							ieManager.store[order] -=car.GetComponent<Car>().cargo;
							cargoGet = car.GetComponent<Car>().cargo;
						} else if(ieManager.store[order]>0)
						{
							cargoGet=ieManager.store[order];
							ieManager.store[order] -=ieManager.store[order];

						}
						car.GetComponent<Car>().SetRoute(rManager,orders[order],order,"delieveringProduct",cargoGet);
					} else 
					{
						SearchForProductInStores(order);
					}
				} else 
				{
					SearchForProductInStores(order);
				}
			}
		}
	}

	void SearchForProductInStores(string order)
	{
		RouteManager targetManager = null;
		List<RouteData> routes = GameObject.FindObjectOfType<MapController>().GetRoutesForManager(rManager);

		foreach(RouteData route in routes)
		{
			targetManager = route.GetManagerWithProductAtStore(order,rManager);
			if(targetManager!=null) break;
		}
		if(targetManager!=null)
		{
			Debug.Log("START CAR");
			GameObject car = Instantiate(carClass,this.transform.position,Quaternion.identity) as GameObject;
			car.transform.parent = this.transform;

			car.GetComponent<Car>().SetRoute(rManager,targetManager,order,"searchingProduct",0);
			carsMoving++;
		}
	}

	public void CarReturned()
	{
		carsMoving--;
	}
}
