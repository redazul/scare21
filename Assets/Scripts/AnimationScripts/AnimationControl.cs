using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Convenience class for managing player animations. Should be on the same GameObject as the Animator
/// </summary>
public class AnimationControl : MonoBehaviour
{
    public enum PlAnimTrigger
    {

    }

    public enum PlAnimBool
    {
        crouch
    }

    public enum PlAnimFloat
    {
        speed, Blend
    }


    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogWarning("could not find the animator on this gameobject");
        }
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        SetFloat(PlAnimFloat.speed, Mathf.InverseLerp(0, 2.5f ,movementSpeed));
    }

    private void SetTrigger(PlAnimTrigger trigger)
    {
        animator.SetTrigger(trigger.ToString());
    }

    private void SetBoolTrue(PlAnimBool animBool)
    {
        SetBool(animBool, true);
    }

    private void SetBoolFalse(PlAnimBool animBool)
    {
        SetBool(animBool, false);
    }

    private void SetBool(PlAnimBool animBool, bool active)
    {
        animator.SetBool(animBool.ToString(), active);
    }

    private void SetBoolOnlyTrue(PlAnimBool animBool)
    {
        for(int i = 0; i < System.Enum.GetNames(typeof(PlAnimBool)).Length; i++)
        {
            if((PlAnimBool) i == animBool)
            {
                animator.SetBool(animBool.ToString(), true);
            }
            else
            {
                animator.SetBool(animBool.ToString(), false);
            }
        }
    }

    private void SetFloat(PlAnimFloat animFloat, float value)
    {
        animator.SetFloat(animFloat.ToString(), value);
    }
}
