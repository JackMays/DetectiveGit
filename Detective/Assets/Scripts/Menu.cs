using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// loading the level from the Menu
	public void LoadScene(int scene)
	{
		SceneManager.LoadScene(scene);
	}
}
