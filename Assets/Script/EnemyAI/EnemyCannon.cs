using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    [SerializeField] private Transform positionCannon;
    [SerializeField] private float speedSpawnTime;
    [SerializeField] private float speedAnimTime;
    [SerializeField] private AudioClip shootAudio;

    private AudioSource audioSource;

    private Animator animator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        StartCoroutine (TimeShoot(speedSpawnTime));
        
    }

    IEnumerator TimeShoot(float second)
    {
        while (true)
        {
            animator.SetBool("Shoot", true);
            yield return new WaitForSeconds(speedAnimTime);
            audioSource.PlayOneShot(shootAudio);
            Instantiate(bomb, positionCannon);
            animator.SetBool("Shoot", false);
            yield return new WaitForSeconds(second);
        }
    }
}
