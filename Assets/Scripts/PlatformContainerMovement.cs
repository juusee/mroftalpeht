using UnityEngine;
using System.Collections;

public class PlatformContainerMovement : MonoBehaviour {

	public bool inverse;

	private static bool rollRight;
	private static bool rollLeft;
	private static bool enableAccelometer;
	private static int turnSpeed;

	void Start () {
	}

	void FixedUpdate () {
		if (enableAccelometer) {	
			float speed = -75 * turnSpeed * Time.deltaTime * Input.acceleration.x;
			/*if (Mathf.Abs (turnSpeed) > Mathf.Abs (120 * Time.deltaTime)) {
				turnSpeed = turnSpeed > 0 ? 120 * Time.deltaTime : -120 * Time.deltaTime;
			}*/
			if (inverse) {
				speed = -speed;
			}
			transform.Rotate (speed, 0, 0);
		}

		if (rollLeft || Input.GetKey (KeyCode.LeftArrow)) {
			if (inverse) {
				RollLeft ();
			} else {
				RollRight ();
			}
		}

		if (rollRight || Input.GetKey (KeyCode.RightArrow)) {
			if (inverse) {
				RollRight ();
			} else {
				RollLeft ();
			}
		}
	}

	public void RollRight() {
		transform.Rotate (10 * turnSpeed * Time.deltaTime, 0, 0);
	}

	public void RollLeft() {
		transform.Rotate (-10 * turnSpeed * Time.deltaTime, 0, 0);
	}

	public void MoveRight() {
		Vector3 targetPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 10);
		float speed = -120 * turnSpeed * Time.deltaTime;
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, step);
	}

	public void MoveLeft() {
		Vector3 targetPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 10);
		float speed = -120 * turnSpeed * Time.deltaTime;
		float step = speed * Time.deltaTime;
		transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPosition, step);
	}

	public void setInverse(bool inverse) {
		this.inverse = inverse;	
	}

	public static void setRollRight(bool roll) {
		rollRight = roll;
	}

	public static void setRollLeft(bool roll) {
		rollLeft = roll;
	}

	public static void setEnableAccelometer(bool enable) {
		enableAccelometer = enable;
	}
	
	public static void setTurnSpeed(int speed) {
		turnSpeed = speed;
	}
}
