using Scare.AI.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Scare.AI.Components.FOVComponent;

public class CatController : MonoBehaviour
{

    EntityWanderer wanderer;
    FOVComponent fovChecker;
    void Awake()
    {
        wanderer = GetComponent<EntityWanderer>();
        fovChecker = GetComponent<FOVComponent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fovChecker.foundItemCallBack += OnFoundItemOfInterest;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFoundItemOfInterest(Collider closestItemOfInterest)
    {
        //for debug purposes: turn indication light red
        Color previousColor = GetComponentInChildren<Light>().color;
        GetComponentInChildren<Light>().color = Color.red;
        //Debug.Log("found item of interest: " + closestItemOfInterest.gameObject.name + " at " + closestItemOfInterest.gameObject.transform.position);
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Init(0.7f, delegate
        {
            GetComponentInChildren<Light>().color = previousColor;
            Destroy(timer);
        });
    }
}
