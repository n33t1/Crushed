using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainAnimator : MonoBehaviour {

	public Sprite[] fountain;
	private int spriteIndex = 0;
	public float waitTime;

	// Use this for initialization
	void Start () {
		InvokeRepeating("ChangeSprite", 0.00001f, waitTime);
	}

	void ChangeSprite ()
	{
		this.GetComponent<SpriteRenderer> ().sprite = fountain [spriteIndex++];

		if (spriteIndex >= fountain.Length) {
			spriteIndex = 0;
		}
	}
}
