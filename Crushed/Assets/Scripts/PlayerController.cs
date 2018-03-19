using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	private float xmin, xmax, ymin, ymax;
	private int lIndex, rIndex, uIndex, dIndex;
	public Sprite[] LeftMovement;
	public Sprite[] RightMovement;
	public Sprite[] UpMovement;
	public Sprite[] DownMovement;
	private Vector3 distVector;
	public float spriteChangeDist;
	public bool canMove;
	private Rigidbody2D rigidBody;

	[SerializeField]
	public Stat Health;
	//public GameObject bulletPrefab;
	//public float bulletSpeed = 5f;
	//public float firingRate = 5f;
	//public float health = 250f;

	//public AudioClip fireSound;

	// Use this for initialization
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
		if (Input.GetKeyDown (KeyCode.Q)) {
			Health.CurrentHealth -= 10;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			Health.CurrentHealth += 10;
		}

		if (canMove) {
			Move ();
		}
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			InvokeRepeating ("Fire", 0.00001f, firingRate);
//		}
//
//		if (Input.GetKeyUp (KeyCode.Space)) {
//			CancelInvoke("Fire");
//		}
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

	void Move ()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		float yInput = Input.GetAxisRaw("Vertical");

		Vector3 newPos = new Vector3(xInput, yInput);

		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
			UpdateVector ();
			lIndex++;
			if(lIndex >= LeftMovement.Length) {lIndex = 0;}
		} else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
			UpdateVector ();
			rIndex++;
			if(rIndex >= RightMovement.Length) {rIndex = 0;}
		} else if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
			UpdateVector ();
			uIndex++;
			if(uIndex >= UpMovement.Length) {uIndex = 0;}
		} else if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
			UpdateVector ();
			dIndex++;
			if(dIndex >= DownMovement.Length) {dIndex = 0;}
		}





		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			this.GetComponent<SpriteRenderer> ().sprite = LeftMovement [lIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				lIndex++;
				UpdateVector ();
			}
			if(lIndex >= LeftMovement.Length) {lIndex = 0;}
		} 

		else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			this.GetComponent<SpriteRenderer> ().sprite = RightMovement [rIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				rIndex++;
				UpdateVector ();
			}
			if(rIndex >= RightMovement.Length) {rIndex = 0;}
		} 

		else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			this.GetComponent<SpriteRenderer> ().sprite = UpMovement [uIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				uIndex++;
				UpdateVector ();
			}
			if(uIndex >= UpMovement.Length) {uIndex = 0;}
		}

		else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			this.GetComponent<SpriteRenderer> ().sprite = DownMovement [dIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				dIndex++;
				UpdateVector ();
			}
			if(dIndex >= DownMovement.Length) {dIndex = 0;}
		}



		float newX = transform.position.x + newPos.x * speed * Time.deltaTime;
		newX = Mathf.Clamp(newX, xmin, xmax);
		float newY = transform.position.y + newPos.y * speed * Time.deltaTime;
		newY = Mathf.Clamp(newY, ymin, ymax);

		rigidBody.MovePosition(new Vector2(newX, newY));

	}

//	void Fire ()
//	{
//		GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
//		bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, bulletSpeed, 0f);
//		AudioSource.PlayClipAtPoint(fireSound, transform.position);
//	}
//
//	void OnTriggerEnter2D (Collider2D collider)
//	{
//		Debug.Log("Player collided with missile");
//		PlayerBullet missile = collider.gameObject.GetComponent<PlayerBullet> ();
//
//		if (missile) {
//			health -= missile.GetDamage ();
//			missile.Hit();
//		}
//
//		if (health <= 0) {
//			Destroy(this.gameObject);
//		}
//	}
}
