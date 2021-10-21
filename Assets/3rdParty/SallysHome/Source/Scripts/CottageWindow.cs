using UnityEngine;
using System.Collections;

public class CottageWindow : MonoBehaviour {

	//Finds main camera in scene, recognizes windows and handles breaking/opening movement

	Camera cam;
	public float distance;
	public GameObject topShards;
	public GameObject bottomShards;
	[Range(1,20)]
	public float removeShardsAfter;

	public AudioClip glassBreak;
	public AudioClip windowOpen;
	public AudioClip windowClose;
	public Font font;

	Transform windowToSlide = null;
	const float yMax = 2.9f;
	const float yMin = 2.049026f;
	float direction;
	bool playerInRange;
	CottagePorchlight cpl = null;

	void Start() {
		cam = Camera.main;
		if (cam == null) {
			Debug.Log("No main camera found! Proceeding without interactions..");
			Destroy(gameObject);
		}
	}
			
	void Update() {

		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, distance)) {
			if (hit.transform.name.Contains("TOP")) {
				playerInRange = true;
				if (Input.GetKeyDown(KeyCode.G)) {
					BreakWindow(topShards, hit.transform);
				}
			}
			else if (hit.transform.name.Contains("Slide")) {
				playerInRange = true;
				if (Input.GetKeyDown(KeyCode.E)) {
					direction = isItOpen(hit.transform);
					windowToSlide = hit.transform;
				}
			}
			else if (hit.transform.name.Contains("BOT")) {
				playerInRange = true;
				if (Input.GetKeyDown(KeyCode.E)) {
					direction = isItOpen(hit.transform.parent);
					windowToSlide = hit.transform.parent;
				}
				if (Input.GetKeyDown(KeyCode.G)) {
					BreakWindow(bottomShards, hit.transform);
				}
			}
			else if (hit.transform.GetComponent<CottagePorchlight>()) {
				playerInRange = false;
				cpl = hit.transform.GetComponent<CottagePorchlight>();
				cpl.playerInRange = true;
			}
			else {
				playerInRange = false;
				if (cpl) {
					cpl.playerInRange = false;
					cpl = null;
				}
			}


		}

		if (windowToSlide) {
			SlideWindow(direction);
		}
	}

	private void BreakWindow(GameObject shards, Transform location) {
		Vector3 pos = location.position;
		Quaternion rot = location.rotation;
		Destroy(location.gameObject);
		GameObject go = (GameObject) Instantiate(shards, pos, rot);
		AudioSource.PlayClipAtPoint(glassBreak, pos);
		foreach ( Rigidbody rbs in go.GetComponentsInChildren<Rigidbody>()) {
			rbs.AddForce(cam.transform.forward * Random.Range(2,7), ForceMode.Impulse);
			Destroy(rbs.gameObject, removeShardsAfter);
		}
	}

	private void SlideWindow(float minOrMaxDirection) {
		Vector3 target = new Vector3(windowToSlide.transform.position.x, minOrMaxDirection, windowToSlide.transform.position.z);
		float dist = Vector3.Distance(windowToSlide.transform.position, target);
		windowToSlide.transform.position = Vector3.Lerp(windowToSlide.transform.position, target, Time.deltaTime *2);
		if (dist < 0.1f) {
			windowToSlide.transform.position = target;
		}
	}


	private float isItOpen(Transform windowTransform) {
		if (windowTransform.position.y >= yMax) 	{
			AudioSource.PlayClipAtPoint(windowClose, windowTransform.position);
			return yMin;
		}
		else {
			AudioSource.PlayClipAtPoint(windowOpen, windowTransform.position);
			return yMax;
		}
	}

	void OnGUI(){
		if (playerInRange) {
			GUIStyle style = new GUIStyle ();
			style.fontSize = 35;
			style.normal.textColor = Color.white;
			style.font = font;
			Rect rect = new Rect (Screen.width / 2 - 200, Screen.height / 2 - 12f, 200, 25);
			GUI.Label (rect, "E to open, G to break windows", style);
		}
	}


}

