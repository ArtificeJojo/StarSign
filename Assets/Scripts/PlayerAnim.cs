using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    bool isPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isPressed", true);
            animator.Play("Walk");
        } 
        
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isPressed", true);
            animator.Play("Walk");
        } 
        
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isPressed", true);
            animator.Play("Walk");
        } 
        
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isPressed", true);
            animator.Play("Walk");
        } 
        animator.SetBool("isPressed", false);
    }
}
