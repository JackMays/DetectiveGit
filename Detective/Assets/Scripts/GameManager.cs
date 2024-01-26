using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List<Button> actionButtons;
	public List<string> subInteractButtons;
	public List<string> subInventoryButtons;
	public List<string> subUseTargetButtons;
	public List<string> subInventTypeButtons;

	public string EmptyScene;
	public string EmptyNPC;
	public string EmptyItems;
	public string EmptyClues;
	public string EmptyMove;

	public float panelOffset;

	public Canvas coreUI;
	public Scrollbar scrollBar;
	public ScrollRect subButtonScroll;
	public GameObject subButtonPanel;
	public Typewriter eventTypewriter;
	public Text muscleText;

	public Button muscleButton;
	public Button mindButton;
	public Button prevMuscleScroll;
	public Button nextMuscleScroll;
	public Button continueChainButton;
	public Button transitionButton;
	public Button endingButton;

	public SceneContent sceneContent;
	public ConclusionManager conclusion;

	List<int> inventory = new List<int>();
	List<string> activeDialogueChainList = new List<string>();
	List<Clue> clues = new List<Clue>();
	List<Item> items = new List<Item>();
	List<Suspect> suspects = new List<Suspect>();
	List<MuscleOption> allMuscleOptions = new List<MuscleOption>();
	List<MuscleOption> activeMuscleOptions = new List<MuscleOption>();

	string chainKeyword = "";

	int primaryEntityTarget = 0;
	int secondaryEntityTarget = 0;
	int secondaryEntityLocation = 0;
	int currentMuscleOption = 0;
	int muscleChain = 0;
	int textChain = 0;

	float defaultHeight = 0.0f;

	bool isSubButtonsActive = false;
	bool isMindOrMuscle = true;
	bool isMuscleChain = false;
	bool isIntroChain = false;
	bool isExamineChain = false;
	bool isDialogueChain = false;
	bool isConclusionChain = false;
	bool isEndingChain = false;
	bool isMuscleEnd = false;
	bool isStoredMuscleToggle = false;
	bool isItemMuscle = false;

	// Use this for initialization
	void Start () 
	{

		// Keep this between for OnLevalLoad
		DontDestroyOnLoad(this.gameObject);
		// Keep Univeral UI
		DontDestroyOnLoad(coreUI.gameObject);
		// Keep Typewriters Text Box
		//DontDestroyOnLoad(eventTypewriter.gameObject);

		// Get the intro text of the very first level on initialisation
		eventTypewriter.TypeInTypewriter(sceneContent.GetSceneIntro());

		isIntroChain = sceneContent.HasIntroChain();

		muscleText.text = "";
		muscleText.gameObject.SetActive(false);
		prevMuscleScroll.gameObject.SetActive(false);
		nextMuscleScroll.gameObject.SetActive (false);
		subButtonScroll.gameObject.SetActive(false);
		scrollBar.gameObject.SetActive(false);
		mindButton.gameObject.SetActive(false);
		continueChainButton.gameObject.SetActive(false);
		transitionButton.gameObject.SetActive(false);
		endingButton.gameObject.SetActive(false);

		defaultHeight = subButtonPanel.GetComponent<RectTransform>().sizeDelta.y;

		//inventory.Add(0);
		//inventory.Add (1);

		/*items.Add (sceneContent.GetItemByID(0));
		items.Add (sceneContent.GetItemByID(1));
		items.Add (sceneContent.GetItemByID(2));*/

		string caseName = SceneManager.GetActiveScene().name; 
		string conclusionPath = sceneContent.GetTxtSubFolder() + "Conclusion/" + caseName + "/";
		string musclePath = sceneContent.GetTxtSubFolder() + "Muscle/" + caseName + "/";
		string muscFrameworkPath = musclePath + "Muscle Framework ";

		Debug.Log(musclePath);
		Debug.Log(muscFrameworkPath);

		conclusion.InitialiseConclusion(conclusionPath);

		int muscleIndex = 0;

		TextAsset muscleFile = Resources.Load(muscFrameworkPath + muscleIndex) as TextAsset;

		while (muscleFile != null)
		{
			string[] muscleArray = muscleFile.text.Split('\n');

			allMuscleOptions.Add (new MuscleOption(musclePath, muscleArray[0], muscleArray[1], muscleArray[2], muscleArray[3], muscleArray[4]));

			Debug.Log(muscleArray[0]);

			++muscleIndex;
			muscleFile = Resources.Load(muscFrameworkPath + muscleIndex) as TextAsset;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Update logic for handling state changes
		// between mind and muscle modes; Mind = true and Muscle = false
		// handling main event textbox changes, transitions and updates and typewriter effect changes
		if (!SceneManager.GetActiveScene().name.Contains("Deduction"))
		{
			// Mind
			if (isMindOrMuscle)
			{
				if (eventTypewriter.HasTypingChanged())
				{

					// Hide UI during text to prevent excessive button presses
					// causing erroneous characters to spill into the text
					if (eventTypewriter.IsTypewriterComplete())
					{
						ToggleInvestigationModeUI(true);

						if (!isIntroChain && !isExamineChain && !isDialogueChain)
						{
							// Check for if all items for deduction have been obtained only when typewriter is typed
							LoadDeduction();
						}
					}
					else
					{

						ToggleInvestigationModeUI(false);
					}

					eventTypewriter.StateChangeAddressed();
				}

			}
			// Muscle
			else
			{
				if (eventTypewriter.gameObject.activeSelf && eventTypewriter.IsTypewriterComplete())
				{
					// Toggling UI change
					if (isMuscleEnd)
					{
						SetMindOrMuscle(true);
						isMuscleEnd = false;
					}

					if (Input.GetMouseButtonDown(0))
					{
						eventTypewriter.gameObject.SetActive(false);
						ToggleInvestigationModeUI(true);
						/*if (activeMuscleOptions.Count != 0)
						{
							muscleText.text = activeMuscleOptions[currentMuscleOption].GetOptionDislay();
						}
						else
						{
							muscleText.text = "No Physical Openings Available.";
						}*/

					}
				}
			}

		}
		else
		{
			// Monitoring text chains in conclusion and ending events
			// And/or mind-muscle changes
			if (eventTypewriter.HasTypingChanged())
			{
				if (eventTypewriter.IsTypewriterComplete())
				{
					if (isConclusionChain || isEndingChain)
					{
						continueChainButton.gameObject.SetActive(true);
					}
					else
					{
						continueChainButton.gameObject.SetActive(false);
					}

					if (isStoredMuscleToggle)
					{
						muscleButton.gameObject.SetActive(true);
						isStoredMuscleToggle = false;
					}
					// If muscle sequence is due to be over (such as due to successful muscle option) return to Mind
					if (isMuscleEnd)
					{
						SetMindOrMuscle(true);
						muscleButton.gameObject.SetActive(false);
						endingButton.gameObject.SetActive(true);
						isMuscleEnd = false;
					}
				}
				else
				{
					continueChainButton.gameObject.SetActive(false);
					muscleButton.gameObject.SetActive(false);
				}

				eventTypewriter.StateChangeAddressed();
			}
		}
	}



	// make scrollrect and panel panel active for populating with buttons
	void PrepareScrollRect()
	{
		// wipe buttons from panel for new ones
		WipePanelButtons();

		scrollBar.gameObject.SetActive(true);
		subButtonScroll.gameObject.SetActive(true);
		

	}

	void ResizePanel(float size)
	{
		float offsetSize = size + panelOffset;

		subButtonPanel.GetComponent<RectTransform>().sizeDelta += new Vector2(offsetSize, defaultHeight);
	}

	void ToggleInvestigationModeUI (bool toggle)
	{
		// Sub buttons as part of mind mode need to follow toggle rules
		// whereas vice versa, mindbutton has to follow toggle rules as it is active in Muscle
		if (isMindOrMuscle)
		{
			if (!isIntroChain && !isExamineChain && !isDialogueChain)
			{
				for (int i = 0; i < actionButtons.Count; ++i)
				{
					actionButtons[i].gameObject.SetActive(toggle);
				}


			}
			else
			{
				for (int i = 0; i < actionButtons.Count; ++i)
				{
					actionButtons[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			// Ignore mind button if theres a chain so players cannot leave Muscle
			if (!isMuscleChain)
			{
				mindButton.gameObject.SetActive(toggle);
			}
			else
			{
				mindButton.gameObject.SetActive(false);
			}

			muscleText.gameObject.SetActive(toggle);
			if (activeMuscleOptions.Count > 1)
			{
				prevMuscleScroll.gameObject.SetActive(toggle);
				nextMuscleScroll.gameObject.SetActive (toggle);
			}
		}
		// When toggled on, the panel and scroll only need to be on if it has buttons
		// Whereas on toggled off, they can just be confirmed as off.
		if (toggle)
		{
			// if there are sub buttons contained in them, the panel and scroll bar should show on toggle
			// unless theres a chain that requires the UI to be shut off
			if (isSubButtonsActive && !isIntroChain && !isExamineChain && !isDialogueChain)
			{
				scrollBar.gameObject.SetActive(true);
				subButtonScroll.gameObject.SetActive(true);
			}

			if (isIntroChain || isExamineChain || isDialogueChain)
			{
				continueChainButton.gameObject.SetActive(true);
			}

		}
		else
		{
			scrollBar.gameObject.SetActive(false);
			subButtonScroll.gameObject.SetActive(false);

			continueChainButton.gameObject.SetActive(false);
		
		}
		if (!isIntroChain && !isExamineChain && !isDialogueChain)
		{
			muscleButton.gameObject.SetActive(toggle);
		}
		else
		{
			muscleButton.gameObject.SetActive(false);
		}
	}
	// Empty buttons are for when the player has no items/clues/moves etc
	// Loads a greyed out button with appropriate button text to describe they dont have anything
	void LoadEmptyButton(string buttonText)
	{
		GameObject subButton = Instantiate(Resources.Load("Prefabs/DummyDuplicateButton")) as GameObject;
		subButton.transform.SetParent(subButtonPanel.transform);
		
		Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
		subButtonTextCompo.text = buttonText;

		subButton.GetComponent<Button>().interactable = false;

		ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
	}

	// checks if all evidence is gathered; consisting of items and clues
	// If so, loads suspects and presents trransition to Deduction phase Scene
	void LoadDeduction()
	{
		bool hasAllEvidence = true;

		if (sceneContent.GetDeductionItemReqCount() == inventory.Count &&
			sceneContent.GetDeductionClueReqCount() == clues.Count)
		{
			bool evidenceFound = false;

			foreach (int inventID in inventory)
			{
				evidenceFound = sceneContent.VerifyItem(inventID);

				if (!evidenceFound)
				{
					hasAllEvidence = false;
					break;
				}	 
			}

			if (!hasAllEvidence)
			{
				return;
			}

			foreach (Clue clue in clues)
			{
				evidenceFound = sceneContent.VerifyClue(clue.GetClueID());

				if (!evidenceFound)
				{
					hasAllEvidence = false;
				}
			}

			if (hasAllEvidence)
			{
				foreach (int inventID in inventory)
				{
					items.Add(sceneContent.GetItemByID(inventID)); 
				}

				for (int i = 0; i < sceneContent.GetMasterLocationNameCount(); ++i)
				{
					for (int j = 0; j < sceneContent.GetNPCCountSpecific(i); ++j)
					{
						NPC npc = sceneContent.GetSpecificNPC(i, j);

						suspects.Add (new Suspect(npc.GetName(), npc.GetTalkIntro()));
					}
				}

				DeductionHide();
				transitionButton.gameObject.SetActive(true);

			}
		}
		else
		{
			return;
		}
	}

	// Type event text with muscle option wipe to ensure list is clear
	public void TypeEventTextWithWipe(string text)
	{
		if (activeMuscleOptions.Count > 0)
		{
			activeMuscleOptions.Clear();
		}

		eventTypewriter.TypeInTypewriter(text);
	}

	public void WipePanelButtons()
	{

		// Wipe buttons from last sub button menu call
		foreach (Button button in subButtonScroll.GetComponentsInChildren<Button>())
		{
			Destroy(button.gameObject);
		}

		subButtonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, defaultHeight);
		
		isSubButtonsActive = false;
		scrollBar.gameObject.SetActive(false);
		subButtonScroll.gameObject.SetActive(false);
	}
	// Mind = true, Muscle = False
	public void SetMindOrMuscle(bool isMorM)
	{
		WipePanelButtons();
		// Set active/inactive based on is Mind OR Muscle
		eventTypewriter.HideText(isMorM);

		if (!SceneManager.GetActiveScene().name.Contains("Deduction"))
		{

			for (int i = 0; i < actionButtons.Count; ++i)
			{
				actionButtons[i].gameObject.SetActive(isMorM);
			}

			// ignore mind button if chain to retain musclemode
			if (!isMuscleChain)
			{
				mindButton.gameObject.SetActive(!isMorM);

				if (isMorM)
				{
					isItemMuscle = false;
				}

			}
		}
		else
		{
			muscleButton.GetComponent<PunchManager>().ToggleActive(!isMorM, true);
			ToggleMuscleButton(true);
		}

		// ignore muscle button if chain to retain musclemode
		if (!isMuscleChain)
		{
			bool hasMuscle = (activeMuscleOptions.Count > 0) ? true : false;
			muscleButton.GetComponent<PunchManager>().ToggleActive(!isMorM, hasMuscle);
			//mindButton.gameObject.SetActive(!isMorM);
		}

		if (!isMorM)
		{
			muscleText.gameObject.SetActive(true);

			if (activeMuscleOptions.Count != 0)
			{
				muscleText.text = activeMuscleOptions[currentMuscleOption].GetOptionDisplay();

				if (activeMuscleOptions.Count > 1)
				{
					prevMuscleScroll.gameObject.SetActive(true);
					nextMuscleScroll.gameObject.SetActive (true);
				}
				else
				{
					prevMuscleScroll.gameObject.SetActive(false);
					nextMuscleScroll.gameObject.SetActive (false);
				}
			}
			else
			{
				muscleText.text = "No Physical Openings Available.";
				prevMuscleScroll.gameObject.SetActive(false);
				nextMuscleScroll.gameObject.SetActive (false);
			}
		}
		else
		{
			muscleText.text = "";
			currentMuscleOption = 0;
			muscleText.gameObject.SetActive(false);
		}

		isMindOrMuscle = isMorM;
	}

	// Hide the investigation interaction buttons so the decuction ones take their place.
	public void DeductionHide()
	{
		foreach(Button button in actionButtons)
		{
			button.gameObject.SetActive(false);
		}

		muscleButton.gameObject.SetActive(false);

		WipePanelButtons();
	}

	public void GoToDeduction()
	{
		//eventTypewriter.ManualClear();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name + " Deduction");
		transitionButton.gameObject.SetActive(false);
	}

	// Loading Panel Buttons based on context and Scene state
	public void LoadSubButtons (int type)
	{
		GameObject subButton = null;

		PrepareScrollRect();

        // // Instantiate and assign button text to Item buttons respresenting those in the inventory
        if (type == 1)
		{
			// if there isnt any items load an empty
			// if there is check if they are not all inactive or picked up
			if (sceneContent.GetItemCountInLocation() == 0)
			{
				LoadEmptyButton(EmptyScene);
			}
			else
			{
				for (int i = 0; i < sceneContent.GetItemCountInLocation(); ++i)
				{
					if (!sceneContent.GetItem(i).HasBeenPickedUp() && sceneContent.GetItem(i).HasBeenActivated())
					{
						break;
					}

					// if we have met the end of the item list without the above break triggering
					// No items are active and the emty button is needed
					if ( i == sceneContent.GetItemCountInLocation() - 1)
					{
						LoadEmptyButton(EmptyScene);
					}
				}
			}

				for (int i = 0; i < sceneContent.GetItemCountInLocation(); ++i)
				{
					if (!sceneContent.GetItem(i).HasBeenPickedUp() && sceneContent.GetItem(i).HasBeenActivated())
					{
						subButton = Instantiate(Resources.Load("Prefabs/ItemSceneInteractButton")) as GameObject;
						subButton.transform.SetParent(subButtonPanel.transform);
						
						Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
						subButtonTextCompo.text = sceneContent.GetItem(i).GetItemName();

						InteractItemManager interactManager = subButton.GetComponent<InteractItemManager>();
						interactManager.AssignObject(sceneContent.GetItem(i).GetItemID(), this);
						
						if (subButton)
						{
							ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
						}
					}
				}
			}
		
		// Instantiate and assign text to NPC buttons based on NPCs in the Scene
		else if (type == 2)
		{
			if (sceneContent.GetNPCCountInScene() == 0)
			{
				LoadEmptyButton(EmptyNPC);
			}
			else
			{
				for (int i = 0; i < sceneContent.GetNPCCountInScene(); ++i)
				{
					subButton = Instantiate(Resources.Load("Prefabs/NPCSceneButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = sceneContent.GetNPC(i).GetName();

					subButton.GetComponent<NPCManager>().AssignObject(i, this);

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
				}
			}
		}
		// Instantiate and assign item text to Item buttons respresenting those in the inventory
		else if (type == 3)
		{
			if (inventory.Count == 0)
			{
				LoadEmptyButton(EmptyItems);
			}
			else
			{
				for (int i = 0; i < inventory.Count; ++i)
				{
					subButton = Instantiate(Resources.Load("Prefabs/ItemInventButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);

					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = sceneContent.GetItemByID(inventory[i]).GetItemName();

					subButton.GetComponent<InventoryItemManager>().AssignObject(inventory[i], this);

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
				}
			}
		}
		// Instantiate and assign button text to Move Location (load scene) buttons for actve scene/location
		else if (type == 4)
		{
			if (sceneContent.GetMoveCountInLocation() == 0)
			{
				LoadEmptyButton(EmptyMove);
			}
			else
			{

				for (int i = 0; i < sceneContent.GetMoveCountInLocation(); ++i)
				{
					if (sceneContent.GetMove(i).CanTravelToScene())
					{
						break;
					}
					
					// if we have met the end of the move list without the above break triggering
					// No move locations are active and the empty button is needed
					if ( i == sceneContent.GetMoveCountInLocation() - 1)
					{
						LoadEmptyButton(EmptyMove);
					}
				}
			}

			for (int i = 0; i < sceneContent.GetMoveCountInLocation(); ++i)
			{
				if (sceneContent.GetMove(i).CanTravelToScene())
				{
					subButton = Instantiate(Resources.Load("Prefabs/MoveButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);

					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = sceneContent.GetMove(i).GetMoveName();

					// Assign buttons manager with the scene it should load within sceneContent's list
					MoveLocationManager moveManager = subButton.GetComponent<MoveLocationManager>();
					moveManager.AssignMoveLocation(sceneContent.GetMove(i).GetLocationID(), this);

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
				}
			}
		}
		// Instantiate and assign button text for Interact Item Actions
		else if (type == 5)
		{
			for (int i = 0; i < subInteractButtons.Count; ++i)
			{
				subButton = Instantiate(Resources.Load("Prefabs/SubInteractButton")) as GameObject;
				subButton.transform.SetParent(subButtonPanel.transform);
				
				Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
				subButtonTextCompo.text = subInteractButtons[i];
				
				subButton.GetComponent<InteractActionManager>().AssignManager(i, this);

				if (subButton)
				{
					ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
				}
			}

			TypeEventTextWithWipe(sceneContent.GetItemByID(primaryEntityTarget).GetInteractText());
		}
        // Instantiate and assign button text for Inventory Item Actions
        else if (type == 6)
		{
			for (int i = 0; i < subInventoryButtons.Count; ++i)
			{
				subButton = Instantiate(Resources.Load("Prefabs/SubInventoryButton")) as GameObject;
				subButton.transform.SetParent(subButtonPanel.transform);
				
				Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
				subButtonTextCompo.text = subInventoryButtons[i];
				
				subButton.GetComponent<InventoryActionManager>().AssignManager(i, this);

				if (subButton)
				{
					ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
				}
			}
			
			// The event box displays the description of the first/rpimary item target while the player selects an Interact action.
			TypeEventTextWithWipe(sceneContent.GetItemByID(primaryEntityTarget).GetInteractText());
		}
        // instantiate buttons and assign button text for deciding whether Use will target inventory or items in the scene
        else if (type == 7)
		{
			for (int i = 0; i < subUseTargetButtons.Count; ++i)
			{
				subButton = Instantiate(Resources.Load("Prefabs/UseLocationButton")) as GameObject;
				subButton.transform.SetParent(subButtonPanel.transform);
				
				Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
				subButtonTextCompo.text = subUseTargetButtons[i];
				
				subButton.GetComponent<UseTargetManager>().AssignManager(i, this);

				if (subButton)
				{
					ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
				}
			}

		}
        // instantiate buttons and assign button text for target items in the scene for the Use Button
        else if (type == 8)
		{
			for (int i = 0; i < sceneContent.GetItemCountInLocation(); ++i)
			{
				if (!sceneContent.GetItem(i).HasBeenPickedUp() && sceneContent.GetItem(i).HasBeenActivated())
				{

					subButton = Instantiate(Resources.Load("Prefabs/ItemSceneUseButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = sceneContent.GetItem(i).GetItemName();
					
					subButton.GetComponent<UseTargetManager>().AssignManager(sceneContent.GetItem(i).GetItemID(), this);

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
				}
			}
			
		}
        // Instantiate buttons and assign button text for target items in the inventory with the Use Button
        else if (type == 9)
		{
			for (int i = 0; i < inventory.Count; ++i)
			{
				// Dont list the item selected in the previous inventory buttons
				if (inventory[i] != primaryEntityTarget)
				{
					subButton = Instantiate(Resources.Load("Prefabs/ItemInventUseButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = sceneContent.GetItemByID(inventory[i]).GetItemName();
					
					subButton.GetComponent<UseTargetManager>().AssignManager(inventory[i], this);

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
				}
			}
			
		}


        // Initial dialogue choices and assign button text for targeted NPC
        else if (type == 10)
		{
			for (int i = 0; i < sceneContent.GetNPC(primaryEntityTarget).GetDialogueCount(); ++i)
			{
				subButton = Instantiate(Resources.Load("Prefabs/DialogueButton")) as GameObject;
				subButton.transform.SetParent(subButtonPanel.transform);
				
				Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
				subButtonTextCompo.text = sceneContent.GetNPC(primaryEntityTarget).GetNextDialogue(i).GetButtonText();

				subButton.GetComponent<DialogueManager>().AssignObject(sceneContent.GetNPC(primaryEntityTarget).GetNextDialogue(i), this);

				if (subButton)
				{
					ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
				}

			}
		}
        // Selecting to see Clue Inventory or Item Inventory and assign button text 
        else if (type == 11)
		{
			for (int i = 0; i < subInventTypeButtons.Count; ++i)
			{
				subButton = Instantiate(Resources.Load("Prefabs/InventTypeButton")) as GameObject;
				subButton.transform.SetParent(subButtonPanel.transform);
				
				Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
				subButtonTextCompo.text = subInventTypeButtons[i];
				
				subButton.GetComponent<InventoryTypeManager>().AssignManager(i, this);

				if (subButton)
				{
					ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
				}
			}
		}
		// Displaying Clue Inventory In Investigation
		else if (type == 12)
		{
			if (clues.Count == 0)
			{
				LoadEmptyButton(EmptyClues);
			}
			else
			{

				for (int i = 0; i < clues.Count; ++i)
				{
					subButton = Instantiate(Resources.Load("Prefabs/ClueInventButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = clues[i].GetButtonText();

					subButton.GetComponent<InventoryClueManager>().AssignObject(i, this);

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
					
				}
			}
		}
		// Displaying Clues for Deduction
		else if (type == 13)
		{
			if (clues.Count == 0)
			{
				LoadEmptyButton(EmptyClues);
			}
			else
			{
				for (int i = 0; i < clues.Count; ++i)
				{
					subButton = Instantiate(Resources.Load("Prefabs/InventDeductionButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = clues[i].GetButtonText();
					
					subButton.GetComponent<DeductionSelectManager>().AssignManager(clues[i].GetClueID(), clues[i].GetSelectText());

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
					
				}
			}
		}
		// Displaying Items for Deduction
		else if (type == 14)
		{
			if (items.Count == 0)
			{
				LoadEmptyButton(EmptyItems);
			}
			else
			{

				for (int i = 0; i < items.Count; ++i)
				{
					subButton = Instantiate(Resources.Load("Prefabs/InventDeductionButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = items[i].GetItemName();
					
					subButton.GetComponent<DeductionSelectManager>().AssignManager(items[i].GetItemName(), items[i].GetInteractText());

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}
				}
			}
		}
		// Instantiating and Assigning Button text for NPC Suspects in Deduction phase
		else if (type == 15)
		{
			if (suspects.Count == 0)
			{
				LoadEmptyButton(EmptyNPC);
			}
			else
			{
				
				for (int i = 0; i < suspects.Count; ++i)
				{
					subButton = Instantiate(Resources.Load("Prefabs/DeducedSuspectButton")) as GameObject;
					subButton.transform.SetParent(subButtonPanel.transform);
					
					Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
					subButtonTextCompo.text = suspects[i].GetNameID();

					subButton.GetComponent<DeductSuspDisplayManager>().AssignManager(suspects[i].GetDescription());

					if (subButton)
					{
						ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
					}

				}
			}
		}
        // Instantiating and Assigning Button text for the final Accusation in Deduction phase
        else if (type == 16)
		{
			for (int i = 0; i < suspects.Count; ++i)
			{
				subButton = Instantiate(Resources.Load("Prefabs/AccusationButton")) as GameObject;
				subButton.transform.SetParent(subButtonPanel.transform);
				
				Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
				subButtonTextCompo.text = suspects[i].GetNameID();

				subButton.GetComponent<AccusationManager>().AssignManager(i, this);

				if (subButton)
				{
					ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
				}
				
			}

			TypeEventTextWithWipe("Who is the Murderer?");
		}


		// reenable subButtons after wipe once theyve been instantiated
		isSubButtonsActive = true;
	}

	// Overload for using a dialogueChoice instance instead of an int ID
	public void LoadSubButtons(DialogueChoice dialogue)
	{
		GameObject subButton = null;

		// prepare next wave of dialogue buttons
		PrepareScrollRect();

		for (int i = 0; i < dialogue.GetDialogueCount(); ++i)
		{
			subButton = Instantiate(Resources.Load("Prefabs/DialogueButton")) as GameObject;
			subButton.transform.SetParent(subButtonPanel.transform);
			
			Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
			subButtonTextCompo.text = dialogue.GetNextDialogue(i).GetButtonText();

			subButton.GetComponent<DialogueManager>().AssignObject(dialogue.GetNextDialogue(i), this);

			if (subButton)
			{
				ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
			}
		}

		// reenable subButtons after wipe once theyve been instantiated
		isSubButtonsActive = true;
	}

	public void LoadSubButtons(List<Deduction> deductions)
	{
		GameObject subButton = null;

		// prepare next wave of dialogue buttons
		PrepareScrollRect();

		for (int i = 0; i < deductions.Count; ++i)
		{
			subButton = Instantiate(Resources.Load("Prefabs/DeducedSuspectButton")) as GameObject;
			subButton.transform.SetParent(subButtonPanel.transform);
			
			Text subButtonTextCompo = subButton.GetComponent<Button>().GetComponentInChildren<Text>();
			subButtonTextCompo.text = deductions[i].GetID();

			subButton.GetComponent<DeductSuspDisplayManager>().AssignManager(deductions[i].GetDeductionText());

			if (subButton)
			{
				ResizePanel(subButton.GetComponent<LayoutElement>().minWidth);
			}
		}

		// reenable subButtons after wipe once theyve been instantiated
		isSubButtonsActive = true;
	}

	public void ToggleMuscleButton(bool toggle)
	{
		if (toggle)
		{
			// if to be toggled on, toggle it immediately if no typing
			// or flag it for toggle after completion
			if (eventTypewriter.IsTypewriterComplete())
			{
				muscleButton.gameObject.SetActive(true);
			}
			else
			{
				isStoredMuscleToggle = true;
			}
		}
		else
		{
			muscleButton.gameObject.SetActive(false);
		}
	}

	public void ToggleContinueButton(bool toggle)
	{
		continueChainButton.gameObject.SetActive(toggle);
	}

	// continuing text chain if there is still more chain count and thereby more files with text to insert into the event text box
	public void ContinueTextChain()
	{
		// if statements checking for the chains in different contexts
		if (isIntroChain)
		{
			TypeEventTextWithWipe(sceneContent.GetIntroChain(textChain));
			++textChain;
			
			if (textChain == sceneContent.GetIntroChainCount())
			{
				isIntroChain = false;
				textChain = 0;
				sceneContent.WipeIntroChainList();
			}
		}
		else if (isExamineChain)
		{
			eventTypewriter.TypeInTypewriter(sceneContent.GetItemByID(primaryEntityTarget).GetExamineChainText(textChain));
			++textChain;
			
			if (textChain == sceneContent.GetItemByID(primaryEntityTarget).GetExamineChainCount())
			{
				isExamineChain = false;
				textChain = 0;
			}
		}
		else if (isDialogueChain)
		{
			eventTypewriter.TypeInTypewriter(activeDialogueChainList[textChain]);
			++textChain;
			
			if (textChain == activeDialogueChainList.Count)
			{
				isDialogueChain = false;
				textChain = 0;
				activeDialogueChainList.Clear();
			}
		}
		else if (isConclusionChain)
		{
			eventTypewriter.TypeInTypewriter(conclusion.GetSuccessChainList()[textChain]);
			++textChain;

			if (textChain == conclusion.GetSuccessChainList().Count)
			{
				isConclusionChain = false;

				textChain = 0;
			}
		}
		else if (isEndingChain)
		{
			eventTypewriter.TypeInTypewriter(conclusion.GetEndingChainList()[textChain]);
			++textChain;

			if (textChain == conclusion.GetEndingChainList().Count)
			{
				isEndingChain = false;

				textChain = 0;
			}
		}

	}

	// Choosing a target in the scene after pressing interact
	public void ExecuteInteractTarget(int itemID)
	{
		primaryEntityTarget = itemID;
		
		LoadSubButtons(5);
	}

	// Choosing a primary target in the scene after pressing inventory
	// Then loading the Inventory Interact Actions (Examine, Use etc)
	public void ExecuteInventoryTarget(int itemID)
	{
		primaryEntityTarget = itemID;

		LoadSubButtons(6);
	}

	// Choosing an NPC to start talking to and begin Dialogue Intro
	public void ExecuteTalkTarget(int npcID)
	{
		primaryEntityTarget = npcID;
		TypeEventTextWithWipe(sceneContent.GetNPC(npcID).GetTalkIntro());

		//setting muscle options if NPC has any muscle options at this time
		
		if (sceneContent.GetNPC(npcID).HasMuscle())
		{
			for (int i = 0; i < allMuscleOptions.Count; ++i)
			{
				foreach (string keyword in sceneContent.GetNPC(npcID).GetMuscleKeywords())
				{
					if (allMuscleOptions[i].GetKeyword() == keyword)
					{
						activeMuscleOptions.Add (allMuscleOptions[i]);
					}
				}
			}
		}

		LoadSubButtons(10);
	}

	// choosing a Dislogue option while talking to an NPC
	public void ExecuteDialogueTarget (DialogueChoice dialogue)
	{
		TypeEventTextWithWipe(dialogue.GetEventText());

		// Unlock items attached to dialogue if any
		if (dialogue.HasItemUnlocks())
		{
			sceneContent.UnlockItems(dialogue);
		}

		// Unlock travel locations attached to dialogue if any
		if (dialogue.HasMoveUnlocks())
		{
			sceneContent.UnlockMoves(dialogue);
		}

		if (dialogue.HasClueUnlocks())
		{
			bool hasClue = false;

			for (int i = 0; i < clues.Count; ++i)
			{
				if (clues[i].GetClueID() == dialogue.GetClueList()[0])
				{
					hasClue = true;
				}
			}

			if (!hasClue)
			{
				clues.Add (new Clue(dialogue.GetClueList()[0], dialogue.GetClueList()[1], dialogue.GetClueList()[2]));
			}
		}

		if (dialogue.HasMuscle())
		{
			for (int i = 0; i < allMuscleOptions.Count; ++i)
			{
				foreach (string keyword in dialogue.GetMuscleKeywords())
				{
					if (allMuscleOptions[i].GetKeyword() == keyword)
					{
						activeMuscleOptions.Add (allMuscleOptions[i]);
					}
				}
			}
		}

		if (dialogue.HasChain())
		{
			for (int i = 0; i < dialogue.GetChainList().Count; ++i)
			{
				activeDialogueChainList.Add (dialogue.GetChainList()[i]);
			}

			isDialogueChain = true;
		}

		if (dialogue.GetDialogueCount() > 0)
		{
			LoadSubButtons(dialogue);
		}


	}

	//executing Use action between two items based on location context
	//be it a item in scene or item in inventory interacting with another scene or inventory item
	public void ExecuteUseTarget(int targetID)
	{
		secondaryEntityTarget = targetID;

		// Scene Item with Inventory Item
		if (secondaryEntityLocation == 0)
		{
			for (int i = 0; i < sceneContent.GetUseItemCount(); ++i)
			{
				Item currentItem = sceneContent.GetUseItem(i);
				if (currentItem.UseReqFilled())
				{
					string primaryItemName = sceneContent.GetItemByID(primaryEntityTarget).GetItemName();
					string secondaryItemName = sceneContent.GetItemByID(secondaryEntityTarget).GetItemName();
					
					if ( primaryItemName == currentItem.GetUseReq(0) && secondaryItemName == currentItem.GetUseReq(1) || 
					    primaryItemName == currentItem.GetUseReq(1) && secondaryItemName == currentItem.GetUseReq(0))
					{
						TypeEventTextWithWipe(currentItem.GetUseText());

						WipePanelButtons();

						sceneContent.UnlockUseItem(currentItem);

						// Remove items
						sceneContent.RemoveItemByID(primaryEntityTarget);
						sceneContent.RemoveItemByID(secondaryEntityTarget);

						for (int j = 0; j < inventory.Count; ++j)
						{
							if (inventory[j] == primaryEntityTarget || inventory[j] == secondaryEntityTarget)
							{
								inventory.RemoveAt(j);
							}
						}

						return;
					}
				}
			}

			TypeEventTextWithWipe("You cannot Use those together");
		}
		// Inventory Item with Another Inventory Item
		else if (secondaryEntityLocation == 1)
		{
			for (int i = 0; i < sceneContent.GetUseItemCount(); ++i)
			{
				Item currentItem = sceneContent.GetUseItem(i);
				if (currentItem.UseReqFilled())
				{
					string primaryItemName = sceneContent.GetItemByID(primaryEntityTarget).GetItemName();
					string secondaryItemName = sceneContent.GetItemByID(secondaryEntityTarget).GetItemName();
					
					if ( primaryItemName == currentItem.GetUseReq(0) && secondaryItemName == currentItem.GetUseReq(1) || 
					    primaryItemName == currentItem.GetUseReq(1) && secondaryItemName == currentItem.GetUseReq(0))
					{
						TypeEventTextWithWipe(currentItem.GetUseText());
						
						WipePanelButtons();

						currentItem.ItemPickedUp();
						sceneContent.UnlockUseItem(currentItem);

						inventory.Add (currentItem.GetItemID());

						// Remove items
						sceneContent.RemoveItemByID(primaryEntityTarget);
						sceneContent.RemoveItemByID(secondaryEntityTarget);

						for (int j = 0; j < inventory.Count; ++j)
						{
							if (inventory[j] == primaryEntityTarget)
							{
								inventory.RemoveAt(j);
							}
						}

						for (int k = 0; k < inventory.Count; ++k)
						{
							if (inventory[k] == secondaryEntityTarget)
							{
								inventory.RemoveAt(k);
							}
						}


						return;
					}
				}
			}

			TypeEventTextWithWipe("You cannot Use those together");
		}
	}

	// differentiating in a use action where the second item to select needs to be loaded from
	public void ExecuteUseLocation(int locationID)
	{
		secondaryEntityLocation = locationID;

		// Scene selected, load items in scene
		if (locationID == 0)
		{
			LoadSubButtons(8);
		}
		// Inventory selected. load items in inventory
		else if (locationID == 1)
		{
			LoadSubButtons(9);
		}
	}

	// execute Interact Actions in Scene
	// Examine, Pick Up
	public void ExecuteInteractAction(int actionID)
	{
		// Examine
		if (actionID == 0)
		{
			TypeEventTextWithWipe(sceneContent.GetItemByID(primaryEntityTarget).GetExamineText());

			if (sceneContent.GetItemByID(primaryEntityTarget).HasMuscle())
			{
				Debug.Log("muscle");

				for (int i = 0; i < allMuscleOptions.Count; ++i)
				{
					foreach (string keyword in sceneContent.GetItemByID(primaryEntityTarget).GetMuscleKeywords())
					{
						if (allMuscleOptions[i].GetKeyword() == keyword)
						{
							activeMuscleOptions.Add (allMuscleOptions[i]);
						}
					}
				}

				isItemMuscle = true;
			}

			// Unlock Move locations if Examining possesses Unlocks
			if (sceneContent.GetItemByID(primaryEntityTarget).HasExamineMoveUnlocks())
			{
				sceneContent.UnlockMoves(sceneContent.GetItemByID(primaryEntityTarget));
			}

            // Unlock Clues if Examining possesses Unlocks
            if (sceneContent.GetItemByID(primaryEntityTarget).HasExamineClueUnlocks())
			{
				bool hasClue = false;
				
				for (int i = 0; i < clues.Count; ++i)
				{
					if (clues[i].GetClueID() == sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[0])
					{
						hasClue = true;
					}
				}

				if (!hasClue)
				{
					clues.Add (new Clue(sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[0], sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[1],
								sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[2]));
				}
	
			}


			// if there is more than one text file to display more than one text block
			isExamineChain = sceneContent.GetItemByID(primaryEntityTarget).HasExamineChain();

			WipePanelButtons();
		}
		// pick up
		else if (actionID == 1)
		{
			TypeEventTextWithWipe(sceneContent.GetItemByID(primaryEntityTarget).GetPickupText());
			WipePanelButtons();

			if (sceneContent.GetItemByID(primaryEntityTarget).CanTake())
			{
				inventory.Add (primaryEntityTarget);
				sceneContent.GetItemByID(primaryEntityTarget).ItemPickedUp();
			}
		}
	}

	// Execute displaying items or clues
	public void ExecuteClueItemAction(int actionID)
	{
		// Clues
		if (actionID == 0)
		{
			LoadSubButtons(12);
		}
		// Items
		else if (actionID == 1)
		{
			LoadSubButtons(3);
		}
	}

	// Execute Inventory actions
	// Examine, Use (different from pick up with Interact actions in scene)
	public void ExecuteInventoryAction(int actionID)
	{
		// Examine
		if (actionID == 0)
		{
			TypeEventTextWithWipe(sceneContent.GetItemByID(primaryEntityTarget).GetExamineText());

			if (sceneContent.GetItemByID(primaryEntityTarget).HasMuscle())
			{
				for (int i = 0; i < allMuscleOptions.Count; ++i)
				{
					foreach (string keyword in sceneContent.GetItemByID(primaryEntityTarget).GetMuscleKeywords())
					{
						if (allMuscleOptions[i].GetKeyword() == keyword)
						{
							activeMuscleOptions.Add (allMuscleOptions[i]);
						}
					}
				}

				isItemMuscle = true;
			}
            // Unlock Move locations if Examining possesses Unlocks
            if (sceneContent.GetItemByID(primaryEntityTarget).HasExamineMoveUnlocks())
			{
				sceneContent.UnlockMoves(sceneContent.GetItemByID(primaryEntityTarget));
			}
            // Unlock Clues if Examining possesses Unlocks
            if (sceneContent.GetItemByID(primaryEntityTarget).HasExamineClueUnlocks())
			{
				bool hasClue = false;

				for (int i = 0; i < clues.Count; ++i)
				{
					if (clues[i].GetClueID() == sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[0])
					{
						hasClue = true;
					}
				}

				if (!hasClue)
				{
					clues.Add (new Clue(sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[0], sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[1],
						sceneContent.GetItemByID(primaryEntityTarget).GetClueList()[2]));
				}

			}

			isExamineChain = sceneContent.GetItemByID(primaryEntityTarget).HasExamineChain();
		
			WipePanelButtons();
		}
		// Use
		else if (actionID == 1)
		{
			LoadSubButtons(7);
		}
	}

	// display Clue in event text box
	public void ExecuteClueDisplayAction(int clueID)
	{
		TypeEventTextWithWipe(clues[clueID].GetSelectText());
	}

	// display deduction in event text box
	public void ExecuteDeductionDisplayAction(string deduction)
	{
		TypeEventTextWithWipe(deduction);
	}

	// display new scene intro in event text box when move action is used 
	public void ExecuteMoveTransition()
	{
		WipePanelButtons();
		TypeEventTextWithWipe(sceneContent.GetSceneIntro());
		isIntroChain = sceneContent.HasIntroChain();

	}

	// scroll through muscle options in muscle mode
	public void ExecuteMuscleScroll(bool next)
	{
		if (next)
		{
			if (currentMuscleOption == activeMuscleOptions.Count - 1)
			{
				currentMuscleOption = 0;
			}
			else
			{
				++currentMuscleOption;
			}
		}
		else
		{
			if (currentMuscleOption == 0)
			{
				currentMuscleOption = activeMuscleOptions.Count - 1;
			}
			else
			{
				--currentMuscleOption;
			}
		}

		muscleText.text = activeMuscleOptions[currentMuscleOption].GetOptionDisplay();
	}

	// executing an active muscle option when selected
	public void ExecuteMuscleAction(string currentAction)
	{
		eventTypewriter.HideText(true);

		if (activeMuscleOptions.Count > 0)
		{
			// Has the right action been picked
			if (activeMuscleOptions[currentMuscleOption].HasSucceeded(currentAction))
			{
				// Unlock Items if specified
				if (activeMuscleOptions[currentMuscleOption].HasItemUnlock())
				{
					sceneContent.UnlockItems(activeMuscleOptions[currentMuscleOption].GetItemUnlock());
				}
				// Unlock Moves if specified
				if (activeMuscleOptions[currentMuscleOption].HasMoveUnlock())
				{
					sceneContent.UnlockMoves(activeMuscleOptions[currentMuscleOption].GetMoveUnlock());
				}

				for (int i = 0; i < allMuscleOptions.Count; ++i)
				{
					if (allMuscleOptions[i].GetKeyword() == activeMuscleOptions[currentMuscleOption].GetKeyword())
					{
						allMuscleOptions.RemoveAt(i);
						
						break;
					}
					
				}

				isMuscleChain = activeMuscleOptions[currentMuscleOption].HasChain();

				// Is there a chain sequence? Populate active list with the nect chain and limit UI if so
				if (isMuscleChain)
				{
					// save keyword for beginning of chain
					if (muscleChain == 0)
					{
						chainKeyword = activeMuscleOptions[currentMuscleOption].GetKeyword();
					}

					for (int i = 0; i < allMuscleOptions.Count; ++i)
					{
						if (allMuscleOptions[i].GetKeyword() == chainKeyword + muscleChain)
						{
							// Type success text and clear list and index ready for repopulation with the next chain option
							TypeEventTextWithWipe(activeMuscleOptions[currentMuscleOption].GetSuccess());
							currentMuscleOption = 0;


							activeMuscleOptions.Add (allMuscleOptions[i]);
							muscleText.text = activeMuscleOptions[currentMuscleOption].GetOptionDisplay();
							++muscleChain;

							if (SceneManager.GetActiveScene().name.Contains("Deduction"))
							{
								SetMindOrMuscle(true);
							}
							
							break;
						}
						
					}
				}
				else
				{
					TypeEventTextWithWipe(activeMuscleOptions[currentMuscleOption].GetSuccess());
					isMuscleEnd = true;
					chainKeyword = "";
					currentMuscleOption = 0;

					if (isItemMuscle)
					{
						sceneContent.RemoveItemByID(primaryEntityTarget);

						for (int i = 0; i < inventory.Count; ++i)
						{
							if (inventory[i] == primaryEntityTarget)
							{
								inventory.RemoveAt(i);
							}
						}

						isItemMuscle = false;
					}

					if (SceneManager.GetActiveScene().name.Contains("Deduction"))
					{
						SetMindOrMuscle(true);

					}


				}
				
				

			}
			else
			{
				eventTypewriter.TypeInTypewriter("You have no Effect");
				currentMuscleOption = 0;
			}

			if (!SceneManager.GetActiveScene().name.Contains("Deduction"))
			{
				ToggleInvestigationModeUI(false);
			}
		}
	}

	// Display ending text in event text box
	public void ExecuteEnding()
	{
			TypeEventTextWithWipe(conclusion.GetEnding());

			isEndingChain = conclusion.HasEndingChain();

			endingButton.gameObject.SetActive(false);
	}

    // Display Accusation text in event text boxa long with if it was successful
    // and any appropriate muscle options such as fight scenes with main suspect
    public bool ExecuteAccusation(int suspID)
	{
		if (conclusion.GetMurderer() == suspects[suspID].GetNameID())
		{
			WipePanelButtons();
			TypeEventTextWithWipe(conclusion.GetSuccess());

			if (conclusion.HasSuccessChain())
			{
				Debug.Log("conclusion chain");
				isConclusionChain = true;
			}

			if (conclusion.HasMuscle())
			{
				for (int i = 0; i < allMuscleOptions.Count; ++i)
				{
					foreach (string keyword in conclusion.GetMuscleKeywords())
					{
						if (allMuscleOptions[i].GetKeyword() == keyword)
						{
							activeMuscleOptions.Add (allMuscleOptions[i]);
						}
					}
				}
			}

			return true;
		}
		else
		{
			TypeEventTextWithWipe("didnt done it");
			return false;
		}
	}

	public bool HasConclusionChainActive()
	{
		return isConclusionChain;
	}


}
