using UnityEngine;
using System.Collections;

// Skeleton for what defines a Move Location
// from its unique ID, name and whether it is available and active to be travelled to
public class MoveLocation {

	string locationName;
	int locationID;

	bool canTravel;


	public MoveLocation (string na, int id, bool travel)
	{
		locationName = na;
		locationID = id;
		canTravel = travel;
	}

	public void TravelUnlocked()
	{
		canTravel = true;
	}

	public string GetMoveName()
	{
		return locationName;
	}

	public int GetLocationID()
	{
		return locationID;
	}

	public bool CanTravelToScene()
	{
		return canTravel;
	}

}
