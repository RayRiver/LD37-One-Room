using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private ObjectMovement _objectMovement;
    private AttackBehaviour _attackBehaviour;

    void Start()
    {
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _objectMovement = GetComponent<ObjectMovement>();

        Camera.main.GetComponent<Camera2D>().SetTarget(gameObject);
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal < Mathf.Epsilon && horizontal > -Mathf.Epsilon && vertical < Mathf.Epsilon && vertical > -Mathf.Epsilon)
        {
            _objectMovement.Moving = false;
        }
        else
        {
            _objectMovement.Moving = true;
            _objectMovement.MovingDirection = new Vector2(horizontal, vertical);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _objectMovement.LookAt(mousePosition);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_attackBehaviour != null)
            {
                _attackBehaviour.Attack();
            }
        }
    }
}
