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
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			UpdateVector ();
			lIndex = 0;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			UpdateVector ();
			rIndex = 0;
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			UpdateVector ();
			uIndex = 0;
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			UpdateVector ();
			dIndex = 0;
		}





		if (Input.GetKey (KeyCode.LeftArrow)) {
			this.transform.position += Vector3.left * speed * Time.deltaTime;
			this.GetComponent<SpriteRenderer> ().sprite = LeftMovement [lIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				lIndex++;
				UpdateVector ();
			}
			if(lIndex >= LeftMovement.Length) {lIndex = 0;}
		} 

		else if (Input.GetKey (KeyCode.RightArrow)) {
			this.transform.position += Vector3.right * speed * Time.deltaTime;
			this.GetComponent<SpriteRenderer> ().sprite = RightMovement [rIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				rIndex++;
				UpdateVector ();
			}
			if(rIndex >= RightMovement.Length) {rIndex = 0;}
		} 

		else if (Input.GetKey (KeyCode.UpArrow)) {
			this.transform.position += Vector3.up * speed * Time.deltaTime;
			this.GetComponent<SpriteRenderer> ().sprite = UpMovement [uIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				uIndex++;
				UpdateVector ();
			}
			if(uIndex >= UpMovement.Length) {uIndex = 0;}
		}

		else if (Input.GetKey (KeyCode.DownArrow)) {
			this.transform.position += Vector3.down * speed * Time.deltaTime;
			this.GetComponent<SpriteRenderer> ().sprite = DownMovement [dIndex];
			if (Vector3.Distance (transform.position, distVector) > spriteChangeDist) {
				dIndex++;
				UpdateVector ();
			}
			if(dIndex >= DownMovement.Length) {dIndex = 0;}
		}

//		if (Input.GetKeyDown (KeyCode.Space)) {
//			InvokeRepeating ("Fire", 0.00001f, firingRate);
//		}
//
//		if (Input.GetKeyUp (KeyCode.Space)) {
//			CancelInvoke("Fire");
//		}

		float newX = Mathf.Clamp(this.transform.position.x, xmin, xmax);
		float newY = Mathf.Clamp(this.transform.position.y, ymin, ymax);
		transform.position = new Vector3(newX, newY, transform.position.z);
	}

	private void UpdateVector ()
	{
		distVector = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}

	void OnCollisionEnter2D (Collision2D collider)
	{
		print("collision");
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
