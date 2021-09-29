using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : Enemy
{
    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float jumpDelay = 0.25f;
    [SerializeField] private float second = 0.25f;
    [SerializeField] private AudioClip jumpClip;

    private Rigidbody2D rigibody;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigibody = GetComponent<Rigidbody2D>();
        StartCoroutine(JumpControll());
    }

    private void Jump()
    {
        rigibody.velocity = Vector2.zero;
        rigibody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        PlayJumpSound();
    }

    IEnumerator JumpControll()
    {
        while (true)
        {
            yield return new WaitForSeconds(second);
            Jump();
        }
    }

    private void PlayJumpSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = (Random.Range(0.8f, 1f));
            audioSource.PlayOneShot(jumpClip);
        }
    }
}
