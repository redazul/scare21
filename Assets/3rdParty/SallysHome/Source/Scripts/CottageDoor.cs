using UnityEngine;
using System.Collections;

public class CottageDoor : MonoBehaviour {

	public float doorOpenAngle = 90.0f;
	public float doorClosedAngle = 0.0f;
	public float speed = 10.0f;

	public AudioClip doorSqueak;
	public Font font;

	Quaternion doorOpen = Quaternion.identity;
	Quaternion doorClosed = Quaternion.identity;

	bool doorStatus = false;
	//bool doorMoving = false;
	bool playerInRange;

	Transform player;

	void Start() {
		if (this.gameObject.isStatic) {
			Destroy (this);
		}
		doorOpen = Quaternion.Euler (0, doorOpenAngle, 0);
		doorClosed = Quaternion.Euler (0, doorClosedAngle, 0);
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		} else {
			Debug.Log ("No player tag found in scene! Continuing without interactions..");
			Destroy (this);
		}
	}

	void Update() {
		if (Vector3.Distance (player.position, this.transform.position) < 2f) {
			playerInRange = true;
			if (Input.GetKeyDown (KeyCode.E)) {
				if (doorStatus) {
					StartCoroutine (this.moveDoor (doorClosed));
					AudioSource.PlayClipAtPoint (doorSqueak, this.transform.position);
				} else {
					StartCoroutine (this.moveDoor (doorOpen));
					AudioSource.PlayClipAtPoint (doorSqueak, this.transform.position);
				}
			}
		} else {
			playerInRange = false;
		}
	}

	IEnumerator moveDoor(Quaternion target) {
		//doorMoving = true;

		while (Quaternion.Angle (transform.localRotation, target) > 0.5f) {
			transform.localRotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * speed);
			yield return null;
		}

		doorStatus = !doorStatus;
		//doorMoving = false;
		yield return null;

	}

	void OnGUI() {
		if (playerInRange) {
			string message;
			GUIStyle style = new GUIStyle ();
			style.fontSize = 35;
			style.normal.textColor = Color.white;
			style.font = font;
			if (!doorStatus) {
				message = "Press E to open";
			} else {
				message = "Press E to close";
			}
			Rect rect = new Rect (Screen.width / 2 - 100, Screen.height / 2 - 12f, 100, 25);
			GUI.Label (rect, message, style);
		}
	}

}
