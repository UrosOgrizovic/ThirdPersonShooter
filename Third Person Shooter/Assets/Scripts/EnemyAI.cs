﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    private CharacterController _AIcontroller;
    private Transform _player;

    [SerializeField]
    public float _speed = 5f;

    private Vector3 _velocity;
    private float _gravity = 20f;

    public enum EnemyState
    {
        Idle, Chase, Attack
    }

    [SerializeField]
    public EnemyState _currentState = EnemyState.Chase;

    private HealthSystem _playerHealth;
    private float _delayAttack = 1.5f; // giving the player a chance to escape
    private float _nextAttack = -1;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;


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
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f); // smooth rotation
            
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
