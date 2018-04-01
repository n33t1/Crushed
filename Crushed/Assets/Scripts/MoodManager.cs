using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodManager : MonoBehaviour {

	// Use this for initialization

	private GameObject previousObject;
	private GameObject receivedObject;
	private GameObject previousObjectEnemy;
	private GameObject receivedObjectEnemy;
	public LevelManager levelManager;

	public MoodStat Mood;
	public int combo = 0;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((Mood.CurrentHappy == 0f && Mood.CurrentRomantic == 0f) || Mood.CurrentFriendly == 100f) {
			levelManager.LoadLevel("Lose Screen");
		}
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (Mood.CurrentHappy == 100f && Mood.CurrentRomantic == 100f && Input.GetKeyDown (KeyCode.P)) {
			levelManager.LoadLevel("Win Screen");
		}

		if (collider.gameObject.name == "Player" && Input.GetKeyDown (KeyCode.E)) {
			if (collider.gameObject.GetComponent<PlayerObjectManager> ().playerObject != null) {
				receivedObject = collider.gameObject.GetComponent<PlayerObjectManager> ().playerObject;
				IncreasePoints();
				collider.gameObject.GetComponent<PlayerObjectManager> ().playerObject = null;
			}
		}

	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.name == "Enemy") {
			if (collider.gameObject.GetComponent<EnemyAI> ().currentObject != null) {
				receivedObjectEnemy = collider.gameObject.GetComponent<EnemyAI> ().currentObject;
				collider.gameObject.GetComponent<EnemyAI> ().prevHappy = Mood.CurrentHappy;
				collider.gameObject.GetComponent<EnemyAI> ().prevRomance = Mood.CurrentRomantic;
				DecreasePoints ();
			}
		}
	}

	private void IncreasePoints ()
	{
		float points = receivedObject.GetComponent<ObjectManager> ().objectPoints;

		if (previousObject && receivedObject == previousObject) {
			Mood.CurrentFriendly += 20.0f;
		}

		switch (receivedObject.name) {
		case "RedRose":
			{
				print (receivedObject.name);
				Mood.CurrentRomantic += points;
				combo = 0;
				break;
			}
		case "BlueRose":
			{
				print (receivedObject.name);
				Mood.CurrentHappy += points;
				combo = 0;
				break;
			}
		case "BlackRose":
			{
				print (receivedObject.name);
				Mood.CurrentRomantic -= 4f * points;
				Mood.CurrentHappy -= 4f * points;
				combo = 0;
				break;
			}
		case "Strawberry":
			{
				print (receivedObject.name);
				Mood.CurrentRomantic += points;
				Mood.CurrentHappy += points;
				combo = 0;
				break;
			}
		case "Chocolate":
			{
				print (receivedObject.name);
				if (previousObject && previousObject.name == "RedRose") {
					Mood.CurrentRomantic += 1.5f * points;
					combo = 1;
				} else {
					Mood.CurrentRomantic += points;
					combo = 0;
				}
				break;
			}
		case "Dress":
			{
				print (receivedObject.name);
				if (previousObject && previousObject.name == "BlueRose") {
					Mood.CurrentHappy += 1.5f * points;
					combo = 1;
				} else {
					Mood.CurrentHappy += points;
					combo = 0;
				}
				break;
			}
		case "Ring":
			{
				print (receivedObject.name);
				if (previousObject && previousObject.name == "RedRose") {
					Mood.CurrentRomantic += 1.5f * points;
					combo = 1;
				} else if (previousObject && previousObject.name == "Chocolate" && combo == 0) {
					Mood.CurrentRomantic += 1.5f * points;
					Mood.CurrentHappy += points;
					combo = 1;
				} 
				else if (previousObject && previousObject.name == "Chocolate" && combo == 1) {
					Mood.CurrentRomantic += 3f * points;
					Mood.CurrentHappy += 2f * points;
					combo = 2;
				} else {
					Mood.CurrentHappy -= 2f * points;
					combo = 0;
				}
					break;
				}
			}

		previousObject = receivedObject;
		receivedObject = null;
	}


	private void DecreasePoints ()
	{
		float points = receivedObjectEnemy.GetComponent<ObjectManager> ().objectPoints;

		switch (receivedObjectEnemy.name) {
		case "RedRose":
			{
				print (receivedObjectEnemy.name);
				Mood.CurrentRomantic -= points;
				combo = 0;
				break;
			}
		case "BlueRose":
			{
				print (receivedObjectEnemy.name);
				Mood.CurrentHappy -= points;
				combo = 0;
				break;
			}
		case "Strawberry":
			{
				print (receivedObjectEnemy.name);
				Mood.CurrentRomantic -= points;
				Mood.CurrentHappy -= points;
				combo = 0;
				break;
			}
		case "Chocolate":
			{
				print (receivedObjectEnemy.name);
				if (previousObjectEnemy && previousObjectEnemy.name == "RedRose") {
					Mood.CurrentRomantic -= 1.5f * points;
					combo = 1;
				} else {
					Mood.CurrentRomantic -= points;
					combo = 0;
				}
				break;
			}
		case "Dress":
			{
				print (receivedObjectEnemy.name);
				if (previousObjectEnemy && previousObjectEnemy.name == "BlueRose") {
					Mood.CurrentHappy -= 1.5f * points;
					combo = 1;
				} else {
					Mood.CurrentHappy -= points;
					combo = 0;
				}
				break;
			}
		case "Ring":
			{
				print (receivedObjectEnemy.name);
				if (previousObjectEnemy && previousObjectEnemy.name == "RedRose") {
					Mood.CurrentRomantic -= 1.5f * points;
					combo = 1;
				} else if (previousObjectEnemy && previousObjectEnemy.name == "Chocolate" && combo == 0) {
					Mood.CurrentRomantic -= 1.5f * points;
					Mood.CurrentHappy -= points;
					combo = 1;
				} 
				else if (previousObjectEnemy && previousObjectEnemy.name == "Chocolate" && combo == 1) {
					Mood.CurrentRomantic -= 3f * points;
					Mood.CurrentHappy -= 2f * points;
					combo = 2;
				}
				break;
			}
		}

		previousObjectEnemy = receivedObjectEnemy;
		receivedObjectEnemy = null;
	
  }
}