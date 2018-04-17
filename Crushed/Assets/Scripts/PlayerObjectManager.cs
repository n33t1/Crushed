using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObjectManager : MonoBehaviour {

	// Use this for initialization
	public GameObject playerObject;
	public Image objectImage;
	public Text text;

	void OnTriggerStay2D (Collider2D collider)
	{
		if (Input.GetKey (KeyCode.Q) && collider.gameObject.tag == "Objects") {
			if (objectImage.sprite == null) {
				objectImage.sprite = collider.gameObject.GetComponent<SpriteRenderer> ().sprite;
				Color c = objectImage.color;
        		c.a = 255;
        		objectImage.color = c;
				playerObject = collider.gameObject;
				text.text = "";
			} else {
				text.text = "You are already carrying an object";
			}
		}
	}

}
