using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	private Rigidbody playerRigidbody;
	private bool isGrounded;
	private float startXVelocity;
	private bool moveOnZAxis;
	private bool usingAccelometer;

	private float speedIncrease;

	public float xVelocity;
	public float yVelocity;
	public float zVelocity;
	public float gravity;

	public GameObject scoreCanvas;

	private static bool jump;

	void Awake () {
		playerRigidbody = GetComponent<Rigidbody> ();
		Physics.gravity = new Vector3(0, -gravity, 0);
		startXVelocity = xVelocity;
		moveOnZAxis = false;
		jump = false;
	}

	void OnEnable () {
		playerRigidbody.isKinematic = false;
		xVelocity = startXVelocity;
		speedIncrease = 0f;
	}

	void OnDisable() {
		playerRigidbody.isKinematic = true;
	}

	void Update() {

		if ((int) transform.position.x / 200 > speedIncrease) {
			speedIncrease++;
			xVelocity++;
		}

		float playerVelocityZAxis = moveOnZAxis ? playerRigidbody.velocity.z : 0;

		if (usingAccelometer) {
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase == TouchPhase.Began && isGrounded) {
					playerRigidbody.velocity = new Vector3 (playerRigidbody.velocity.x, yVelocity, playerVelocityZAxis);
				}
			}
		}

		if ((jump || Input.GetKey (KeyCode.UpArrow)) && isGrounded) {
			playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, yVelocity, playerVelocityZAxis);
		}
		scoreCanvas.GetComponentInChildren<Text> ().text = ((int) transform.position.x / 10).ToString();
	}

	void FixedUpdate () {
		float playerVelocityZAxis = moveOnZAxis ? playerRigidbody.velocity.z : 0;
		isGrounded = Physics.Raycast (transform.position, - Vector3.up, 2);
		playerRigidbody.velocity = new Vector3 (xVelocity, playerRigidbody.velocity.y, playerVelocityZAxis);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "floor") {
			gameObject.SetActive (false);
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
