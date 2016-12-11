using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private ObjectMovement _objectMovement;
    private AttackBehaviour _attackBehaviour;

    private float SHOOT_CD = 0.3f;

    private float _shootCD;
    private float _shootTimer;

    void Awake()
    {
        Messenger.AddListener<string, float>("SkillLevelUpFinished", OnSkillLevelUpFinished);
    }

    void Start()
    {
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _objectMovement = GetComponent<ObjectMovement>();

        Camera.main.GetComponent<Camera2D>().SetTarget(gameObject);

        _shootCD = SHOOT_CD;
        _shootTimer = _shootCD;
    }

    void Update()
    {
        _shootTimer += Time.deltaTime;

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
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_attackBehaviour != null)
            {
                if (_shootTimer > _shootCD)
                {
                    _attackBehaviour.Attack();
                    _shootTimer = 0;
                }
            }
        }
    }

    void OnSkillLevelUpFinished(string id, float effect)
    {
        if (id == "shootspeed")
        {
            var speed_multiple = 1 + effect;
            _shootCD = SHOOT_CD / speed_multiple;
        }
    }
}
