using UnityEngine;
using System.Collections;

public class CottagePorchlight : MonoBehaviour {

	//Attached to porchlight, it toggles the pointlight and adjusts emissive material (lightbulb) realtime. Event is available if 'playerInRange' is set to true
	//externally. In this demo example, 'CottageWindow' casts ray and recognizes the presence of this class (and toggles the boolean).

	public bool isOn;
	public AudioClip lightSwitchSound;
	[HideInInspector] public bool playerInRange;

	Light pointLight;
	Renderer r;

	void Start () {
		pointLight = GetComponentInChildren<Light>();
		r = GetComponent<Renderer>();
		if (pointLight.enabled) {
			isOn = true;
		}
	}

	void Update () {
		if (playerInRange) {
			if (Input.GetKeyDown(KeyCode.E)) {
				if (lightSwitchSound) 		AudioSource.PlayClipAtPoint(lightSwitchSound, this.transform.position);
				if (isOn) {
					pointLight.enabled = false;
					isOn = false;
					Color color = Color.yellow * Mathf.LinearToGammaSpace(0.0001f);
					r.materials[1].SetColor("_EmissionColor", color);
					DynamicGI.SetEmissive(r, color);
				}
				else {
					pointLight.enabled = true;
					Color color = Color.yellow * Mathf.LinearToGammaSpace(8);
					r.materials[1].SetColor("_EmissionColor", color);
					DynamicGI.SetEmissive(r, color);
					isOn = true;
				}
			}
		}
	}
}
