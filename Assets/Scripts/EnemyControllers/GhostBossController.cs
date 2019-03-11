using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBossController : EnemyControllerBase
{
    public Transform[] SpawnPoints;
    public Light GhostLight;
    public Transform SpawnPoint;
    public GameObject SpawnPojectile;
    CapsuleCollider2D _capsuleCollider;
    PlayerController _player;
    bool _isVisible;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _currentHealth = MaxHealth;
        InvokeRepeating("BehaviourLoop", 0, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("IsIdling", !_isDead);
        _capsuleCollider.enabled = !_isVisible;
    }

    void BehaviourLoop()
    {
        if (!_isDead)
        {
            _isVisible = !_isVisible;

            if (_isVisible)
            {
                _animator.SetTrigger("Vanish");
            }
            else
            {
                var position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
                transform.position = position;
                _animator.SetTrigger("Appear");
                Invoke("Attack", 1.5f);
            }

            if (_player.transform.position.x > transform.position.x)
            {
                _spriteRenderer.flipX = true;
                SpawnPoint.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                SpawnPoint.transform.eulerAngles = new Vector3(0, 180, 0);
                _spriteRenderer.flipX = false;
            }
        }
        else
        {
            _capsuleCollider.enabled = false;
        }
    }

    void Attack()
    {
        Instantiate(SpawnPojectile, SpawnPoint.position, SpawnPoint.rotation);
        _animator.SetTrigger("Attack");
    }

    public void ToggleSpriteRenderer()
    {
        GhostLight.enabled = !_isVisible;
        _spriteRenderer.enabled = !_isVisible;
    }
}
