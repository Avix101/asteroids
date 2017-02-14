using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Detects Collisions between two game objects using the Circle Collision method or the Point-To-Shape
/// method depending on the situation (asteroid versus ship or bullet versus asteroid)
/// </summary>

public class CollisionDetection : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Purpose: Checks to see if two game objects (Ship and Asteroid) are colliding using the Circle Collision method
	/// </summary>
	/// <returns><c>true</c>If the two objects are colliding<c>false</c>If the two objects aren't colliding</returns>
	/// <param name="ship">The Ship</param>
	/// <param name="shipRadius">The Ship's radius</param>
	/// <param name="asteroid">The Asteroid</param>
	/// <param name="asteroidRadius">The Asteroid's radius</param>
	public bool CircleCollision(GameObject ship, float shipRadius, GameObject asteroid, float asteroidRadius)
	{
		//Grab the ship's position and the asteroid's position
		Vector3 shipCenter = ship.transform.position;
		Vector3 asteroidCenter = asteroid.transform.position;

		//Ensure that both position vectors have their z component set to 0
		shipCenter.z = 0;
		asteroidCenter.z = 0;

		//Calculate the vector between the two objects' centers, compute the squared distance between them, and compute the sum
		//of the radii squared
		Vector3 centerVector = shipCenter - asteroidCenter;
		float distanceSqaured = Vector3.Dot (centerVector, centerVector);
		float radii = Mathf.Pow (shipRadius + asteroidRadius, 2);

		//If the sum of the radii squared is greater than the distance squared
		if(radii > distanceSqaured)
		{
			//The ship and asteroid are colliding
			return true;
		}

		//The ship and the asteroid are not colliding
		return false;
	}

	/// <summary>
	/// Purpose: Checks to see if two game objects (Asteroid and Bullet) are colliding using the Point-To-Shape method
	/// </summary>
	/// <returns><c>true</c>If the asteroid is colliding with the bullet<c>false</c>If the asteroid is not colliding with the bullet</returns>
	/// <param name="asteroid">The Asteroid</param>
	/// <param name="asteroidRadius">The Asteroid's radius</param>
	/// <param name="bullet">The Bullet</param>
	public bool BulletCollision(GameObject asteroid, float asteroidRadius, GameObject bullet)
	{
		//Grab the position of the asteroid and the position of the bullet
		Vector3 asteroidCenter = asteroid.transform.position;
		Vector3 bulletCenter = bullet.transform.position;

		//Set the z component of the asteroid's position vector to 0
		asteroidCenter.z = 0;

		//Calculate the vector between the bullet and the asteroid, compute the sqaured distance between them, and compute the sum 
		//of the radii squared
		Vector3 centerVector = bulletCenter - asteroidCenter;
		float distanceSqaured = Vector3.Dot (centerVector, centerVector);
		float radii = Mathf.Pow (asteroidRadius, 2);

		//If the sum of the radii squared is greater than the distance squared
		if(radii > distanceSqaured)
		{
			//The asteroid and bullet are colliding
			return true;
		}

		//The asteroid and bullet aren't colliding
		return false;
	}
}
