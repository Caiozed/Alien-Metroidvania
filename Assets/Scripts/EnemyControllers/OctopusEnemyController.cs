using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusEnemyController : EnemyControllerBase
{
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (transform.position.x > other.transform.position.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }

            if (_currentHealth > 0)
                transform.position = Vector2.MoveTowards(transform.position, other.transform.position, Speed * Time.deltaTime);
        }
    }
}
