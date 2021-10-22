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
         //Debug.Log(other.name);
         if(other.name == "PlayerArmature")
         {
             _anim.SetBool("climb",true);
            
            
             _moveScript.enabled = false;
             //_chrController.enabled = false;
             _playerInput.enabled = false;
             _animScript.climbAnim = true;

        
             
     
             
         }
            
     }
}
