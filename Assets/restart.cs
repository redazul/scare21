using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("SallysHome Takuto's Edit");
        }
        
    }
}
