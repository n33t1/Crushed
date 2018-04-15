using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour {

	// Use this for initialization
	public float objectPoints;
	public Text text;
	public Image image;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject.name == "Player" && image.sprite == null) {
			text.text = "Press Q to pickup the object";
		}
	}

	void OnTriggerExit2D (Collider2D collider)
	{
		text.text = "";
	}
}
