using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerLoot")
        {
            Destroy(gameObject);

            Messenger.Broadcast<string, Vector3>("PlaySfx", "Gear", transform.position);

            Messenger.Broadcast("GetGear");
        }
    }
}
