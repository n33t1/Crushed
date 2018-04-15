using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat {

	[SerializeField]
	private BarScript healthBar;

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
			healthBar.Value = currentHealth;
		}
	}

}
