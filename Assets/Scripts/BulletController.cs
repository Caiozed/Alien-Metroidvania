using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed;
    public float Damage;
    Rigidbody2D _rb;
    CircleCollider2D _circleCollider;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _rb.AddForce(transform.right * Speed * Time.deltaTime * 30, ForceMode2D.Force);
        Destroy(gameObject, 1.2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        _rb.useFullKinematicContacts = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _circleCollider.enabled = false;
        anim.SetTrigger("Impact");
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
