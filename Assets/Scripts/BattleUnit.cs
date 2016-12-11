using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public GameObject _bodyPrefab;
    public float _bodyFadeTime = 0;
    public string _explosionAudioName;
    public string _explosionEffectName;

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
            if (!string.IsNullOrEmpty(_explosionEffectName))
            {
                Messenger.Broadcast<string, Vector3>("PlayEffect", _explosionEffectName, transform.position);
            }

            if (!string.IsNullOrEmpty(_explosionAudioName))
            {
                Messenger.Broadcast<string, Vector3>("PlaySfx", _explosionAudioName, transform.position);
            }

            Messenger.Broadcast<BattleUnit>("CreateBody", this);
            Destroy(gameObject);
        }

    }
}
