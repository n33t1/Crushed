using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodScript : MonoBehaviour {

	[SerializeField]
	private float fillAmountFriendly;

	[SerializeField]
	private float fillAmountHappy;

	[SerializeField]
	private float fillAmountRomantic;

	[SerializeField]
	private float lerpSpeed = 2f;

	[SerializeField]
	private Image friendly;

	[SerializeField]
	private Image happy;

	[SerializeField]
	private Image romantic;

	public float ValueFriendly {
		set {
			fillAmountFriendly = value / 100f;
		}

	}

	public float ValueHappy {
		set {
			fillAmountHappy = value / 100f;
		}

	}

	public float ValueRomantic {
		set {
			fillAmountRomantic = value / 100f;
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		HandleBar();

	}

	private void HandleBar ()
	{
		if (fillAmountFriendly != friendly.fillAmount) {
			friendly.fillAmount = Mathf.Lerp(friendly.fillAmount, fillAmountFriendly, Time.deltaTime * lerpSpeed);
		}

		if (fillAmountHappy != happy.fillAmount) {
			happy.fillAmount = Mathf.Lerp(happy.fillAmount, fillAmountHappy, Time.deltaTime * lerpSpeed);
		}

		if (fillAmountRomantic != romantic.fillAmount) {
			romantic.fillAmount = Mathf.Lerp(romantic.fillAmount, fillAmountRomantic, Time.deltaTime * lerpSpeed);
		}

	}
}
