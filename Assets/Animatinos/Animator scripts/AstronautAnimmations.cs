using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AstronautAnimmations : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null) 
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isWalkingBack", false);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWalkingLeft", true);
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isWalkingBack", false);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isWalkingBack", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isWalkingRight", true);
                animator.SetBool("isWalkingBack", false);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isJumping", true);
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isWalkingBack", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isWalkingBack", false);
            }
        }
    }
}
