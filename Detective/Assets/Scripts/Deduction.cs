using UnityEngine;
using System.Collections;

public class Deduction {

	string identifier;
	string firstObject;
	string secondObject;
	string deductionText;

	public Deduction (string id, string first, string second, string deduction)
	{
		identifier = id;
		firstObject = first;
		secondObject = second;
		deductionText = deduction;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// return deduction if two paired item/clues from user match or blank
	public string CheckDeduction(string first, string second)
	{
		if ((firstObject == first && secondObject == second) ||
		    (firstObject == second && secondObject == first))
		{
			return deductionText;
		}
		else
		{
			return "";
		}
	}

	public string GetID()
	{
		return identifier;
	}

	public string GetDeductionText()
	{
		return deductionText;
	}
}
