using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public float MaxHP;

    [HideInInspector]
    public float HP;

    [HideInInspector]
    public float Atk = 1;

    void Awake()
    {
        HP = MaxHP;
    }

    public void Hurt(BattleUnit from)
    {
        HP -= from.Atk;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }
}
