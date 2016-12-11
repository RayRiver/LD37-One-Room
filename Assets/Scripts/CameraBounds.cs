using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    void Awake()
    {
        var collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Camera.main.GetComponent<Camera2D>().SetBounds(collider.bounds);
        }
    }
}
