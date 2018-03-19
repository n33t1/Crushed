using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat {

	[SerializeField]
	private BarScript bar;

	[SerializeField]
	private float maxHealth = 100f;

	[SerializeField]
	private float minHealth = 0f;

	[SerializeField]
	private float currentHealth;

	public float CurrentHealth {
		get {
			return currentHealth;
		}
		set {
			
			this.currentHealth = Mathf.Clamp (value, minHealth, maxHealth);
			bar.Value = currentHealth;
		}
	}

}
