using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Girl : MonoBehaviour {

	// Use this for initialization
	public Image playerImage;
	public Image enemyImage;
	public Text text;

	void OnTriggerEnter2D (Collider2D collider)
	{
		MoodManager mood = gameObject.GetComponent<MoodManager> ();

		if (mood.Mood.CurrentHappy == 100f && mood.Mood.CurrentRomantic == 100f) {
			text.text = "Press P to Propose";
		}

		if (collider.gameObject.name == "Player" && playerImage.sprite != null) {
			text.text = "Press E to give the object to the girl";
		}


	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (collider.gameObject.name == "Player" && Input.GetKey (KeyCode.E)) {
				playerImage.sprite = null;
				Color c = playerImage.color;
        		c.a = 0;
        		playerImage.color = c;
				text.text = "";
		}

		if (collider.gameObject.name == "Enemy" && enemyImage != null) {
				enemyImage.sprite = null;
				Color c = enemyImage.color;
        		c.a = 0;
        		enemyImage.color = c;
		}
	}

	void OnTriggerExit2D (Collider2D collider)
	{
		text.text = "";
	}
}
