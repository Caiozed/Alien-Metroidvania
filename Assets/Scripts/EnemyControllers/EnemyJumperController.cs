using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumperController : EnemyControllerBase
{
    // Start is called before the first frame update
    public float JumpHeight;
    public LayerMask _raycastLayerMask;
    bool _isGrounded;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentHealth = MaxHealth;
        InvokeRepeating("BehaviourLoop", 2, 3);
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, 0.2f, _raycastLayerMask);
        _animator.SetBool("IsMoving", !_isGrounded);

        if(_isDead) _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void BehaviourLoop()
    {
        if (_isGrounded)
        {
            _rb.velocity = (new Vector2(Mathf.Round(Random.Range(-Speed, Speed)), JumpHeight));
        }
    }
}
