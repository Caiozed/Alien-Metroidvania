using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionCode2D.Renderers;
using UnityEngine.Experimental.Input;
public class PlayerController : MonoBehaviour
{
    public InputMaster Controls;
    public GameObject BulletPrefab, DeathEffect;
    public Transform BulletPoint, BulletPointUp, BulletPointDown, BulletPointWall, BulletPoints;
    public ParticleSystem ChargedJumpEffect;
    public float MaxHealth, Speed, JumpHeight, JumpTime, WallJumpTime, InvunerableBlinks;
    public Vector2 WallJumpForce;
    public LayerMask _raycastLayerMask;
    RectTransform HeathFill;
    Rigidbody2D _rb;
    Vector2 _direction;
    bool _btnJumpPressed = false, _isGrounded, _isDucking, _isLookingUp, _isNearWallLeft, _isNearWallRight, _isWallClinging, _isHovering, _isVulnerable;
    public bool HaveChargedJump, HaveWallJump;
    float currentJumptime, currentJumpHeight, currentWallJumptime, currentWallJumpHeight, _currentHealth;
    int JumpTimes = 1;
    Animator anim;
    CircleCollider2D _circleCollider;
    SpriteRenderer _spriteRenderer;
    EdgeCollider2D _edgeCollider;
    BoxCollider2D _boxCollider;
    SpriteGhostTrailRenderer ghostTrail;
    LineRenderer _lineRenderer;
    public enum PlayerPower
    {
        ChargedJump,
        WallJump
    }

    void Awake()
    {
        Controls.Player.Jump.performed += ctx => Jump();
        Controls.Player.Shoot.performed += ctx => Shoot();
        Controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInChildren<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        // _circleCollider = GetComponent<CircleCollider2D>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        ghostTrail = GetComponentInChildren<SpriteGhostTrailRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        HeathFill = GameObject.FindWithTag("HealthFill").GetComponent<RectTransform>();
        _currentHealth = MaxHealth;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        var x = _direction.x * Speed * Time.deltaTime * 60;
        var y = currentJumpHeight * Time.deltaTime * 60;

        //Draw charged jump direction line
        // if (!_isHovering)
        // {
        _rb.AddForce(new Vector2(x, y), ForceMode2D.Force);
        //     _lineRenderer.SetPosition(1, Vector2.zero);
        // }
        // else
        // {
        //     _lineRenderer.SetPosition(1, _direction / 2);
        // }

        JumpUpdate();
        WallJumpUpdate();

        _isGrounded = Physics2D.Raycast(transform.position, -Vector3.up, 0.05f, _raycastLayerMask);

        if (HaveWallJump)
        {
            _isNearWallLeft = Physics2D.Raycast(transform.position + new Vector3(0, 0.11f, 0), -Vector3.right, 0.13f, _raycastLayerMask);
            _isNearWallRight = Physics2D.Raycast(transform.position + new Vector3(0, 0.11f, 0), Vector3.right, 0.13f, _raycastLayerMask);
        }

        if (_isGrounded) JumpTimes = 1;

        HandleMoveAnimation();
        HandleJumpAnimation();
        HandleWallAnimation();
    }

    void Jump()
    {
        _btnJumpPressed = !_btnJumpPressed;

        //Charged jump
        if (_btnJumpPressed && JumpTimes > 0 && !_isGrounded && HaveChargedJump && !_isWallClinging)
        {
            JumpTimes = 0;
            // _rb.AddForce(new Vector2(_direction.x, _direction.y/2), ForceMode2D.Impulse);
            currentJumptime = JumpTime;
            currentJumpHeight = JumpHeight;
        }
        //Normal jump
        else if (_isGrounded && _btnJumpPressed)
        {
            anim.SetTrigger("Jump");
            currentJumptime = JumpTime;
            currentJumpHeight = JumpHeight;
        };

        //WallJump
        if (!_isGrounded && _isWallClinging)
        {
            currentWallJumptime = WallJumpTime;
            currentWallJumpHeight = -_direction.normalized.x * WallJumpForce.x * Time.deltaTime * 60;
        }
    }

    void JumpUpdate()
    {
        //NormalJump
        if (currentJumptime > 0 && _btnJumpPressed)
        {
            currentJumptime -= Time.deltaTime;
        }
        else
        {
            // //Charged jump
            // if (_btnJumpPressed && JumpTimes > 0 && !_isWallClinging && _rb.velocity.y <= 0 && HaveChargedJump)
            // {
            //     // _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //     if (!ChargedJumpEffect.isPlaying)
            //         ChargedJumpEffect.Play();
            //     // _isHovering = true;
            // }
            // else
            // {
            //     // _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //     ChargedJumpEffect.Stop();
            //     // _isHovering = false;
            // }
            currentJumpHeight = 0;
        }
    }

