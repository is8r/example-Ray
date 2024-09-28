using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        Move();
        Scan();
    }

    void Move()
    {
        //カメラの向いている方向に入力に合わせて移動
        //cinemachine Framing Transposer + FreeLookの設定が必要
        var speed = 5.0f;
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        var movement = forward * input.z + right * input.x;
        transform.position += movement * Time.deltaTime * speed;

        //移動に合わせて回転
        if (movement.magnitude > 0)
        {
            var direction = movement.normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
        }
    }

    void Scan()
    {
        //進行方向にRayを飛ばす
        var offset = Vector3.up * 0.75f;
        var origin = transform.position + offset;
        var direction = transform.forward;
        var ray = new Ray(origin, direction);
        var distance = 2.0f;
        Physics.Raycast(ray, out RaycastHit hit, distance);

        if (hit.collider != null)
        {
            Debug.Log($"Hit: {hit.collider?.name}");
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
    }
}
