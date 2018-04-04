using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	private float xmin, xmax, ymin, ymax;
	private int lIndex, rIndex, uIndex, dIndex;
	public Sprite[] LeftMovement;
	public Sprite[] RightMovement;
	public Sprite[] UpMovement;
	public Sprite[] DownMovement;
	private Vector3 distVector;
	private Vector3 prevLoc = Vector3.zero;
	public float spriteChangeDist;
	public bool canMove;
	private Rigidbody2D rigidBody;

	[SerializeField]
	public Stat Health;
	private bool paused = false;


	void Start () {
		lIndex = rIndex = uIndex = dIndex = 0;
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance));
		Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		xmin = leftmost.x + (GetComponent<Renderer>().bounds.size.x)/2;
		xmax = rightmost.x - (GetComponent<Renderer>().bounds.size.x)/2;
		ymin = downmost.y + (GetComponent<Renderer>().bounds.size.y)/2;
		ymax = upmost.y - (GetComponent<Renderer>().bounds.size.y)/2;
		rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!paused)
			Move ();
		else {
			GetComponent<NavMeshAgent2D> ().destination = transform.position;
		}
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (collider.gameObject.tag == "GardenObjects" || collider.gameObject.name == "Girl") {
			float minYObject = collider.gameObject.transform.position.y - (collider.gameObject.GetComponent<Renderer> ().bounds.size.y) / 2;
			float minYPlayer = transform.position.y - (GetComponent<Renderer> ().bounds.size.y) / 2;
			if (minYPlayer < minYObject) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, collider.gameObject.transform.position.z - 1);
			} else {
				transform.position = new Vector3 (transform.position.x, transform.position.y, collider.gameObject.transform.position.z + 1);
			}
		}
	}

	private void UpdateVector ()
	{
		distVector = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		HitWithBullet(collider);
	}

	void HitWithBullet (Collider2D collider)
	{
		PlayerBullet missile = collider.gameObject.GetComponent<PlayerBullet> ();

		if (missile) {
			Health.CurrentHealth -= missile.GetDamage ();
			missile.Hit();
		}

		if (Health.CurrentHealth <= 0 && !paused) {
			paused = true;
			StartCoroutine("Wait");
		}
	}

	IEnumerator Wait ()
	{
		Vector3 currentDest = GetComponent<NavMeshAgent2D> ().destination;
		GetComponent<NavMeshAgent2D> ().destination = transform.position;
		yield return new WaitForSeconds(30f);
		GetComponent<NavMeshAgent2D> ().destination = currentDest;
		paused = false;
		Health.CurrentHealth = 100f;
		yield break;
	}

	void Move ()
	{
		Vector3 newPos = new Vector3 (transform.position.x, transform.position.y);
		float newX = Mathf.Clamp (newPos.x, xmin, xmax);
		float newY = Mathf.Clamp (newPos.y, ymin, ymax);

		rigidBody.MovePosition (new Vector2 (newX, newY));

		Vector3 curVel = (transform.position - prevLoc) / Time.deltaTime;
		curVel = curVel.normalized;
		if (Mathf.Abs (curVel.x) > Mathf.Abs (curVel.y)) {
			if (curVel.x > 0) {
				// it's moving right
				this.GetComponent<SpriteRenderer> ().sprite = RightMovement [rIndex];
				if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
					rIndex++;
					UpdateVector ();
				}
				if (rIndex >= RightMovement.Length) {
					rIndex = 0;
				}
			} else if (curVel.x < 0) {
				// it's moving left
				this.GetComponent<SpriteRenderer> ().sprite = LeftMovement [lIndex];
				if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
					lIndex++;
					UpdateVector ();
				}
				if (lIndex >= LeftMovement.Length) {
					lIndex = 0;
				}
			}
		} else {
			if (curVel.y > 0) {
				// it's moving up
				this.GetComponent<SpriteRenderer> ().sprite = UpMovement [uIndex];
				if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
					uIndex++;
					UpdateVector ();
				}
				if (uIndex >= UpMovement.Length) {
					uIndex = 0;
				}
			} else if (curVel.y < 0) {
				// it's moving down
				this.GetComponent<SpriteRenderer> ().sprite = DownMovement [dIndex];
				if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
					dIndex++;
					UpdateVector ();
				}
				if (dIndex >= DownMovement.Length) {
					dIndex = 0;
				}
			}
		}
     	prevLoc = transform.position;
		}
}
