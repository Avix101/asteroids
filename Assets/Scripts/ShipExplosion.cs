using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Explodes the ship upon death
/// </summary>

public class ShipExplosion : MonoBehaviour 
{
	//Public sprites shown on death
	public ParticleSystem explosionParticles;
	public GameObject chassis;
	public GameObject rightEngine;
	public GameObject leftEngine;

	//Private engine speed tracks
	private bool rotateEngines;
	private float leftEngineSpeed;
	private float rightEngineSpeed;

	// Use this for initialization
	void Start () 
	{
		//Disable the explosion particles and don't rotate the engines
		explosionParticles.enableEmission = false;
		rotateEngines = false;

		//Assign each engine a random rotate speed
		leftEngineSpeed = Random.Range (0.1f, 1f);
		rightEngineSpeed = Random.Range (0.1f, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the engines should rotate, rotate them
		if(rotateEngines)
		{
			RotateEngines();
		}
	}

	/// <summary>
	/// Purpose: Animate the explosion of the ship
	/// </summary>
	public void Explode()
	{
		//Emit explosion particles on top of the ship
		explosionParticles.Emit (300);

		//Turn off the ship's sprite renderer (it becomes invisible), and turn off the bullet spawn and ability components
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BulletSpawn> ().enabled = false;
		GetComponent<ShipAbilities> ().enabled = false;

		//Make the separate ship sprites visible
		chassis.GetComponent<SpriteRenderer> ().enabled = true;
		leftEngine.GetComponent<SpriteRenderer> ().enabled = true;
		rightEngine.GetComponent<SpriteRenderer> ().enabled = true;

		//Detach the engines from the parent object
		leftEngine.transform.parent = null;
		rightEngine.transform.parent = null;

		//Now we rotate the engines
		rotateEngines = true;
	}

	/// <summary>
	/// Purpose: Rotates the engines
	/// </summary>
	void RotateEngines()
	{
		//Rotate each engine the predefined random amount
		leftEngine.transform.Rotate (new Vector3 (0, 0, leftEngineSpeed));
		rightEngine.transform.Rotate (new Vector3 (0, 0, rightEngineSpeed));
	}
}
