using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

public class detect_collision : MonoBehaviour
{

    public Animator _anim;
    public GameObject _player;
    public PlayerInput _playerInput;
    public ThirdPersonController _moveScript;
    public anim_boss _animScript;
    public CharacterController _chrController;
   
     void OnTriggerEnter(Collider other) {
         Debug.Log(other.name);
         if(other.name== "WallCollider")
         {

             _anim.SetBool("climb",true);
             _animScript.climbAnim = true;
             _moveScript.enabled = false;
             _playerInput.enabled = false;
         }

          if(other.name== "TopCollider")
         {

             _anim.SetBool("top",true);
             _animScript.climbAnim = true;
             _moveScript.enabled = false;
             _playerInput.enabled = false;
         }

        if(other.name== "EndCollider")
         {
             Debug.Log("last collider");

             _animScript.climbAnim = false;
             _animScript.moveForward = false;
             _anim.SetBool("climb",false);
              _anim.SetBool("top",false);
            
   
             _moveScript.enabled = true;
             _playerInput.enabled = true;
         }
            
     }

    //    void OnCollisionExit(Collision collisionInfo)
    // {
    //     print("No longer in contact with " + collisionInfo.transform.name);
    // }
}
