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

    public BattleUnit Owner;

    // Use this for initialization
    void Start()
    {
        _objectMovement = GetComponent<ObjectMovement>();
        _objectMovement.Moving = true;
        _objectMovement.MovingDirection = Direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
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
                    Destroy(gameObject);
                }
            }
        }
    }
}
