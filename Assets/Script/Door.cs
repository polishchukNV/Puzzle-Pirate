using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : UnityEngine.MonoBehaviour
{
 
    public int score;
    private Animator animator;

    private static Door instance;
    public static Door Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Door>();
            return instance;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        CloseDoor();
    }

    public void OpenDoor() => animator.SetBool("Door", true);
    private void CloseDoor() => animator.SetBool("Door", false);
}
