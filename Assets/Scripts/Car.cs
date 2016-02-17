using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Car : MonoBehaviour {
	public float speed = 2f;
	public int cargo = 5;

	protected RouteManager startManager;
	protected RouteManager destinationManager;

	protected Vector3 startPosition = new Vector3();
	protected Vector3 destinationPosition = new Vector3();

	protected string status = "onBase";
	protected string productNeed;
	protected int productGet = 0;

	// Use this for initialization
	void Start () {
	
	}

	public void SetRoute(RouteManager start, RouteManager end, string product, string currentStatus, int cargoGet)
	{
		startManager = start;
		destinationManager = end;
		startPosition = startManager.transform.position;
		destinationPosition = destinationManager.transform.position;
		transform.rotation = Quaternion.LookRotation((destinationPosition - startPosition).normalized);
		status = currentStatus;
		productNeed = product;
		productGet = cargoGet;
	}

	public 
	
	// Update is called once per frame
	void Update () {
		switch (status)
		{
			case "onBase":

				break;
			case "searchingProduct":
				if(Vector3.Distance(transform.position,destinationPosition)>0.2f)
				{
					transform.position = Vector3.MoveTowards(transform.position, destinationPosition, speed*Time.deltaTime);
				} else 
				{
					ImportExportManager ieManager = destinationManager.GetComponent<ImportExportManager>();
					if(ieManager.store[productNeed]>cargo)
					{
						//ieManager.store[productNeed] -=cargo;
						ieManager.RemoveFromStore(productNeed,cargo);
						productGet+=cargo;
					} else if(ieManager.store[productNeed]>0)
					{
						productGet=ieManager.store[productNeed];
						ieManager.RemoveFromStore(productNeed, ieManager.store[productNeed]);
						//ieManager.store[productNeed] -=ieManager.store[productNeed];

					}
					status = "retrievingProduct";
				}
				break;
			case "retrievingProduct":
				if(Vector3.Distance(transform.position,startPosition)>0.2f)
				{
					transform.position = Vector3.MoveTowards(transform.position, startPosition, speed*Time.deltaTime);
				} else 
				{
					status = "onBase";
					startManager.GetComponent<ImportExportManager>().AddProductToStore(productNeed,productGet);
					productGet=0;
					startManager.GetComponent<TruckStationManager>().CarReturned();
				}
				break;
			case "delieveringProduct":
				if(Vector3.Distance(transform.position,destinationPosition)>0.2f)
				{
					transform.position = Vector3.MoveTowards(transform.position, destinationPosition, speed*Time.deltaTime);
				} else 
				{
					destinationManager.GetComponent<ImportExportManager>().needsGet[productNeed] +=productGet;
					productGet=0;
					status = "onBack";
				}
				break;
			case "onBack":
				if(Vector3.Distance(transform.position,startPosition)>0.2f)
				{
					transform.position = Vector3.MoveTowards(transform.position, startPosition, speed*Time.deltaTime);
				} else 
				{
					status = "onBase";
					startManager.GetComponent<TruckStationManager>().CarReturned();
				}
				break;
		}
	}
}
