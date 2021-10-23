using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

public class anim_boss : MonoBehaviour
{

    public Animator _anim;
    public float speed = 0;
    public bool crouch = false;
    public bool climb = false;
    public GameObject _player;
    public bool climbAnim = false;
    public bool moveForward =false;
    public PlayerInput _playerInput;
    public ThirdPersonController _moveScript;
    public CharacterController _chrController;
    // Start is called before the first frame update
    void Start()
    {
      
        
    }

            public void animKeyFrameClimb()
    {
             _anim.SetBool("climb",false);
            
    }



    public void stopClimb()
    {
        climbAnim = false;
        Debug.Log("move forward at end of animation");
        moveForward = true;

    }

    public void stopForwardClimb()
    {
        moveForward = false;
       _moveScript.enabled = true;
       _playerInput.enabled = true;
    }

    // public void turnOnController()
    // {
        
    //   _chrController.enabled = true;
    // }

    // Update is called once per frame
    void Update()
    {

        //_player.transform.Translate(Vector3.up * Time.deltaTime);

            
        if(climbAnim==true)
        {
                _player.transform.Translate(Vector3.up * Time.deltaTime);
        }

        if(moveForward==true)
        {
            _player.transform.Translate(Vector3.forward*Time.deltaTime);
        }

                
            



       

        if(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") )
        {
            
            
            //running
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if(speed< 1f)
                {
                    speed=speed+ 0.09f;
                    
                
                }

                if(speed<1.01f)
                {
                    _anim.SetFloat("speed",speed);
                }
                
            
            //wallk
            }else{
                if(speed<0.6f)
                {
                    
                speed = speed + 0.09f;
                }

                if(speed>=0.6f)
                {
                    
                speed = speed - 0.09f;
                }
                if(speed<1.01f)
                {
                    _anim.SetFloat("speed",speed);
                }
                

            }

            //not walking 
        }else{
            if(speed>0f)
            {
                 speed = speed - 0.09f;
            }
               
                 if(speed<1.01f)
                 {
                     _anim.SetFloat("speed",speed);
                 }
                 

        }

        if(Input.GetKeyDown("c"))
        {
            if(crouch==false)
            {
            crouch = true;
            _anim.SetBool("crouch",true);
            }else{
            crouch = false;
            _anim.SetBool("crouch",false);

            }


        }


        
    }

    public void animKeyFrameCrouch()
    {
             _anim.SetBool("crouch",false);
    }







}
