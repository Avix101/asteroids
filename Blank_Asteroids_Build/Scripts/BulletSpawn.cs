using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Handles the spawning of bullets from the ship
/// </summary>

public class BulletSpawn : MonoBehaviour 
{
	//Public bullet spawning information	
	public int ticksBetweenBullets;
	public GameObject leftCannon;
	public GameObject rightCannon;
	public GameObject bulletPrefab;
	public List<GameObject> bulletList;

	//Private bullet spawning information
	private int ticksUntilNextBullet;
	private bool cannon;

	// Use this for initialization
	void Start () 
	{
		//Create a new bullet list and set the time between firing bullets as well as the current cooldown value (0);
		bulletList = new List<GameObject>();
		ticksBetweenBullets = 30;
		ticksUntilNextBullet = 0;

		//The cannon variable is a simple bool that determines which of the ship's cannons will be used
		cannon = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the bullet spawning cooldown is not yet 0
		if(ticksUntilNextBullet > 0)
		{
			//Decrement the counter and return early
			ticksUntilNextBullet--;
			return;
		}

		//If the player is pressing the space bar and the ship is not currently teleporting
		if (Input.GetKey (KeyCode.Space) && !gameObject.GetComponent<ShipAbilities>().IsTeleporting) 
		{
			//Create a new bullet
			GameObject newBullet;

			//Determine which cannon the bullet should be fired from
			if(cannon)
			{
				//Instatiate the bullet using the bullet prefab, the proper cannon's location, and the current rotation of the ship
				newBullet = (GameObject) Instantiate(bulletPrefab, leftCannon.transform.position, gameObject.GetComponent<ShipMovement>().TotalRotation);
			}
			else
			{
				newBullet = (GameObject) Instantiate(bulletPrefab, rightCannon.transform.position, gameObject.GetComponent<ShipMovement>().TotalRotation);
			}

			//Set the bullet's direction and starting velocity to the ship's current direction and velocity respectively
			newBullet.GetComponent<BulletMovement>().bulletDirection = gameObject.GetComponent<ShipMovement>().ShipDirection;
			newBullet.GetComponent<BulletMovement>().startingVelocity = gameObject.GetComponent<ShipMovement>().ShipVelocity;

			//If the player is currently moving backwards
			if(Input.GetKey(KeyCode.S))
			{
				//The speed augment (adjustment for the moving ship) should be set to 0
				newBullet.GetComponent<BulletMovement>().speedAugment = 0;
			}
			else
			{
				//Otherwise we can factor in the speed augment
				newBullet.GetComponent<BulletMovement>().speedAugment = 1;
			}

			//Add the new bullet to the master list of bullets in the scene
			bulletList.Add(newBullet);

			//If the quick-fire ability is currently in use
			if(GetComponent<ShipAbilities>().BulletAugment)
			{
				//Reduce the cooldown between bullets firing and alert the Ship Abilities class that a bullet was fired
				ticksUntilNextBullet += ticksBetweenBullets / 3;
				GetComponent<ShipAbilities>().QuickfireUse();
			}
			else
			{
				//If the quick-fire ability is not active, apply the normal cooldown
				ticksUntilNextBullet += ticksBetweenBullets;
			}

			//Switch the cannon so that the next bullet uses the other cannon
			cannon = !cannon;
		}
	}
}
