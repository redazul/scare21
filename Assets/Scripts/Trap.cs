using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, ISpawnable
{
    private bool wasSpawned = false;
    public void SetSpawned(bool wasSpawned)
    {
        this.wasSpawned = wasSpawned;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
