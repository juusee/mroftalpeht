using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour {
	public float startDelay = 0.5f;
	public float endDelay = 2.5f;

	public PlatformContainerManager platformContainerManager;
	public PlayerManager playerManager;
	public GameObject panel;
	public GameObject startCounter;
	public GameObject scoreCanvas;
	public GameObject gameButtons;
	public ButtonEventHandler leftButton;
	public ButtonEventHandler rightButton;
	public ButtonEventHandler jumpButton;
	public ToggleGroup movementToggleGroup;
	public Slider turnSpeedSlider;
	public Camera mainCamera;

	private WaitForSeconds startWait;
	private WaitForSeconds endWait;

	private bool useGameButtons;

	private string gameName;

	public void setGameName(string gameName) {
		this.gameName = gameName;
		if (gameName == "randomSlippery") {
			playerManager.setMoveOnZAxis (true);
		} else {
			playerManager.setMoveOnZAxis (false);
		}
		platformContainerManager.SetGame (gameName);
	}

	void Start () {
		useGameButtons = true;
		startWait = new WaitForSeconds (startDelay);
		endWait = new WaitForSeconds (endDelay);

		playerManager.Setup ();
		platformContainerManager.Setup ();

		jumpButton.onPointerDown.AddListener(delegate {PlayerMovement.setJump (true);});
		jumpButton.onPointerUp.AddListener(delegate {PlayerMovement.setJump (false);});

		StartCoroutine (GameLoop());
	}

	private IEnumerator GameLoop () {		
		panel.SetActive (true);
		yield return StartCoroutine (RoundStarting());

		PlatformContainerMovement.setEnableAccelometer(false);
		PlatformContainerMovement.setTurnSpeed((int)turnSpeedSlider.value);

		// todo change so that should not remove on every round
		leftButton.onPointerDown.RemoveAllListeners ();
		leftButton.onPointerUp.RemoveAllListeners ();
		rightButton.onPointerDown.RemoveAllListeners ();
		rightButton.onPointerUp.RemoveAllListeners ();
		// These can be all the time
		leftButton.onPointerDown.AddListener(delegate {PlatformContainerMovement.setRollLeft (true);});
		leftButton.onPointerUp.AddListener(delegate {PlatformContainerMovement.setRollLeft (false);});
		rightButton.onPointerDown.AddListener(delegate {PlatformContainerMovement.setRollRight (true);});
		rightButton.onPointerUp.AddListener(delegate {PlatformContainerMovement.setRollRight (false);});

		string movementToggleTag = movementToggleGroup.ActiveToggles ().FirstOrDefault ().tag;
		if (movementToggleTag == "accelometer") {
			useGameButtons = false;
			PlatformContainerMovement.setEnableAccelometer(true);
			playerManager.setUsingAccelometer (true);
		} else if (movementToggleTag == "autoJump") {				
			leftButton.onPointerDown.AddListener(delegate {PlayerMovement.setJump (true);});
			leftButton.onPointerUp.AddListener(delegate {PlayerMovement.setJump (false);});
			rightButton.onPointerDown.AddListener(delegate {PlayerMovement.setJump (true);});
			rightButton.onPointerUp.AddListener(delegate {PlayerMovement.setJump (false);});
		} else {
			useGameButtons = true;
			playerManager.setUsingAccelometer (false);
		}

		panel.SetActive (false);

		if (Screen.width > Screen.height) {
			mainCamera.fieldOfView = 75;
		} else {
			mainCamera.fieldOfView = 97;
		}

		if (useGameButtons) {
			gameButtons.SetActive (true);
		}
		startCounter.SetActive (true);
		yield return StartCoroutine (StartCounter (3));
		startCounter.SetActive (false);
		scoreCanvas.SetActive (true);
		//platformContainerManager.setGameName (gameName);
		gameName = null;
		yield return StartCoroutine (RoundPlaying());
		yield return StartCoroutine (RoundEnding());
		scoreCanvas.SetActive (false);
		gameButtons.SetActive (false);
		//	SceneManager.LoadScene ("Main");
		StartCoroutine (GameLoop ());
	}

	private IEnumerator RoundStarting () {
		playerManager.Reset ();
		platformContainerManager.Reset ();
		
		playerManager.DisableControl ();
		platformContainerManager.DisableControl ();

		// starts game without menu
		// this.setGameName("something");

		while (gameName == null) {
			yield return null;
		}

		//yield return startWait;
	}

	private IEnumerator StartCounter (int count) {
		WaitForSeconds countWait = new WaitForSeconds (0.8f);
		startCounter.GetComponentInChildren<Text> ().text = count.ToString();
		yield return countWait;
		--count;
		if (count > 0) {
			yield return StartCounter (count);
		}
	}
		
	private IEnumerator RoundPlaying () {
		playerManager.EnableControl ();
		platformContainerManager.EnableControl ();

		while (playerManager.playerInstance.activeSelf) {
			yield return null;
		}
	}

	private IEnumerator RoundEnding () {
		playerManager.DisableControl ();
		platformContainerManager.DisableControl ();
		yield return endWait;
	}

	public void setUseGameButtons(bool use) {
		useGameButtons = use;
	}
}
