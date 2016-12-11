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

        if (tag == "Player")
        {
            Messenger.AddListener("GetHeart", OnGetHeart);
            Messenger.AddListener<string, float>("SkillLevelUpFinished", OnSkillLevelUpFinished);
        }
    }

    void Start()
    {     
        if (tag == "Player")
        {
            Messenger.Broadcast<float, float>("UIUpdateHealth", HP, MaxHP);
        }
    }

    void OnGetHeart()
    {
        if (tag == "Player")
        {
            HP++;
            if (HP > MaxHP)
            {
                HP = MaxHP;
            }

            Messenger.Broadcast<float, float>("UIUpdateHealth", HP, MaxHP);
        }
    }

    void OnSkillLevelUpFinished(string id, float effect)
    {
        if (tag == "Player")
        {
            if (id == "maxhp")
            {
                MaxHP += effect;
                HP = MaxHP;
                Messenger.Broadcast<float, float>("UIUpdateHealth", HP, MaxHP);
            }
        }
    }

    public GameObject GetBodyPrefab()
    {
        return _bodyPrefab;
    }

    public float GetBodyFadeTime()
    {
        return _bodyFadeTime;
    }

    public void Hurt(string ownerTag, float atk)
    {
        HP -= atk;

        if (HP <= 0)
        {
            if (!string.IsNullOrEmpty(_explosionEffectName))
            {
                Messenger.Broadcast<string, Vector3>("PlayEffect", _explosionEffectName, transform.position);
            }

            if (!string.IsNullOrEmpty(_explosionAudioName))
            {
                Messenger.Broadcast<string, Vector3>("PlaySfx", _explosionAudioName, transform.position);
                Messenger.Broadcast<float>("CameraShake", 0.05f);
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

            if (tag == "Player")
            {
                Messenger.Broadcast("PlayerDead");
            }

            Messenger.Broadcast<BattleUnit>("CreateBody", this);
            Destroy(gameObject);
        }

        if (tag == "Player")
        {
            Messenger.Broadcast<float, float>("UIUpdateHealth", HP, MaxHP);
            Messenger.Broadcast<float>("CameraShake", 0.03f);
        }
    }
}
