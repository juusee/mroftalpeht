using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour {
	
	private Vector3 targetPosition;
	private float step;
	private float speed;
	private GameObject newShape;


	void Awake () {
		targetPosition = new Vector3 (0, -20, 0);
	}

	void FixedUpdate () {
		transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPosition, step);
	}

	void OnEnable() {
		//speed = minimumSpeed;
		step = speed * Time.deltaTime;
		transform.localPosition = new Vector3 (targetPosition.x, -40, targetPosition.z);
		// Direction towards center of cylinder
		//Vector3 direction = transform.localPosition - new Vector3(transform.localPosition.x, 0, 0);
		// Position 20 units towards that direction
		//targetPosition = direction / direction.magnitude * 20;
	}

	public void setSpeed(float speed) {
		this.speed = speed;
	}

	public float getSpeed() {
		return speed;
	}
}
