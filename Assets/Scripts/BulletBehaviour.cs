using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Vector2 _direction;
    public Vector2 Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value.normalized;
        }

    }

    private ObjectMovement _objectMovement;
    private AudioSource _audioSource;

    public BattleUnit Owner;

    void Awake()
    {
        _objectMovement = GetComponent<ObjectMovement>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _objectMovement.Moving = true;
        _objectMovement.MovingDirection = Direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Messenger.Broadcast<string, Vector3>("PlaySfx", "HitStatic", transform.position);
            Destroy(gameObject);
        }
        else
        {
            var com = other.GetComponent<BattleUnit>();
            if (com != null)
            {
                if (Owner.tag != other.gameObject.tag)
                {
                    com.Hurt(Owner);
                    Messenger.Broadcast<string, Vector3>("PlaySfx", "Hit", transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }

}