    void WallJumpUpdate()
    {
        //WallJump
        if (currentWallJumptime > 0 && _isWallClinging && _btnJumpPressed)
        {
            currentWallJumptime -= Time.deltaTime;
            var y2 = WallJumpForce.y * Time.deltaTime * 60;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            Debug.Log(currentWallJumpHeight);
            _rb.AddForce(new Vector2(currentWallJumpHeight, y2), ForceMode2D.Force);
        }
        else
        {
            currentWallJumptime = 0;
        }
    }

    void Move(Vector2 direction)
    {
        _isDucking = direction.y < 0 ? true : false;
        _isLookingUp = direction.y > 0 ? true : false;
        anim.SetBool("isDucking", _isDucking);
        anim.SetBool("isLookingUp", _isLookingUp);

        if (!_isDucking && !_isLookingUp)
        {
            _direction = new Vector2(direction.x, direction.y);
            //Change facing direction
            if (_direction.x != 0)
            {
                var lookDirection = _direction.x > 0 ? 0 : 180;
                var flipX = _direction.x > 0 ? false : true;
                _spriteRenderer.flipX = flipX;
                BulletPoints.eulerAngles = new Vector2(0, lookDirection);
            }
        }
        else
        {
            //ChargedJump
            if (_isHovering)
                _direction = new Vector2(direction.x, direction.y);
            else
                _direction = new Vector2(0, direction.y);
        }
    }

    void Shoot()
    {
        if (_isHovering || (!_isGrounded && !_isWallClinging)) return;
        if (_isLookingUp)
        {
            Instantiate(BulletPrefab, BulletPointUp.position, BulletPointUp.transform.rotation);
        }
        else if (_isDucking)
        {
            Instantiate(BulletPrefab, BulletPointDown.position, BulletPointDown.transform.rotation);
        }
        else if (_isWallClinging)
        {
            Instantiate(BulletPrefab, BulletPointWall.position, BulletPointWall.transform.rotation);
        }
        else
        {
            Instantiate(BulletPrefab, BulletPoint.position, BulletPoint.transform.rotation);
        }
    }

    public void EnablePower(PlayerPower powerToEnable)
    {
        switch (powerToEnable)
        {
            case PlayerPower.ChargedJump:
                HaveChargedJump = true;
                break;
            case PlayerPower.WallJump:
                HaveWallJump = true;
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_isVulnerable)
        {
            _currentHealth -= damage;
            UpdateHealth();
            _isVulnerable = true;
            gameObject.layer = 11;
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
                Instantiate(DeathEffect, anim.transform.position, transform.rotation);
            }
            else
            {
                anim.SetTrigger("Hit");
                StartCoroutine("HitAnimation");
            }
        }
    }

    IEnumerator HitAnimation()
    {
        for (var i = 0; i < InvunerableBlinks; i++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, i % 2);
            yield return new WaitForSeconds(.2f);
        }
        _isVulnerable = false;
        gameObject.layer = 2;
    }

    void UpdateHealth()
    {
        HeathFill.localScale = new Vector2(_currentHealth / MaxHealth, HeathFill.localScale.y);
    }

    void HandleMoveAnimation()
    {
        var isRunning = _direction.x != 0 && !_isDucking ? true : false;
        anim.SetBool("isRunning", isRunning);
    }

    void HandleJumpAnimation()
    {
        var isOnAir = _isGrounded ? false : true;
        // if (isOnAir)
        // {
        //     _circleCollider.enabled = true;
        //     _edgeCollider.enabled = false;
        //     _boxCollider.enabled = false;
        // }
        // else
        // {
        //     _circleCollider.enabled = false;
        //     _edgeCollider.enabled = true;
        //     _boxCollider.enabled = true;
        // }
        anim.SetBool("isOnAir", isOnAir);
    }

    void HandleWallAnimation()
    {
        if (!_isGrounded && _isNearWallLeft && _direction.x < 0)
        {
            JumpTimes = 1;
            _isWallClinging = true;
        }
        else if (!_isGrounded && _isNearWallRight && _direction.x > 0)
        {
            JumpTimes = 1;
            _isWallClinging = true;
        }
        else
        {
            _isWallClinging = false;
        }

        //Mirror sprite
        var mirror = _isWallClinging ? -1 : 1;
        anim.transform.localScale = new Vector3(mirror, 1, 1);
        anim.SetBool("isWallClinging", _isWallClinging);
    }

    void OnEnable()
    {
        Controls.Enable();
    }

    void OnDisable()
    {
        Controls.Disable();
    }
}
