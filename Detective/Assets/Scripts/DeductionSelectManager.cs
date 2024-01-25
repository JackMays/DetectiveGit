using UnityEngine;
using System.Collections;

public class DeductionSelectManager : MonoBehaviour {

	DeductionSceneManager sceneManagerRef;

	string objectName  = "";
	string objectExamine = "";



	// Use this for initialization
	void Start () 
	{
		sceneManagerRef = GameObject.FindGameObjectWithTag("DeductionSceneManager").GetComponent<DeductionSceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignManager(string objName, string objExamine)
	{
		objectName = objName;
		objectExamine = objExamine;
	}

	public void PassObjNameSelection()
	{
		sceneManagerRef.SaveObjectSelection(objectName, objectExamine);
	}
}
