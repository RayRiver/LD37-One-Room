using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public GameObject _bodyPrefab;

    public float MaxHP;

    [HideInInspector]
    public float HP;

    [HideInInspector]
    public float Atk = 1;

    void Awake()
    {
        HP = MaxHP;
    }

    public GameObject GetBodyPrefab()
    {
        return _bodyPrefab;
    }

    public void Hurt(BattleUnit from)
    {
        HP -= from.Atk;

        if (HP <= 0)
        {
            Messenger.Broadcast<BattleUnit>("CreateBody", this);
            Destroy(gameObject);
        }

    }
}
