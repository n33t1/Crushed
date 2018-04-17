using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour {

	// Use this for initialization
	public GameObject[] objects;
	private List<GameObject> unexploredObjects;
	private List<GameObject> safeObjects;
	private List<GameObject> romanceObjects;
	private List<GameObject> happyObjects;
	public Transform player;
	public MoodManager moodManager;
	public Image image;
	public GameObject currentObject;
	[SerializeField]
	private GameObject previousObject;
	public float prevHappy;
	public float prevRomance;
	public GameObject Girl;
	public float bulletSpeed = 2f;
	public AudioClip fireSound;
	public float fireRate = 3f;
	private bool executingCombo = false;
	private int comboIndex;
	public GameObject bulletPrefab;
	private string status = "safe";
	private bool moving = false;
	private List<GameObject> MaxHappyCombo;
	private float maxHappyComboVal;
	private List<GameObject> MaxRomanticCombo;
	private float maxRomanticComboVal;
	private bool firing = false;
	private List<GameObject> TempList;
	private float tempComboVal = 0;

	//0 for friendship edges, 1 for romantic edges
	private Dictionary<GameObject, List<GameObject>>[] Edges = new Dictionary<GameObject, List<GameObject>>[2];

	void Start () {
		safeObjects = new List<GameObject>();
		romanceObjects = new List<GameObject>();
		happyObjects = new List<GameObject>();
		TempList = new List<GameObject>();
		MaxHappyCombo = new List<GameObject>();
		MaxRomanticCombo = new List<GameObject>();
		LoadObjects ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateStatus ();
		if (!moving) {
			GoalBehaviour ();
		}

		if (unexploredObjects.Count <= 0) {
			LoadObjects();
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
		if (Mathf.Pow (moodManager.Mood.CurrentHappy, 2) + Mathf.Pow (moodManager.Mood.CurrentRomantic, 2) <= 1000f) {
			status = "winning";
		} else if (Mathf.Pow(moodManager.Mood.CurrentHappy, 2) +  Mathf.Pow(moodManager.Mood.CurrentRomantic, 2) <= 5000f) {
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

		if (status == "winning") {
			WinningBehaviour ();
		} else if (status == "safe") {
			SafeBehaviour ();
		} else if (status == "danger") {
			DangerBehaviour ();
		} else {
			CriticalBehaviour ();
		}
	}

	void WinningBehaviour ()
	{
		firing = false;
		CancelInvoke();
		gameObject.GetComponent<NavMeshAgent2D> ().speed = 1.0f;
		float probability = (float)Random.Range (0, 100)/100f;
		if (probability <= 0.75f) {
			SafeBehaviour ();
		} else {
			gameObject.GetComponent<NavMeshAgent2D> ().destination = NextObjectPosition ();
		}
	}

	void SafeBehaviour ()
	{
		firing = false;
		CancelInvoke();
		gameObject.GetComponent<NavMeshAgent2D> ().speed = 1.0f;
		if (unexploredObjects.Count > 0) {
			int index = Random.Range (0, unexploredObjects.Count);
			gameObject.GetComponent<NavMeshAgent2D>().destination = unexploredObjects[index].transform.position;
			unexploredObjects.RemoveAt(index);
		}
	}

	void DangerBehaviour ()
	{
		firing = false;
		CancelInvoke();
		gameObject.GetComponent<NavMeshAgent2D> ().speed = 1.5f;
		float probability = (float)Random.Range (0, 100)/100f;
		if (probability <= 0.25f) {
			SafeBehaviour ();
		} else {
			if (!executingCombo) {
				if (moodManager.Mood.CurrentHappy > moodManager.Mood.CurrentRomantic) {
					if (MaxHappyCombo.Count > 0) {
						executingCombo = true;
						comboIndex = 0;
						gameObject.GetComponent<NavMeshAgent2D> ().destination = MaxHappyCombo [0].gameObject.transform.position;
					} else {
						gameObject.GetComponent<NavMeshAgent2D> ().destination = NextObjectPosition ();
					}
				} else {
					if (MaxRomanticCombo.Count > 0) {
						executingCombo = true;
						comboIndex = 1;
						gameObject.GetComponent<NavMeshAgent2D> ().destination = MaxRomanticCombo [0].gameObject.transform.position;
					} else {
						gameObject.GetComponent<NavMeshAgent2D> ().destination = NextObjectPosition ();
					}
				}
			} else {
				gameObject.GetComponent<NavMeshAgent2D> ().destination = NextObjectPosition ();
			}
		}
	}

	void CriticalBehaviour ()
	{
		gameObject.GetComponent<NavMeshAgent2D> ().speed = 0.5f;
		gameObject.GetComponent<NavMeshAgent2D> ().destination = player.position - Vector3.one;
		if (!firing) {
			InvokeRepeating ("Fire", 0.01f, fireRate);
			firing = true;
		}
		moving = false;
	}

	void Fire ()
	{
		if (EnemyBullet.usedBullets < EnemyBullet.maxBullets) {
			GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			Vector2 fireDirection = player.position - transform.position;
			fireDirection = fireDirection.normalized;
			bullet.GetComponent<EnemyBullet> ().originPoint = transform.position;
			bullet.GetComponent<Rigidbody2D> ().velocity = fireDirection * bulletSpeed;
			if (fireDirection != Vector2.zero) {
				float angle = Mathf.Atan2 (fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
				bullet.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}
			AudioSource.PlayClipAtPoint (fireSound, transform.position);
		}
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (collider.gameObject.tag == "Objects" && currentObject == null
		    && gameObject.GetComponent<NavMeshAgent2D> ().velocity.magnitude == 0f) {
			currentObject = collider.gameObject;
			gameObject.GetComponent<NavMeshAgent2D> ().destination = Girl.transform.position;
			if (image.sprite == null) {
				image.sprite = collider.gameObject.GetComponent<SpriteRenderer> ().sprite;
				Color c = image.color;
				c.a = 255;
				image.color = c;
			}
		} else if (collider.gameObject.tag == "Girl" && currentObject != null) {
			if (status == "safe" || status == "danger") {
				CreateGraph ();
			}
			previousObject = currentObject;
			currentObject = null;
			moving = false;
		} else if (collider.gameObject.tag == "Objects" && currentObject != null) {
			gameObject.GetComponent<NavMeshAgent2D> ().destination = Girl.transform.position;
		}
	}

	private Vector2 NextObjectPosition ()
	{
		if (executingCombo) {
			return NextComboPosition();
		} 
		else {
			if (moodManager.Mood.CurrentHappy > moodManager.Mood.CurrentRomantic) {
				return NextHappyPosition();
			} 
			else {
				return NextRomanticPosition();
			}
		}
	}

	private Vector2 NextComboPosition ()
	{
		if (Edges [comboIndex] [previousObject].Count == 0) {
			executingCombo = false;
			if (comboIndex == 0) {
				if (maxHappyComboVal < tempComboVal) {
					MaxHappyCombo.Clear ();
					MaxHappyCombo.AddRange (TempList);
					maxHappyComboVal = tempComboVal;
				}
			} else {
				if (maxRomanticComboVal < tempComboVal) {
					MaxRomanticCombo.Clear ();
					MaxRomanticCombo.AddRange (TempList);
					maxRomanticComboVal = tempComboVal;
				}
			}
			TempList.Clear ();
			tempComboVal = 0f;
			return NextObjectPosition ();
		} else {
			int maxValueIndex = 0;
			float maxValue = 0;
			for (int i = 0; i < Edges [comboIndex] [previousObject].Count; i++) {
				if (maxValue < Edges [comboIndex] [previousObject] [i].GetComponent<ObjectManager> ().objectPoints) {
					maxValue = Edges [comboIndex] [previousObject] [i].GetComponent<ObjectManager> ().objectPoints;
					maxValueIndex = i;
				}
			}

			TempList.Add(Edges [comboIndex] [previousObject] [maxValueIndex].gameObject);
			tempComboVal += Edges [comboIndex] [previousObject] [maxValueIndex].gameObject.GetComponent<ObjectManager>().objectPoints;
			return Edges [comboIndex] [previousObject] [maxValueIndex].gameObject.transform.position;
		}
	}

	private Vector2 NextHappyPosition ()
	{
		if (Edges[0] != null && Edges [0].Count > 0) {
			while (true) {
				if (happyObjects.Count > 0) {
					int ind = Random.Range (0, happyObjects.Count);
					if (Edges [0].ContainsKey (happyObjects [ind])) {
						comboIndex = 0;
						executingCombo = true;
						TempList.Add(Edges [0] [happyObjects [ind]] [0].gameObject);
						tempComboVal += Edges [0] [happyObjects [ind]] [0].gameObject.GetComponent<ObjectManager>().objectPoints;
						return Edges [0] [happyObjects [ind]] [0].gameObject.transform.position;
					}
				}
			}
		} else if (happyObjects.Count > 0) {
			return happyObjects [Random.Range (0, happyObjects.Count)].gameObject.transform.position;
		} else {
			return unexploredObjects[Random.Range (0, unexploredObjects.Count)].gameObject.transform.position;
		}
	}

	private Vector2 NextRomanticPosition ()
	{
		if (Edges[1] != null && Edges [1].Count > 0) {
			while (true) {
				if (romanceObjects.Count > 0) {
					int ind = Random.Range (0, romanceObjects.Count);
					if (Edges [1].ContainsKey (romanceObjects [ind])) {
						comboIndex = 1;
						executingCombo = true;
						TempList.Add(Edges [1] [romanceObjects [ind]] [0].gameObject);
						tempComboVal += Edges [1] [romanceObjects [ind]] [0].gameObject.GetComponent<ObjectManager>().objectPoints;
						return Edges [1] [romanceObjects [ind]] [0].gameObject.transform.position;
					}
				}
			}
		} else if (romanceObjects.Count > 0) {
			return romanceObjects [Random.Range (0, romanceObjects.Count)].gameObject.transform.position;
		} else {
			return unexploredObjects[Random.Range (0, unexploredObjects.Count)].gameObject.transform.position;
		}
	}

	private void CreateGraph ()
	{
		if (prevHappy > moodManager.Mood.CurrentHappy) {
				if (!happyObjects.Contains (currentObject))
					happyObjects.Add (currentObject);
				if (!safeObjects.Contains (currentObject))
					safeObjects.Add (currentObject);
				if (moodManager.combo > 0) {
					//wrtie the edge to the graph
					if (!Edges [0].ContainsKey (previousObject)) {
						Edges [0].Add (previousObject, null);
						Edges [0] [previousObject].Add (currentObject);
					} else {
						if (!Edges [0] [previousObject].Contains (currentObject)) {
							Edges [0] [previousObject].Add (currentObject);
						}
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
	}
}
