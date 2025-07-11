using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float smoothTime = 0.15f;

    public Vector3 positionOffset = Vector3.zero;

    [Header("Axis Limitation")]
    public Vector2 xLimit = new Vector2(-100, 100); // X axis limitation
    public Vector2 yLimit = new Vector2(-100, 100); // Y axis limitation

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    
    private void LateUpdate()
{
    if (target == null)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            return; // Không làm gì nếu vẫn chưa có Player
        }
    }
    Vector3 targetPosition = target.position + positionOffset;
    targetPosition = new Vector3(
        Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y),
        Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y),
        -10
    );
    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
}
}