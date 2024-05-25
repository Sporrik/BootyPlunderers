using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ShootMinigame : MonoBehaviour
{
    public Collider2D miss, miss2, hit, hit2, bullseye, pointer;

    private int _speed = 1;

    private bool hasShot;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        pointer.attachedRigidbody.isKinematic = true;
        hasShot = false;
    }

    void Update()
    {
        if (!hasShot)
        {
            pointer.gameObject.transform.position = new Vector3(Mathf.Sin(Time.time * _speed) * 9.6f, 0f, -5f); //*5 for distance

            if (Input.GetKeyDown(KeyCode.Space))
            {
                pointer.attachedRigidbody.isKinematic = false;
                CheckCollision();
                hasShot = true;
            }
        }               
    }

    public int CheckCollision()
    {
        if (pointer.IsTouching(hit) || pointer.IsTouching(hit2))
        {
            return 15;
        }
        else if (pointer.IsTouching(bullseye))
        {
            return 30;
        }
        else
        {
            return 0;
        }
    }
}
