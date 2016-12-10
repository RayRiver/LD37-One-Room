using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private AttackBehaviour _attackBehaviour;
    private ObjectMovement _objectMovement;
    private GameObject _target;

    void Start()
    {
        _objectMovement = GetComponent<ObjectMovement>();
        _attackBehaviour = GetComponent<AttackBehaviour>();

        _target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(StartAI());
    }

    IEnumerator StartAI()
    {
        yield return new WaitForSeconds(1); 

        while (true)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
            if (_target)
            {
                // Facing target;
                _objectMovement.LookAt(_target.transform.position);

                if (Vector3.Distance(transform.position, _target.transform.position) >= 5)
                {
                    // Move to target;
                    _objectMovement.Moving = true;
                    _objectMovement.MovingDirection = _target.transform.position - transform.position;
                }
                else
                {
                    // dont move;
                    _objectMovement.Moving = false;

                    //  Check can shoot;
                    if (_shootTimer > _shootCD)
                    {
                        Shoot();
                        _shootTimer = 0;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private float _shootCD = 1;
    private float _shootTimer = 0;

    void Update()
    {
        _shootTimer += Time.deltaTime;
    }

    void Shoot()
    {
        if (_attackBehaviour != null)
        {
            _attackBehaviour.Attack();
        }
    }

}
