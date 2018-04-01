using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Girl : MonoBehaviour {

	// Use this for initialization
	public Image image;
	public Text text;

	void OnTriggerEnter2D (Collider2D collider)
	{
		MoodManager mood = gameObject.GetComponent<MoodManager> ();

		if (mood.Mood.CurrentHappy == 100f && mood.Mood.CurrentRomantic == 100f) {
			text.text = "Press P to Propose";
		}

		if (collider.gameObject.name == "Player" && image.sprite != null) {
			text.text = "Press E to give the object to the girl";
		}
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (collider.gameObject.name == "Player" && Input.GetKeyDown (KeyCode.E)) {
				image.sprite = null;
				text.text = "";
		}
	}

	void OnTriggerExit2D (Collider2D collider)
	{
		text.text = "";
	}
}
