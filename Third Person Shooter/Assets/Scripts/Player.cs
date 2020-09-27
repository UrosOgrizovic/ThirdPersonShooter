using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController _controller;

    [SerializeField]
    float _speed = 6.0f;
    [SerializeField]
    float _jumpHeight = 8.0f;
    [SerializeField]
    float _gravity = 20.0f;
    [SerializeField]
    float _camSensitivity = 1.5f;
    [SerializeField]
    EnemyAI _enemy;

    private Vector3 _direction;

    private Camera _mainCamera;

    public bool bulletTime;
    Light _light;
    Text _bulletTimeText;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();

        _bulletTimeText = GameObject.FindGameObjectWithTag("BulletTimeText").GetComponent<Text>();
        _light = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();
    }

    void Update()
    {
        Movement();
        CameraControls();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(1))
        {
            bulletTime = !bulletTime;
            float speedToSet = 5f;
            if (bulletTime)
            {
                speedToSet = 1f;
                _light.color = Color.green;
                _bulletTimeText.text = "Bullet time";
            }
            else
            {
                _light.color = Color.white;
                _bulletTimeText.text = "";
            }
            if (_enemy != null)
            {
                _enemy._speed = speedToSet;
            }
            Debug.Log("Speed = " + _speed);
            
        }
        
    }

    void Movement()
    {
        if (_controller.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _direction = new Vector3(horizontal, 0.0f, vertical);
            _direction *= _speed;

            if (Input.GetButton("Jump"))
            {
                _direction.y = _jumpHeight;
            }
        }
        _direction.y -= _gravity * Time.deltaTime;

        _direction = transform.TransformDirection(_direction);

        _controller.Move(_direction * Time.deltaTime);
    }

    void CameraControls()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Looking left & right
        Vector3 rotationY = transform.localEulerAngles;
        rotationY.y += mouseX * _camSensitivity;
        transform.localRotation = Quaternion.AngleAxis(rotationY.y, Vector3.up);

        // Looking up & down
        Vector3 rotationX = _mainCamera.gameObject.transform.localEulerAngles;
        rotationX.x -= mouseY * _camSensitivity;
        // so as not to aim to high, but minus values are not accepted for min, so commented out
        //rotationX.x = Mathf.Clamp(rotationX.x, 0, 39);
        _mainCamera.gameObject.transform.localRotation = Quaternion.AngleAxis(rotationX.x, Vector3.right);
    }
}
