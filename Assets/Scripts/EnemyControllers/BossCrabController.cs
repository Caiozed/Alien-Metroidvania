using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BossCrabController : EnemyControllerBase
{
    // Start is called before the first frame update
    public LayerMask _raycastLayerMask;
    public Transform[] Projectiles;
    public bool Started;
    public GameObject Bullet;
    bool _isNearWallLeft, _isNearWallRight, _canMove, _deactivateParentAnim;
    float currentSpeed;
    public void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _currentHealth = MaxHealth;
        currentSpeed = Speed;
        InvokeRepeating("ToggleMovement", 0, 2);
        CameraShaker.Instance.StartShake(2, 2, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Started) { return; }

        CameraShaker.Instance.enabled = false;
        _isNearWallLeft = Physics2D.Raycast(transform.position, -Vector3.right, 1f, _raycastLayerMask);
        _isNearWallRight = Physics2D.Raycast(transform.position, Vector3.right, 1f, _raycastLayerMask);

        _rb.constraints = _canMove ? RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        Move();
        _animator.SetBool("IsMoving", _canMove);

        if (!_deactivateParentAnim)
        {
            GetComponentsInParent<Animator>()[1].enabled = false;
            _deactivateParentAnim = true;
        }

        if (_isDead) _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Move()
    {
        if (_canMove)
        {
            for (int i = 0; i < Projectiles.Length; i++)
            {
                Instantiate(Bullet, Projectiles[i].position, Projectiles[i].rotation);
            }

            if (_isNearWallLeft)
            {
                currentSpeed = Speed;
            }
            else if (_isNearWallRight)
            {
                currentSpeed = -Speed;
            }

            _spriteRenderer.flipY = currentSpeed > 0 ? true : false;
            _rb.velocity = new Vector2(currentSpeed, _rb.velocity.y);
        }
    }

    void ToggleMovement()
    {
        _canMove = !_canMove;
    }
}
