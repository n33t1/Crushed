using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	// Use this for initialization
	public GameObject[] objects;
	private HashSet<GameObject> unexploredObjects;
	private HashSet<GameObject> safeObjects;
	public MoodManager moodManager;

	[SerializeField]
	private GameObject currentObject;
	[SerializeField]
	private GameObject previousObject;

	public GameObject Girl;

	private string status = "safe";
	bool moving = false;

	//0 for friendship edges, 1 for romantic edges
	Dictionary<GameObject, GameObject>[] Edges = new Dictionary<GameObject, GameObject>[2];

	void Start () {
		LoadObjects ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateStatus ();
		if (!moving) {
			GoalBehaviour ();
		}
	}

	private void LoadObjects ()
	{
		unexploredObjects = new HashSet<GameObject>();
		foreach(GameObject go in objects){
			unexploredObjects.Add(go);
		}
	}

	private void UpdateStatus ()
	{
		if (moodManager.Mood.CurrentFriendly <= 50f && moodManager.Mood.CurrentRomantic <= 50f) {
			status = "safe";
		} else if (moodManager.Mood.CurrentFriendly <= 50f || moodManager.Mood.CurrentRomantic <= 50f) {
			status = "danger";
		} else {
			status = "critical";
		}
	}

	void GoalBehaviour ()
	{
		moving = true;

		if (status == "safe") {
			SafeBehaviour ();
		} else if (status == "danger") {
			DangerBehaviour ();
		} else {
			CriticalBehaviour ();
		}
	}

	void SafeBehaviour ()
	{
		if (unexploredObjects.Count > 0) {
			int index = Random.Range (0, unexploredObjects.Count);
			gameObject.GetComponent<NavMeshAgent2D>().destination = objects[index].transform.position;
		}
	}

	void DangerBehaviour ()
	{
	}

	void CriticalBehaviour ()
	{
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (collider.gameObject.tag == "Objects" && currentObject == null &&
			gameObject.GetComponent<NavMeshAgent2D>().velocity.magnitude == 0f) 
		{
			currentObject = collider.gameObject;
			gameObject.GetComponent<NavMeshAgent2D> ().destination = Girl.transform.position;
		} 
		else if (collider.gameObject.tag == "Girl" && currentObject != null) 
		{
			previousObject = currentObject;
			currentObject = null;
			moving = false;
		}
	}
}
