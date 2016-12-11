using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public string _id;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerLoot")
        {
            if (_id == "Gear")
            {
                Destroy(gameObject);
                Messenger.Broadcast<string, Vector3>("PlaySfx", "Gear", transform.position);
                Messenger.Broadcast("GetGear");
            }
            else if (_id == "Heart")
            {
                Destroy(gameObject);
                Messenger.Broadcast<string, Vector3>("PlaySfx", "Health", transform.position);
                Messenger.Broadcast("GetHeart");
            }
        }
    }
}
