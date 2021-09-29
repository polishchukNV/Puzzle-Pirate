using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioClip buttonMenu;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void MenuState(int menuState)
    {
        animator.SetInteger("Menu", menuState);
    }

    public void AudioSource()
    {
        audioSource.PlayOneShot(buttonMenu);
    }
}
