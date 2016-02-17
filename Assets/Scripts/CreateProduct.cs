using UnityEngine;
using System.Collections;

public class CreateProduct : MonoBehaviour {

	public float startSpeed;
	// Use this for initialization
	void Start () {
		//InvokeRepeating("CreateNewProduct",1f,1f);
	}
	
	// Update is called once per frame
	public GameObject CreateNewProduct (GameObject product) {
		GameObject newProduct = Instantiate(product,this.transform.position,Quaternion.identity) as GameObject;
		newProduct.GetComponent<Rigidbody>().velocity = new Vector3(startSpeed+Random.Range(-startSpeed*0.25f,startSpeed*0.25f),Random.Range(-0.25f,0.25f),0);
		newProduct.GetComponent<Rigidbody>().angularVelocity = new Vector3(newProduct.GetComponent<Rigidbody>().velocity.y,-startSpeed,0);
		newProduct.transform.parent = this.transform;
		return newProduct;
	}
}
