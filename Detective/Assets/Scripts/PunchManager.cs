using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PunchManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

	GameManager managerRef;

	BoxCollider2D box;

	string[] buttonModes = new string[3]{"Punch", "Grab", "Ram"};

	string defaultText = "Muscle";

	Vector3 originalPos;

	int currentMode = 0;

	float isDownTimer = 0;
	float isDownCap = 100.0f;
	float isDownIncrement = 1.0f;


	Text buttonText;

	bool isMuscleActive = false;
	bool isDown = false;
	bool isDragDisable = false;

	// Use this for initialization
	void Awake () 
	{
		managerRef = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
		box = GetComponent<BoxCollider2D>();
		buttonText = GetComponentInChildren<Text>();

		box.enabled = false;

	}

	void Start ()
	{
		originalPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isDown)
		{
			Debug.Log (isDownTimer);

			if (isDownTimer < isDownCap)
			{
				isDownTimer += isDownIncrement;
			}
			else
			{
				gameObject.transform.position = Input.mousePosition;

				// Get button mode to ignore the release of the mouse when it is called on dragging end
				if (!isDragDisable)
				{
					isDragDisable = true;
				}
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		gameObject.transform.position = originalPos;
		isDown = false;
		isDragDisable = false;
		isDownTimer = 0.0f;
		managerRef.ExecuteMuscleAction(buttonModes[currentMode]);
	}

	public void ToggleActive (bool active, bool muscle)
	{
		isMuscleActive = active;

		if (isMuscleActive)
		{
			buttonText.text = buttonModes[currentMode];
			if (muscle)
			{
				box.enabled = true;
			}
		}
		else
		{
			buttonText.text = defaultText;
			currentMode = 0;
			box.enabled = false;
		}
	}

	public void SwitchButtonMode()
	{
		if (isMuscleActive)
		{
			if (!isDragDisable)
			{
				++currentMode;

				if (currentMode == buttonModes.Length)
				{
					currentMode = 0;
				}

				buttonText.text = buttonModes[currentMode];
			}
			else
			{
				// Single instance of release when finished dragging is ignored
				isDragDisable = false;
			}
		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (isMuscleActive)
		{
			isDown = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//Reset button if moved
		if (isDownTimer >= isDownCap)
		{
			gameObject.transform.position = originalPos;
		}

		isDown = false;
		isDownTimer = 0.0f;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (isDownTimer < isDownCap)
		{
			isDown = false;
			isDownTimer = 0.0f;
		}
	}

	public bool HasMuscleActive()
	{
		return isMuscleActive;
	}
}
