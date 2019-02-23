using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrabController : EnemyControllerBase
{
    // Start is called before the first frame update
    public LayerMask _raycastLayerMask;
    bool _isNearWallLeft, _isNearWallRight, _canMove;
    float currentSpeed;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentHealth = MaxHealth;
        currentSpeed = Speed;
        InvokeRepeating("ToggleMovement", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        _isNearWallLeft = Physics2D.Raycast(transform.position, -Vector3.right, 0.3f, _raycastLayerMask);
        _isNearWallRight = Physics2D.Raycast(transform.position, Vector3.right, 0.3f, _raycastLayerMask);

        _rb.constraints = _canMove ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezePositionX; 

        Move();
        _animator.SetBool("IsMoving", _canMove);
    }

    void Move()
    {
        if (_canMove)
        {
            if (_isNearWallLeft)
            {
                currentSpeed = Speed;
            }
            else if (_isNearWallRight)
            {
                currentSpeed = -Speed;
            }

            _spriteRenderer.flipX = currentSpeed > 0 ? true : false;
            _rb.velocity = new Vector2(currentSpeed, _rb.velocity.y);
        }
    }

    void ToggleMovement()
    {
        _canMove = !_canMove;
    }
}
