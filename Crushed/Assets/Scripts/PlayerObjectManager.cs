﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObjectManager : MonoBehaviour {

	// Use this for initialization
	public GameObject playerObject;
	public Image objectImage;
	public Text text;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (Input.GetKeyDown (KeyCode.Q) && collider.gameObject.tag == "Objects") {
			if (objectImage.sprite == null) {
				objectImage.sprite = collider.gameObject.GetComponent<SpriteRenderer> ().sprite;
				playerObject = collider.gameObject;
				text.text = "";
			} else {
				text.text = "You are already carrying an object";
			}
		}
	}

}
