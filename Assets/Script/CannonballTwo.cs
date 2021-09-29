using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballTwo : MonoBehaviour
{
    [SerializeField]private Vector2 force;
    [SerializeField]private float time;

    private Animator animator;

    private Rigidbody2D rigidbody;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 4)
        {
            StartCoroutine(Boom());
        }
    }

    private IEnumerator Boom()
    {
        animator.SetBool("State", true);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
