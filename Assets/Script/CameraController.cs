using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 position;
    private float positionX;
    [SerializeField] private Vector3 minPosition, maxPosition;

    private void Start()
    {
        positionX = transform.position.x;
    }

    private void FixedUpdate()
    {
        position = NewPlayer.Instance.transform.position;
        position.z = -10f;
        position.x = positionX;
        transform.position = Vector3.Lerp(transform.position,position, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + minPosition, transform.position + minPosition + Vector3.right * 20f);
        Gizmos.DrawLine(transform.position - maxPosition, transform.position - maxPosition + Vector3.right * 20f);
    }
}
