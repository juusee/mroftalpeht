using UnityEngine;
using System.Collections;

public class GameObjectFollower : MonoBehaviour {

	private Vector3 gameObjectLastPosition;
	private float xDistanceToMove;
	private float yDistanceToMove;
	private float zDistanceToMove;

	public Transform gameObjectTransform;
	public bool followX;
	public bool followY;
	public bool followZ;

	// Use this for initialization
	void Start () {
		gameObjectLastPosition = gameObjectTransform.position;
		xDistanceToMove = 0;
		yDistanceToMove = 0;
		xDistanceToMove = 0;
	}

	// Update is called once per frame
	void Update () {
		if (followX) {
			xDistanceToMove = gameObjectTransform.position.x - gameObjectLastPosition.x;
		}
		if (followY) {
			yDistanceToMove = gameObjectTransform.position.y - gameObjectLastPosition.y;
		}
		if (followZ) {
			zDistanceToMove = gameObjectTransform.position.z - gameObjectLastPosition.z;
		}

		transform.position = new Vector3(transform.position.x + xDistanceToMove, 
			transform.position.y + yDistanceToMove, 
			transform.position.z + zDistanceToMove);

		gameObjectLastPosition = gameObjectTransform.position;
	}

	public void setFollowY(bool followY) {
		this.followY = followY;
	}
}
