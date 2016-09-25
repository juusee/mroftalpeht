using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlatformContainerManager : MonoBehaviour {

	public Transform player;
	public Transform platformSpawnPoint;
	public GameObject platformContainer;
	public GameObject startPath;
	public GameObject smallPlatform;
	public GameObject semiSmallPlatform;
	public GameObject normalPlatform;

	public float playerDistanceFromEdge;
	public float generationDistanceFromPlayer;

	public GameObjectFollower camera;
	
	public ButtonEventHandler leftButton;
	public ButtonEventHandler rightButton;

	//private PlatformContainerMovement platformContainerMovement;
	private List<GameObject> platformContainers;
	private List<GameObject> movingPlatformContainers;
	private List<GameObject> normalPlatforms;
	private List<GameObject> smallPlatforms;
	private List<GameObject> semiSmallPlatforms;

	private float prevPlatformLength;

	private Vector3 platformSpawnPointPosition;
	private Quaternion platformSpawnPointRotation;

	private List<float> followYDistances;

	private string[] modes;
	private string mode;
	private string game;

	private bool enableAccelometer;
	private int accelometerPower;

	void FixedUpdate() {
		if ((platformSpawnPoint.position.x) < player.position.x + generationDistanceFromPlayer) {
			spawnPlatform ();
		}
		// todo
		/*if (followYDistances.Count > 0 && followYDistances [0] - player.transform.position.x < 30) {
			camera.setFollowY (true);
			followYDistances.RemoveAt (0);
		} else if (followYDistances.Count == 0) {
			// todo change not to set on every frame?
			camera.setFollowY (false);
		}*/
	}

	void spawnPlatform() {		
		GameObject platformContainer = getPlatformContainer ();
		float playerVelocity = player.GetComponent<Rigidbody> ().velocity.x;
		// To keep fixed distance the from edge, block minimun speed is
		// playerVelocity * (blockTravelDistance / (generationDistanceFromPlayer - playerDistanceFromEdge))

		platformContainer.GetComponent<PlatformContainerMovement>().setInverse(false);
		platformContainer.GetComponent<PlatformContainerMovement>().setInverse(false);
		platformSpawnPoint.GetComponent<PlatformContainerMovement>().setInverse(false);
		if (game == "upsideDown") {
			platformContainer.GetComponent<PlatformContainerMovement>().setInverse(true);
			platformContainer.GetComponent<PlatformContainerMovement>().setInverse(true);
			platformSpawnPoint.GetComponent<PlatformContainerMovement>().setInverse(true);
		}

		GameObject platform = platformContainer.transform.GetChild (0).gameObject;
		GameObject newPlatform;
		if (Random.value > 0.5) {
			newPlatform = getSmallPlatform ();
		} else {
			newPlatform = getSemiSmallPlatform ();
		}
		newPlatform.SetActive (true);
		Collider platformCollider = newPlatform.GetComponent<Collider> ();
		float platformLength = platformCollider.bounds.size.x;
		float gap = 15f;

		newPlatform.transform.parent = platform.transform.parent;
		newPlatform.transform.position = new Vector3 (0, -20, 0);
		newPlatform.transform.localRotation = Quaternion.identity;
		platform.SetActive (false);
		platform.transform.parent = null;

		platformContainer.GetComponent<PlatformManager> ().setPlatformSpeed (playerVelocity * (20 / (generationDistanceFromPlayer - playerDistanceFromEdge)));
		newPlatform.SetActive (true);

		// todo better place
		modes = new string[]{"rollRight", "rollLeft"};
		float[] modeWeights = new float[]{3f, 3f};
		float weightSum = 0f;
		for (int i = 0; i < modeWeights.Length; ++i) {weightSum += modeWeights [i];}
		float random = Random.Range (0f, 1f);
		float tempSum = 0f;
		int index = -1;
		while (tempSum < random) {
			++index;
			tempSum += modeWeights[index] / weightSum;
		}
		mode = modes[index];

		if (mode == "rollRight") {
			platformSpawnPoint.Rotate (Vector3.right * 30);
			platformSpawnPoint.position = new Vector3 (
				platformSpawnPoint.position.x + platformLength / 2 + prevPlatformLength / 2 + gap,
				platformSpawnPoint.position.y,
				platformSpawnPoint.position.z
			);
		} else if (mode == "rollLeft") {
			platformSpawnPoint.Rotate (Vector3.right * -30);
			platformSpawnPoint.position = new Vector3 (
				platformSpawnPoint.position.x + platformLength / 2 + prevPlatformLength / 2 + gap,
				platformSpawnPoint.position.y,
				platformSpawnPoint.position.z
			);
		} else if (mode == "noRollUp") {
			//gap = 10f;
			platformSpawnPoint.position = new Vector3 (
				platformSpawnPoint.position.x + platformLength / 2 + prevPlatformLength / 2 + gap,
				platformSpawnPoint.position.y + 3,
				platformSpawnPoint.position.z
			);
			followYDistances.Add (platformSpawnPoint.position.x);
		}

		platformContainer.transform.position = platformSpawnPoint.position;
		platformContainer.transform.rotation = platformSpawnPoint.rotation;
		platformContainer.SetActive (true);

		prevPlatformLength = platformLength;
	}

	GameObject getPlatformContainer() {		
		GameObject platformContainer = null;
		int nonActiveCount = 0;
		for (int i = 0; i < platformContainers.Count; ++i) {
			if (!platformContainers[i].activeSelf || player.transform.position.x - platformContainers [i].transform.position.x > 30) {
				platformContainer = platformContainers [i];
			}
			if (!platformContainers [i].activeSelf) {
				++nonActiveCount;
			}
		}
		if (platformContainer == null) {
			platformContainer = (GameObject)Instantiate (this.platformContainer);
			platformContainers.Add (platformContainer);
		}
		platformContainer.SetActive (false);
		return platformContainer;
	}

	GameObject getSmallPlatform() {
		GameObject smallPlatform = null;
		for (int i = 0; i < smallPlatforms.Count; ++i) {
			if (!smallPlatforms [i].activeSelf) {
				smallPlatform = smallPlatforms [i];
			}
		}
		if (smallPlatform == null) {
			smallPlatform = (GameObject)Instantiate (this.smallPlatform);
			smallPlatforms.Add (smallPlatform);
		}
		smallPlatform.SetActive (false);
		return smallPlatform;
	}

	GameObject getSemiSmallPlatform() {
		GameObject semiSmallPlatform = null;
		for (int i = 0; i < semiSmallPlatforms.Count; ++i) {
			if (!semiSmallPlatforms [i].activeSelf) {
				semiSmallPlatform = semiSmallPlatforms [i];
			}
		}
		if (semiSmallPlatform == null) {
			semiSmallPlatform = (GameObject)Instantiate (this.semiSmallPlatform);
			semiSmallPlatforms.Add (semiSmallPlatform);
		}
		semiSmallPlatform.SetActive (false);
		return semiSmallPlatform;
	}

	public void Setup() {
		// todo better
		platformContainers = new List<GameObject> ();
		movingPlatformContainers = new List<GameObject> ();
		normalPlatforms = new List<GameObject> ();
		smallPlatforms = new List<GameObject> ();
		semiSmallPlatforms = new List<GameObject> ();

		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("platformContainer");
		for (int i = 0; i < gameObjects.Length; ++i) {
			GameObject temp = gameObjects [i];
			temp.SetActive (false);
			platformContainers.Add (temp);
		}
		platformSpawnPointPosition = platformSpawnPoint.position;
		platformSpawnPointRotation = platformSpawnPoint.rotation;

		followYDistances = new List<float> ();

		mode = "rollRight";
	}

	public void SetGame(string game) {
		this.game = game;
		if (game == "upsideDown") {
			platformSpawnPoint.Rotate (Vector3.right * 180);
			platformSpawnPoint.position = new Vector3 (
				platformSpawnPoint.position.x,
				platformSpawnPoint.position.y - 40,
				platformSpawnPoint.position.z
			);
		}
	}
		
	public void Reset() {
		// todo better
		for (int i = 0; i < platformContainers.Count; ++i) {
			platformContainers [i].SetActive (false);
		}
		platformSpawnPoint.position = platformSpawnPointPosition;
		platformSpawnPoint.rotation = platformSpawnPointRotation;
		startPath.transform.rotation = Quaternion.identity;
		prevPlatformLength = 0;
	}

	public void DisableControl() {
		startPath.GetComponent<PlatformContainerMovement>().enabled = false;
		platformSpawnPoint.GetComponent<PlatformContainerMovement>().enabled = false;
		for (int i = 0; i < platformContainers.Count; ++i) {
			platformContainers[i].GetComponent<PlatformContainerMovement>().enabled = false;
		}
	}

	public void EnableControl() {
		startPath.GetComponent<PlatformContainerMovement>().enabled = true;
		platformSpawnPoint.GetComponent<PlatformContainerMovement>().enabled = true;
		for (int i = 0; i < platformContainers.Count; ++i) {
			platformContainers[i].GetComponent<PlatformContainerMovement>().enabled = true;
		}
	}
}
