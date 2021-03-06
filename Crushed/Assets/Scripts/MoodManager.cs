﻿using System.Collections;
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
	public int playerCombo = 0;
	
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

		if (collider.gameObject.name == "Player" && Input.GetKey (KeyCode.E)) {
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
				Mood.CurrentRomantic += points;
				playerCombo = 0;
				break;
			}
		case "BlueRose":
			{
				Mood.CurrentHappy += points;
				playerCombo = 0;
				break;
			}
		case "BlackRose":
			{
				Mood.CurrentRomantic -= 4f * points;
				Mood.CurrentHappy -= 4f * points;
				playerCombo = 0;
				break;
			}
		case "Strawberry":
			{
				Mood.CurrentRomantic += points;
				Mood.CurrentHappy += points;
				playerCombo = 0;
				break;
			}
		case "Chocolate":
			{
				if (previousObject && previousObject.name == "RedRose") {
					Mood.CurrentRomantic += 1.5f * points;
					playerCombo = 1;
				} else {
					Mood.CurrentRomantic += points;
					playerCombo = 0;
				}
				break;
			}
		case "Dress":
			{
				if (previousObject && previousObject.name == "BlueRose") {
					Mood.CurrentHappy += 1.5f * points;
					playerCombo = 1;
				} else {
					Mood.CurrentHappy += points;
					playerCombo = 0;
				}
				break;
			}
		case "Ring":
			{
				if (previousObject && previousObject.name == "RedRose") {
					Mood.CurrentRomantic += 1.5f * points;
					playerCombo = 1;
				} else if (previousObject && previousObject.name == "Chocolate" && playerCombo == 0) {
					Mood.CurrentRomantic += 1.5f * points;
					Mood.CurrentHappy += points;
					playerCombo = 1;
				} 
				else if (previousObject && previousObject.name == "Chocolate" && playerCombo == 1) {
					Mood.CurrentRomantic += 3f * points;
					Mood.CurrentHappy += 2f * points;
					playerCombo = 2;
				} else {
					Mood.CurrentHappy -= 2f * points;
					playerCombo = 0;
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
				Mood.CurrentRomantic -= points;
				combo = 0;
				break;
			}
		case "BlueRose":
			{
				Mood.CurrentHappy -= points;
				combo = 0;
				break;
			}
		case "Strawberry":
			{
				Mood.CurrentRomantic -= points;
				Mood.CurrentHappy -= points;
				combo = 0;
				break;
			}
		case "Chocolate":
			{
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