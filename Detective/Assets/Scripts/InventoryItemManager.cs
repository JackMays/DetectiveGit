using UnityEngine;
using System.Collections;

public class InventoryItemManager : ObjectManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PassInventoryTarget()
	{
		managerRef.ExecuteInventoryTarget(assignedObjectID);
	}
}
