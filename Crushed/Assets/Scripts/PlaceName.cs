using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlaceName : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
			this.gameObject.GetComponent<Text>().text = "Place : " + SceneManager.GetActiveScene().name;
	}
}
