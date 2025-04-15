using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _jumpTriggerName = "onJump";
    [SerializeField] private string _airBoolName = "isOnAir";

    [Header("Physics")]
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _Jumpforce = 5.0f;

    private bool _isOnAir = false;

    [Header("Inputs")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    //////////////////////////////////////////////////////////

    [SerializeField] private KeyCode Rotate_right = KeyCode.Q;
    [SerializeField] private KeyCode Rotate_left = KeyCode.E;

    /////////////////////////////////////////////////////////
    private Vector3 _dir = Vector3.zero,_velocity;
    
    private Rigidbody _rb;

    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();

        _rb.constraints = RigidbodyConstraints.FreezeRotationX  |  RigidbodyConstraints.FreezeRotationZ;

        // _rb.constraints = RigidbodyConstraints.FreezeRotationY;

    }

    private void Update()
    {

        

        _dir.x = Input.GetAxis("Vertical");
        _animator.SetFloat(_xAxisName, _dir.x);
        _dir.z = Input.GetAxis("Horizontal");
        _animator.SetFloat(_zAxisName, _dir.z);

        if (Input.GetKeyDown(_jumpKey) && !_isOnAir)
        {
            _animator.SetTrigger(_jumpTriggerName);
            Debug.Log("esta saltando");
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (_dir.sqrMagnitude != 0.0f)
        {
            Movement(_dir);
        }
    }

    private void Jump()
    {
        _rb.AddForce (transform.up * _Jumpforce, ForceMode.Impulse);
    }

    private void Movement(Vector3 _dir)
    {
        _rb.MovePosition(transform.position + _dir.normalized * _moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.gameObject.layer}");
        Debug.Log("esta colicionando");

        if (collision.gameObject.layer == 30)
        {
            _isOnAir = false;
            _animator.SetBool(_airBoolName, _isOnAir);
        }
        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            _velocity = _rb.velocity;
           _rb.velocity = rb.velocity;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 30)
        {
            _isOnAir = true;
            _animator.SetBool(_airBoolName, _isOnAir);
        }

        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            _rb.velocity = _velocity;
        }
    }
}
