using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Handle the movement and information of each individual asteroid
/// </summary>

public class AsteroidMovement : MonoBehaviour 
{
	//Public asteroid information that can be changed by another script (e.g. SceneManager)
	public Vector3 asteroidPosition;
	public Vector3 asteroidDirection;
	public Vector3 asteroidVelocity;
	public float radius;
	public float speed;
	public bool secondLevel;
	public bool collectable;

	//Private die distance and ship reference
	private float dieDistance;
	private GameObject ship;
	
	// Use this for initialization
	void Start () 
	{
		//Set the initial location of the asteroid based on its spawn location
		asteroidPosition = transform.position;
		asteroidVelocity = Vector3.zero;

		//If the asteroid is radioactive (collectable) and a second level asteroid
		if(secondLevel && collectable)
		{
			//Grab a reference of the main ship
			ship = GameObject.Find ("Ship");
		}

		//If speed has not been set by AsteroidSpawn (first level asteroids)
		if(speed == 0)
		{
			//Give the asteroid a random speed between 1 and 3
			speed = Random.Range(1,3);
		}

		//Die distance will destroy the asteroid object when it goes too far from the origin (for this game, that roughly means offscreen)
		dieDistance = 11;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the asteroid is second level and radioactive, and the harvest ability is usable
		if(secondLevel && collectable && Input.GetKey(KeyCode.Alpha3) && HarvestEnabled())
		{
			//Accelerate the asteroid towards the ship (magnet-like ability)
			GoToShip();
		}

		//Adjust the asteroid's velocity based on it's set speed, direction, and delta time
		asteroidVelocity = speed * asteroidDirection * Time.deltaTime;

		//Update the position (no acceleration here, unless magnet is active)
		asteroidPosition += asteroidVelocity;

		//Apply the new position to the transformation
		transform.position = asteroidPosition;

		//If the asteroid flies too far from the center of the screen (offscreen in this case)
		if(transform.position.magnitude > dieDistance)
		{
			//Remove the asteroid from the master list in Scene Manager and destroy it
			GameObject.Find("SceneManager").GetComponent<SceneManager>().asteroids.Remove(gameObject);
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Purpose: Used to allow special asteroids to accelerate towards the ship if the harvest ability is used
	/// Note: This is only called within this class; in order to account for acceleration and a new direction
	/// </summary>
	void GoToShip()
	{
		//Set a new direction (pointing towards the ship) and normalize the new direction vector
		asteroidDirection = ship.transform.position - asteroidPosition;
		asteroidDirection.Normalize ();

		//Augment the speed based on the acceleration rate (currently constant);
		speed += 0.04f;
	}

	/// <summary>
	/// Purpose: Checks to see if the harvest ability is usable (The Ship Abilities class will handle all other critical actions)
	/// </summary>
	/// <returns><c>true</c>, if harvest ability is usable, <c>false</c> if harvest ability is not usable.</returns>
	bool HarvestEnabled()
	{
		//Check to see if there is enough radioactive energy to activate the harvest ability
		return ship.GetComponent<ShipAbilities> ().RadioactiveEnergy > 0;
	}
}
