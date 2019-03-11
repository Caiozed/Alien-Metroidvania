using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkullEnemyController : EnemyControllerBase
{
    // Start is called before the first frame update
    PlayerController player;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentHealth = MaxHealth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > player.transform.position.x)
        {
            _spriteRenderer.flipY = false;
        }
        else
        {
            _spriteRenderer.flipY = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (_isDead) return;
        if (other.transform.CompareTag("Player"))
        {
            if (_currentHealth > 0)
            {
                Vector3 diff =  player.transform.position - transform.position;
 
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                 transform.rotation = Quaternion.Euler(0f, 0f, rot_z-180);
               
                transform.position = Vector2.MoveTowards(transform.position, other.transform.position+ new Vector3(0,0.1f,0), Speed * Time.deltaTime);
            }
        }
    }
}
