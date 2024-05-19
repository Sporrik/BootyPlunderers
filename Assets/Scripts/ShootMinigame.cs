using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMinigame : MonoBehaviour
{
    public Collider2D miss, hit, bullseye, pointer;

    void Update()
    {
        pointer.gameObject.transform.position = new Vector3 (Mathf.Sin(Time.time) * 5, 4f, -3f);
        
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (pointer.IsTouching(miss))
        {
            Debug.Log("Miss!");
        }
        else if (pointer.IsTouching(hit))
        {
            Debug.Log("Hit!");
        }
        else if (pointer.IsTouching(bullseye))
        {
            Debug.Log("Bullseye!");
        }
    }
}
