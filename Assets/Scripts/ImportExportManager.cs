using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImportExportManager : MonoBehaviour {

	public GameObject[] import;
	public GameObject importPoint;
	public GameObject[] export;
	public float creationSpeed=1f;


	public Dictionary<string, int> needs =
		new Dictionary<string, int>(){};
	public Dictionary<string, int> needsGet =
		new Dictionary<string, int>(){};	
	
	public Dictionary<string, int> store =
		new Dictionary<string, int>(){};

	protected CreateProduct productCreator;

	public List<GameObject> productsObjects = new List<GameObject>();
	// Use this for initialization
	void Start () {
		int i=0;
		foreach (GameObject product in import)
		{
			string productName = product.GetComponent<ProductDescription>().productName;
			Debug.Log(productName);
			if(needs.ContainsKey(productName))
			{
				needs[productName] ++;
				//needsGet[productName] ++;
			} else 
			{
				needs.Add(productName,1);
				needsGet.Add(productName,0);
				//needsGet[productName] ++;
			}
			Instantiate(product,importPoint.transform.position+new Vector3(i*0.4f,0,0),Quaternion.identity);
			i++;
		}

		foreach (GameObject product in export)
		{
			string productName = product.GetComponent<ProductDescription>().productName;
			AddProductToStore(productName,0);
		}

		productCreator = gameObject.GetComponent<CreateProduct>();
		InvokeRepeating("CallProductCreator",creationSpeed,creationSpeed);
	}

	public void AddProductToStore(string product, int num)
	{
		if(!store.ContainsKey(product))
		{
			store.Add(product,0);
			//needsGet[productName] ++;
		}
		store[product]+=num;
	}

	
	// Update is called once per frame
	void CallProductCreator () {
		bool isAllConditions = true;
		foreach (string key in needs.Keys)
		{
			if(needs[key] > needsGet[key])
			{
				isAllConditions = false;
			}
		}
		if(isAllConditions)
		{
			foreach (GameObject product in export)
			{
				productsObjects.Add(productCreator.CreateNewProduct(product));
				store[product.GetComponent<ProductDescription>().productName] ++;
			}
			foreach (string key in needsGet.Keys)
			{
				if(key!="worker") needsGet[key] --;
			}
		}

	}

	public void RemoveFromStore(string productName,int num)
	{
		store[productName] -=num;
		int removedObjects = 0;
		Debug.Log("RemoveFromStore in " + GetComponent<RouteManager>().buildingType + "; productsObjects count: " + productsObjects.Count + "; Need to remove: " + num);
		while (productsObjects.Count>0 && removedObjects<num)
		{
			foreach(GameObject product in productsObjects)
			{
				if (product.GetComponent<ProductDescription>().productName==productName)
				{
					removedObjects++;
					productsObjects.Remove(product);
					Destroy(product);
					break;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//ProductDescription descr = null;
		//Debug.Log("get something");
		string descr = other.gameObject.GetComponent<ProductDescription>().productName;
		if(needs.ContainsKey(descr))
		{
			needsGet[descr] ++;
			//Debug.Log(descr+" get into the building");
			other.gameObject.GetComponent<WorkerDrugger>().isOnPlace = true;
			other.gameObject.GetComponent<WorkerDrugger>().currentPlace = gameObject;
			other.gameObject.GetComponent<WorkerDrugger>().workersOnPlace = needsGet[descr];
		}
	}

	void OnTriggerExit(Collider other)
	{
		string descr = other.gameObject.GetComponent<ProductDescription>().productName;
		if(needs.ContainsKey(descr))
		{
			needsGet[descr] --;
			//Debug.Log(descr+" get out of building");
			other.gameObject.GetComponent<WorkerDrugger>().isOnPlace = false;
			other.gameObject.GetComponent<WorkerDrugger>().currentPlace = null;
			other.gameObject.GetComponent<WorkerDrugger>().workersOnPlace = 0;
		}
	}
}
