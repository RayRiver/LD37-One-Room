using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{

    void Start()
    {
        Messenger.AddListener<BattleUnit>("CreateBody", OnCreateBody);
    }

    void OnCreateBody(BattleUnit unit)
    {
        var prefab = unit.GetBodyPrefab();

        if (prefab != null)
        {
            var go = Instantiate(prefab);
            go.transform.parent = transform;
            go.transform.position = unit.transform.position;
            go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))); // random rotation;
        }
    }
}
