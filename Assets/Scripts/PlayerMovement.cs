using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	Rigidbody playerRigidbody;
	bool isGrounded;
	float startXVelocity;
	float startYVelocity;
	bool moveOnZAxis;
	bool usingAccelometer;

	float speedIncrease;

	public float xVelocity;
	public float yVelocity;
	public float zVelocity;
	public float gravity;

	public GameObject scoreCanvas;

	static bool jump;

	float startJumpPos = 0;
	float endJumpPos = 0;
	bool beenAir = false;
	float lastPos = 0;
	float totalPaska = 0;

	int previousPlatformID;

	void Awake () {
		playerRigidbody = GetComponent<Rigidbody> ();
		Physics.gravity = new Vector3(0, -gravity, 0);
		startXVelocity = xVelocity;
		startYVelocity = yVelocity;
		moveOnZAxis = false;
		jump = false;
	}

	void OnEnable () {
		playerRigidbody.isKinematic = false;
		xVelocity = startXVelocity;
		yVelocity = startYVelocity;
		speedIncrease = 0f;
	}

	void OnDisable() {
		playerRigidbody.isKinematic = true;
	}

	void Update() {
		float playerVelocityZAxis = moveOnZAxis ? playerRigidbody.velocity.z : 0;

		if (usingAccelometer) {
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase == TouchPhase.Began && isGrounded) {
					playerRigidbody.velocity = new Vector3 (playerRigidbody.velocity.x, yVelocity, playerVelocityZAxis);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			jump = true;
		}

		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			jump = false;
		}

		/*if (transform.position.x >= 40 && startJumpPos == 0) {
			startJumpPos = transform.position.x;
			//print ("startpos " + startJumpPos);
			//print ("startvel " + playerRigidbody.velocity.x);
			jump = true;
		}

		if (transform.position.x > 40 && isGrounded && beenAir && endJumpPos == 0) {
			endJumpPos = transform.position.x;
			print ("endpos " + endJumpPos);
			print ("endvel " + playerRigidbody.velocity.x);
			print ("total " + (endJumpPos - startJumpPos));
		}

		if (isGrounded) {
			beenAir = false;
		} else {
			beenAir = true;
		}*/

		if (jump && isGrounded) {
			playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, yVelocity, playerVelocityZAxis);
			//print ("X: " + ((transform.position.x - lastPos) * yVelocity * 2));
			//yVelocity = 17f / ((transform.position.x - lastPos) * 2f);
			//print ((17 / ((transform.position.x - lastPos) * 2)));
			jump = false;
		}

		/*if (playerRigidbody.velocity.y != 0) {
			totalPaska += (transform.position.x - lastPos);
			print (playerRigidbody.velocity.y + ": " + totalPaska);
			if (playerRigidbody.velocity.y == (yVelocity - yVelocity * 2 + 1) || playerRigidbody.velocity.y == (yVelocity - yVelocity * 2 + 2)) {
				print ("GRAND TOTAL: " + (transform.position.x - startJumpPos));
			}
		}*/	
	}

	void FixedUpdate () {
		float playerVelocityZAxis = moveOnZAxis ? playerRigidbody.velocity.z : 0;
		isGrounded = Physics.Raycast (transform.position, - Vector3.up, 2);
		playerRigidbody.velocity = new Vector3 (xVelocity, playerRigidbody.velocity.y, playerVelocityZAxis);
		if ((int) transform.position.x / 200 > speedIncrease) {
			speedIncrease++;
			xVelocity++;
			// todo better place
			Collider coll = GetComponent<Collider>();
			float deltaTimeWithFriction = Time.deltaTime * (1 - coll.material.dynamicFriction / 10);
			float jumpLength = 22;
			// Distance traveled in one frame is xVelocity * deltaTimeWithFriction
			yVelocity = jumpLength / (xVelocity * deltaTimeWithFriction * 2);
		}
		lastPos = transform.position.x;
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "floor") {
			gameObject.SetActive (false);
		}
		if (col.gameObject.tag == "platform" && col.gameObject.GetInstanceID() != previousPlatformID) {
			previousPlatformID = col.gameObject.GetInstanceID();
			string newScore = "" + (int.Parse(scoreCanvas.GetComponentInChildren<Text> ().text) + 1);
			scoreCanvas.GetComponentInChildren<Text> ().text = newScore;
		}
	}

	public void setMoveOnZAxis(bool moveOnZAxis) {
		this.moveOnZAxis = moveOnZAxis;
	}

	public static void setJump(bool j) {
		jump = j;
	}

	public void setUsingAccelometer(bool usingAccelometer) {
		this.usingAccelometer = usingAccelometer;
	}
}
