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

    [System.Serializable]
    public struct DropItem
    {
        public string id;
        [Range(0, 1)]
        public float rate;
    }
    public DropItem[] _dropItems;

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

            // create drop item;
            if (_dropItems != null)
            {
                float n = Random.Range(0, 1);
                foreach (var item in _dropItems)
                {
                    if (n < item.rate)
                    {
                        Messenger.Broadcast<string, Vector3>("DropItem", item.id, transform.position);
                        break;
                    }
                }
            }

            Messenger.Broadcast<BattleUnit>("CreateBody", this);
            Destroy(gameObject);
        }

    }
}
