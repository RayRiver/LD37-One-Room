using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public GameObject _bodyPrefab;
    public float _bodyFadeTime = 0;

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

    public float GetBodyFadeTime()
    {
        return _bodyFadeTime;
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
