using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {
	
	void Start() {
	}

	void OnEnable() {		
	}

	public void setPlatformSpeed(float speed) {		
		for (int i = 0; i < transform.childCount; ++i) {
			transform.GetChild(i).GetComponent<PlatformMovement>().setSpeed(speed);
		}
	}
}
