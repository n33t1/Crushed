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
	private Vector3 newPos;
	public LevelManager levelManager;
	private Vector2 moveDirection = new Vector2(1f, 0f);
	[SerializeField]
	public Stat Health;
	public GameObject bulletPrefab;
	public float bulletSpeed = 5f;
	public float firingRate = 5f;
	public AudioClip fireSound;

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

		if (canMove) {
			Move ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.00001f, firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
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

	void Move ()
	{
		float xInput = Input.GetAxisRaw ("Horizontal");
		float yInput = Input.GetAxisRaw ("Vertical");

		if (xInput != 0f && yInput != 0f) {
			yInput = 0f;
		}; 

		newPos = new Vector3(xInput, yInput);

		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
			UpdateVector ();
			lIndex++;
			if(lIndex >= LeftMovement.Length) {lIndex = 0;}
		} 
		if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
			UpdateVector ();
			rIndex++;
			if(rIndex >= RightMovement.Length) {rIndex = 0;}
		} 
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
			UpdateVector ();
			uIndex++;
			if(uIndex >= UpMovement.Length) {uIndex = 0;}
		} 
		if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
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
			ChangeDirection(KeyCode.LeftArrow);
		} 

		else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			this.GetComponent<SpriteRenderer> ().sprite = RightMovement [rIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				rIndex++;
				UpdateVector ();
			}
			if(rIndex >= RightMovement.Length) {rIndex = 0;}
			ChangeDirection(KeyCode.RightArrow);
		} 

		else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			this.GetComponent<SpriteRenderer> ().sprite = UpMovement [uIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				uIndex++;
				UpdateVector ();
			}
			if(uIndex >= UpMovement.Length) {uIndex = 0;}
			ChangeDirection(KeyCode.UpArrow);
		}

		else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			this.GetComponent<SpriteRenderer> ().sprite = DownMovement [dIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				dIndex++;
				UpdateVector ();
			}
			if(dIndex >= DownMovement.Length) {dIndex = 0;}
			ChangeDirection(KeyCode.DownArrow);
		}



		float newX = transform.position.x + newPos.x * speed * Time.deltaTime;
		newX = Mathf.Clamp(newX, xmin, xmax);
		float newY = transform.position.y + newPos.y * speed * Time.deltaTime;
		newY = Mathf.Clamp(newY, ymin, ymax);

		rigidBody.MovePosition(new Vector2(newX, newY));

	}

	void Fire ()
	{
		if (PlayerBullet.usedBullets < PlayerBullet.maxBullets) {
			GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			bullet.GetComponent<PlayerBullet> ().originPoint = transform.position;
			bullet.GetComponent<Rigidbody2D> ().velocity = moveDirection * bulletSpeed;
			if (moveDirection != Vector2.zero) {
				float angle = Mathf.Atan2 (moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
				bullet.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}
			AudioSource.PlayClipAtPoint (fireSound, transform.position);
		}
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		HitWithBullet(collider);
	}

	void HitWithBullet (Collider2D collider)
	{
		EnemyBullet missile = collider.gameObject.GetComponent<EnemyBullet> ();

		if (missile) {
			Health.CurrentHealth -= missile.GetDamage ();
			missile.Hit();
		}

		if (Health.CurrentHealth <= 0) {
			levelManager.LoadLevel("Lose Screen");
		}
	}

	void ChangeDirection (KeyCode key)
	{
		if(key == KeyCode.UpArrow || key == KeyCode.W)			{moveDirection = new Vector2(0f, 1f);}

		else if(key == KeyCode.DownArrow || key == KeyCode.S)	{moveDirection = new Vector2(0f, -1f);}

		else if(key == KeyCode.LeftArrow || key == KeyCode.A)	{moveDirection = new Vector2(-1f, 0f);}

		else if(key == KeyCode.RightArrow || key == KeyCode.D)	{moveDirection = new Vector2(1f, 0f);}
		
	}
}
