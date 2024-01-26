using UnityEngine;
using System.Collections;

// Class for handling the buttons in a Deduction Scene
// communicating with Deduction Scene Manager to pass objects and associated output strings assigned to check them against each other for Deductions
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
	// initialise with a name and a examination description text that displays when selected
	public void AssignManager(string objName, string objExamine)
	{
		objectName = objName;
		objectExamine = objExamine;
	}

	//Passing object to Deduction Scene Manager
	// function for button press
	public void PassObjNameSelection()
	{
		sceneManagerRef.SaveObjectSelection(objectName, objectExamine);
	}
}
