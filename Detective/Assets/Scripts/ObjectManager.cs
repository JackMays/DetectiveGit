using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

	protected int assignedObjectID;
	
	protected GameManager managerRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignObject(int id, GameManager manager)
	{
		assignedObjectID = id;
		managerRef = manager;
	}
}
