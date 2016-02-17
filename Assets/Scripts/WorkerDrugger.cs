using UnityEngine;
using System.Collections;

public class WorkerDrugger : MonoBehaviour {
	public bool isOnPlace = false;
	public GameObject currentPlace;
	public int workersOnPlace = 0;


	protected bool isMovingOnPlace = false;
	protected Vector3 startPos = new Vector3();
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void OnMouseDrag () {
		Vector3 v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		v3 = Camera.main.ScreenToWorldPoint(v3);
		this.transform.position = new Vector3(v3.x, v3.y, this.transform.position.z);
	}

	void OnMouseUp() {
		if(isOnPlace && currentPlace!=null)
		{
			isMovingOnPlace = true;
		}
	}

	void Update ()
	{
		if(isMovingOnPlace)
		{
			if(Vector3.Distance(transform.position,currentPlace.transform.position+new Vector3(-0.5f-workersOnPlace*0.75f,0,0))>0.1f)
			{
				//Debug.Log("workerMoving");
				transform.position = Vector3.SmoothDamp(transform.position, currentPlace.transform.position+new Vector3(-0.5f-workersOnPlace*0.75f,0,0), ref velocity, 0.1f);
			} else 
			{
				isMovingOnPlace = false;
			}
		}
	}
}
