using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _maxSlopeAngle;

    private Rigidbody _rb;
    private float _horizontal;
    private float _vertical;
    private Vector3 _direction;


    private const float RAY_DISTANCE = 0.1f;
    private RaycastHit _slopeHit;
    private int _groundLayer; // ground layer
    [SerializeField]
    private bool _isSlope;
    [SerializeField]
    private bool _isJump;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        _groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            _isJump = true;
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        _direction = new Vector3(_horizontal, 0f, _vertical);

        _isSlope = IsOnSlope();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _isSlope ? AdjustDirectionToSlope(_direction) : _direction;
        Vector3 gravity = !_isSlope && _isJump ? Vector3.zero : Vector3.down * Mathf.Abs(_rb.velocity.y);

        _rb.velocity = (velocity * _moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isJump = false;
    }

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out _slopeHit, RAY_DISTANCE, _groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle != 0f && angle < _maxSlopeAngle;
        }

        return false;
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }
}
