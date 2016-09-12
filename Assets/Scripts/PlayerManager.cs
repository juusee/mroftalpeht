using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public GameObject playerInstance;
	public Transform playerSpawnPoint;

	private PlayerMovement playerMovement;

	public void Setup() {
		playerMovement = playerInstance.GetComponent<PlayerMovement> ();
	}

	public void Reset() {
		playerInstance.transform.position = playerSpawnPoint.position;
		playerInstance.transform.rotation = playerSpawnPoint.rotation;
		playerInstance.SetActive(false);
		playerInstance.SetActive(true);
	}

	public void DisableControl() {
		playerMovement.enabled = false;
	}

	public void EnableControl() {
		playerMovement.enabled = true;
	}

	public void setMoveOnZAxis(bool moveOnZAxis) {
		playerMovement.setMoveOnZAxis(moveOnZAxis);
	}

	public void setUsingAccelometer(bool usingAccelometer) {
		playerMovement.setUsingAccelometer (usingAccelometer);
	}
}
