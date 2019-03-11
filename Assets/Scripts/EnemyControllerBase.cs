using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyControllerBase : MonoBehaviour
{
    public float Damage, Speed;
    public float MaxHealth;
    public UnityEvent OnDeathEvent;
    [HideInInspector]
    public float _currentHealth;
    [HideInInspector]
    public Rigidbody2D _rb;
    [HideInInspector]
    public SpriteRenderer _spriteRenderer;
    [HideInInspector]
    public Animator _animator;
    [HideInInspector]
    public bool _isDead;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0 && !_isDead)
        {
            _animator.SetTrigger("Die");
            OnDeathEvent.Invoke();
            _isDead = true;
        }
        else
            StartCoroutine("HitAnimation");
    }

    IEnumerator HitAnimation()
    {
        var originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    public void Destroy()
    {
        this.gameObject.SetActive(false);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                var player = other.transform.GetComponent<PlayerController>();
                player.TakeDamage(Damage);
                break;

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "PlayerBullet":
                var bullet = other.transform.GetComponent<BulletController>();
                TakeDamage(bullet.Damage);
                break;
        }

        switch (other.transform.tag)
        {
            case "Player":
                var player = other.transform.GetComponent<PlayerController>();
                player.TakeDamage(Damage);
                break;

        }
    }
}
