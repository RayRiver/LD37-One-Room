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

    private bool _hurt = false;
    private Color _hurtColor;
    private float _hurtRecoverTime = 2f;
    private float _hurtRecoverTimer = 0;
    void Update()
    {
        if (_hurt)
        {
            _hurtRecoverTimer += Time.deltaTime;
            if (_hurtRecoverTimer >= _hurtRecoverTime)
            {
                _hurt = false;
                _hurtRecoverTimer = 0;
            }
            else
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                if (renderers != null)
                {
                    foreach (var renderer in renderers)
                    {
                        var color = Color.Lerp(renderer.material.color, Color.white, Mathf.Clamp01(_hurtRecoverTimer / _hurtRecoverTime));
                        renderer.material.color = color;
                    }
                }
            }
        }
    }

    public void Hurt(string ownerTag, float atk)
    {
        HP -= atk;

        if (tag == "Player")
        {
            _hurtColor = Color.red;
        }
        else if (tag == "Enemy")
        {
            _hurtColor = Color.red;
        }
        else
        {
            _hurtColor = Color.red;
        }

        // hurt color;
        _hurt = true;
        _hurtRecoverTimer = 0;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.color = _hurtColor;
            }
        }

        if (HP <= 0)
        {
            if (!string.IsNullOrEmpty(_explosionEffectName))
            {
                Messenger.Broadcast<string, Vector3>("PlayEffect", _explosionEffectName, transform.position);
            }

            if (!string.IsNullOrEmpty(_explosionAudioName))
            {
                Messenger.Broadcast<string, Vector3>("PlaySfx", _explosionAudioName, transform.position);
                Messenger.Broadcast<float, float>("CameraShake", 0.04f, 0.2f);
            }

            // create drop item;
            if (_dropItems != null)
            {
                foreach (var item in _dropItems)
                {
                    float n = Random.Range(0.0f, 1.0f);
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
            Messenger.Broadcast<float, float>("CameraShake", 0.07f, 0.25f);
        }
    }
}
