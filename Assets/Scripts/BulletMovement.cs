using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Handles the movement for an individual bullet
/// </summary>

public class BulletMovement : MonoBehaviour 
{
	//Public bullet information
	public Vector3 bulletPosition;
	public Vector3 bulletDirection;
	public Vector3 bulletVelocity;
	public Vector3 startingVelocity;
	public float speedAugment;

	//Private bullet information (basically info that doesn't need to be public)
	private float speed;
	private float dieDistance;

	// Use this for initialization
	void Start () 
	{
		//Set the bullet's initial position to its actual position in the game
		bulletPosition = transform.position;

		//Set the bullet's initial speed to zero
		bulletVelocity = Vector3.zero;

		//The speed augment variable determines how fast the bullet should travel based on the speed of the ship at the time of firing
		//When a bullet is created, the starting velocity is set to be the velocity of the ship, which is multiplied by 60 to be
		//relevant before delta time is considered
		speedAugment = speedAugment * startingVelocity.magnitude * 60;

		//The speed of a bullet (not considering ship speed) and the distance from the origin at which it will be destroyed
		speed = 4f;
		dieDistance = 10;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Set the bullet's velocity based on its own speed and the speed of the ship at the time of firing
		bulletVelocity = (speed + Mathf.Max(speedAugment,0)) * bulletDirection * Time.deltaTime;

		//Update the position
		bulletPosition += bulletVelocity;

		//Apply the position transformation
		transform.position = bulletPosition;

		//If the bullet has travelled too far from the center of the screen (offscreen in this case)
		if(transform.position.magnitude > dieDistance)
		{
			//Remove the bullet from the master list and destroy it
			GameObject.Find("Ship").GetComponent<BulletSpawn>().bulletList.Remove(gameObject);
			Destroy(gameObject);
		}
	}
}
