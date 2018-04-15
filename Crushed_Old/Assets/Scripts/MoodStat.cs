using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MoodStat{

	public MoodScript moodBar;

	[SerializeField]
	private float maxFriendly = 100f;

	[SerializeField]
	private float minFriendly = 0f;

	[SerializeField]
	private float maxHappy = 100f;

	[SerializeField]
	private float minHappy = 0f;

	[SerializeField]
	private float maxRomantic = 100f;

	[SerializeField]
	private float minRomantic = 0f;

	[SerializeField]
	private float currentFriendly;

	[SerializeField]
	private float currentHappy;

	[SerializeField]
	private float currentRomantic;

	public float CurrentHappy {
		get {
			return currentHappy;
		}
		set {
			
			this.currentHappy = Mathf.Clamp (value, minHappy, maxHappy);
			moodBar.ValueHappy = currentHappy;
		}
	}

	public float CurrentFriendly {
		get {
			return currentFriendly;
		}
		set {
			
			this.currentFriendly = Mathf.Clamp (value, minFriendly, maxFriendly);
			moodBar.ValueFriendly = currentFriendly;
		}
	}

	public float CurrentRomantic {
		get {
			return currentRomantic;
		}
		set {
			
			this.currentRomantic = Mathf.Clamp (value, minRomantic, maxRomantic);
			moodBar.ValueRomantic = currentRomantic;
		}
	}
}
