using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float _speed = 10;
    public Transform _renderer;

    public bool Moving { set; get; }


    private Vector2 _movingDirection;
    public Vector2 MovingDirection
    {
        get
        {
            return _movingDirection;
        }
        set
        {
            _movingDirection = value.normalized;
        }
    }

    private Controller2D _controller;
    private Vector2 _velocity;
    private BulletBehaviour _bulletBehaviour;

    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _bulletBehaviour = GetComponent<BulletBehaviour>();
    }

    void Update()
    {
        if (Moving)
        {
            _velocity.x = MovingDirection.x;
            _velocity.y = MovingDirection.y;
            _velocity.Normalize();

            Vector3 dir = _velocity * _speed * Time.deltaTime;

            if (tag == "Player")
            {
                var speed_multiple = GameManager.Instance.GetSkillEffect("movespeed");
                dir += dir * speed_multiple;
            }
            else if (tag == "Bullet")
            {
                if (_bulletBehaviour != null)
                {
                    if (_bulletBehaviour.OwnerTag == "Player")
                    {
                        dir += dir * GameManager.Instance.GetSkillEffect("power");
                    }
                }
            }

            _controller.Move(dir);
        }
    }

    public void LookAt(Vector2 targetPosition)
    {
        if (_renderer)
        {
            Vector2 v = targetPosition - new Vector2(transform.position.x, transform.position.y);
            Quaternion q = Quaternion.FromToRotation(Vector3.up, v);
            //q = Quaternion.Slerp(transform.rotation, q, 0.1f);
            _renderer.transform.rotation = q;
        }
    }
}
