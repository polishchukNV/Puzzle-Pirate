using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private float distance = 10f;
    public float direction;


    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,transform.right, direction*distance,objectLayer);
     
        if(hitInfo != null)
        {
             Debug.DrawLine(transform.position, hitInfo.point,Color.red);
            if (hitInfo.collider.CompareTag("Player"))
            {
                NewPlayer.Instance.TakeDamege(1);
            }   
        }
        else
        {
            Debug.DrawLine(transform.position, transform.right * distance * direction, Color.green);
        }
    }
    
}
