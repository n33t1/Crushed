using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	// Use this for initialization
	public GameObject[] objects;
	private List<GameObject> unexploredObjects;
	private List<GameObject> safeObjects;
	private List<GameObject> romanceObjects;
	private List<GameObject> happyObjects;

	public MoodManager moodManager;

	public GameObject currentObject;
	[SerializeField]
	private GameObject previousObject;

	// Numerical values for mood
	private float totalMood;
	public float prevHappy;
	public float prevRomance;

	public GameObject Girl;

	private string status = "safe";
	bool moving = false;

	//0 for friendship edges, 1 for romantic edges
	Dictionary<GameObject, List<GameObject>>[] Edges = new Dictionary<GameObject, List<GameObject>>[2];

	Dictionary<Dictionary<GameObject, GameObject>, int> vaLUES;

	void Start () {
		LoadObjects ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		totalMood = moodManager.Mood.CurrentHappy + moodManager.Mood.CurrentRomantic; 
		UpdateStatus ();
		if (!moving) {
			GoalBehaviour ();
		}
	}

	private void LoadObjects ()
	{
		unexploredObjects = new List<GameObject>();
		foreach(GameObject go in objects){
			unexploredObjects.Add(go);
		}
	}

	private void UpdateStatus ()
	{
		if (Mathf.Pow(moodManager.Mood.CurrentHappy, 2) +  Mathf.Pow(moodManager.Mood.CurrentRomantic, 2) <= 5000f) {
			status = "safe";
		} else if (Mathf.Pow(moodManager.Mood.CurrentHappy, 2) +  Mathf.Pow(moodManager.Mood.CurrentRomantic, 2) <= 12500f) {
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
			gameObject.GetComponent<NavMeshAgent2D>().destination = unexploredObjects[index].transform.position;
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
			if (prevHappy > moodManager.Mood.CurrentHappy) {
				if(!happyObjects.Contains(currentObject))
					happyObjects.Add (currentObject);
				if(!safeObjects.Contains(currentObject))
					safeObjects.Add (currentObject);
				if (moodManager.combo > 0) {
					//wrtie the edge to the graph
					if(!Edges[0].ContainsKey(previousObject))
					{
						Edges[0].Add(previousObject, null);
						Edges[0][previousObject].Add(currentObject);
					} else{
						if(!Edges[0][previousObject].Contains(currentObject)){
							Edges[0][previousObject].Add(currentObject);
						}
					}
			}

			if (prevRomance > moodManager.Mood.CurrentRomantic) {
				if(!romanceObjects.Contains(currentObject))
					romanceObjects.Add (currentObject);
				if(!safeObjects.Contains(currentObject))
					safeObjects.Add (currentObject);
				if (moodManager.combo > 0) {
					//wrtie the edge to the graph
					if(!Edges[1].ContainsKey(previousObject))
					{
						Edges[1].Add(previousObject, null);
						Edges[1][previousObject].Add(currentObject);
					}
					else{
						if(!Edges[1][previousObject].Contains(currentObject)){
							Edges[1][previousObject].Add(currentObject);
						}
					}
				}
			}
				
			previousObject = currentObject;
			currentObject = null;
			moving = false;
		}
	}
}
}
