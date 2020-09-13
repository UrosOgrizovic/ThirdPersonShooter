using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private CharacterController _AIcontroller;
    private Transform _player;

    [SerializeField]
    private float _speed = 3f;

    private Vector3 _velocity;
    private float _gravity = 20f;

    public enum EnemyState
    {
        Idle, Chase, Attack
    }

    [SerializeField]
    private EnemyState _currentState = EnemyState.Chase;

    private HealthSystem _playerHealth;
    private float _delayAttack = 1.5f; // giving the player a chance to escape
    private float _nextAttack = -1;

    void Start()
    {
        _AIcontroller = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerHealth = _player.GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Chase:
                EnemyMovement();
                break;
            case EnemyState.Attack:
                if (Time.time > _nextAttack)
                {
                    _playerHealth.Damage(10);
                    _nextAttack = Time.time + _delayAttack;
                }
                break;
            case EnemyState.Idle:
                break;
        }
    }

    public void EnemyMovement()
    {
        //Check if we are grounded
        if (_AIcontroller.isGrounded)
        {
            Vector3 direction = _player.position - transform.position; // ai will follow player
            direction.Normalize();
            direction.y = 0;
            transform.localRotation = Quaternion.LookRotation(direction); // TODO: smooth rotation
            _velocity = direction * _speed;
        }

        _velocity.y -= _gravity;
        _AIcontroller.Move(_velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _currentState = EnemyState.Attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _currentState = EnemyState.Chase;
        }
    }
}
