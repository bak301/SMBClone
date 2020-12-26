using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum State
{
    GROUND,
    SLIDE,
    FALL,
    WALLJUMP,
    JUMP,
    DASH,
    KNOCKBACK,
    DEAD
}

public class Player : MonoBehaviour, IActor
{
    [SerializeField] private float health;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Bullet bullet;
    [SerializeField] private FixedJoystick joystick;

    private RightSideCollider rightCollider;
    private LeftSideCollider leftCollider;
    private GroundCheck groundCheck;
    private HeadCheck headCheck;

    internal bool isLeftCollided;
    internal bool isRightCollided;
    internal bool isGrounded;
    internal bool isHeadCollided;
    internal bool isDashing;
    internal bool isInvulnerable;

    internal State state;
    private Transform player;
    private Rigidbody2D body;
    private SpriteRenderer rend;
    private float facing; // -1 : left , 1 : right


    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        isDashing = false;
        state = State.GROUND;
        facing = 1;
        player = GetComponent<Transform>();
        rend = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        leftCollider = GetComponentInChildren<LeftSideCollider>();
        rightCollider = GetComponentInChildren<RightSideCollider>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        headCheck = GetComponentInChildren<HeadCheck>();
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isRightCollided = rightCollider.isCollided;
        isLeftCollided = leftCollider.isCollided;
        isGrounded = groundCheck.isCollided;
        isHeadCollided = headCheck.isCollided;

        UpdateState();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Bullet firingBullet = SimplePool.Spawn(bullet, transform.position + new Vector3(facing * 1.5f, 0, 0), Quaternion.identity);
            firingBullet.SetBulletDirection(Vector3.right * facing);
            firingBullet.SetOwner(name);
        }

        switch (state)
        {
            case State.GROUND:
                HandleHorizontalMovement();

                if (Input.GetKeyDown("x") && isGrounded == true)
                {
                    StartCoroutine(Jump(0.3f));
                }

                if (Input.GetKeyDown("c") && isGrounded == true)
                {
                    StartCoroutine(Dash(0.3f, facing));
                }
                break;

            case State.SLIDE:
                if (isGrounded == false)
                {
                    if (Input.GetKeyDown("x"))
                    {
                        state = State.WALLJUMP;
                        StartCoroutine(WallJump(0.05f, 0.1f));
                    }
                    else
                    {
                        if ((Input.GetKey("left") && isLeftCollided)
                            || (Input.GetKey("right") && isRightCollided))
                        {
                            SlideDown();
                        }
                        else
                        {
                            MoveDown();
                            if ((Input.GetKey("left") && isRightCollided)
                                || (Input.GetKey("right") && isLeftCollided))
                            {
                                MoveHorizontalJoystick();
                                MoveHorizontalKeyboard();
                            }
                        }
                    }
                }
                else
                {
                    HandleHorizontalMovement();
                }
                break;

            case State.FALL:
                HandleHorizontalMovement();
                MoveDown();
                break;

            case State.JUMP:
                HandleHorizontalMovement();
                break;

            case State.DASH:
                if (Input.GetKeyDown("x") && isGrounded == true)
                {
                    StartCoroutine(Jump(0.3f));
                }

                if (isRightCollided || isLeftCollided)
                {
                    StopCoroutine(Dash(0.3f, facing));           
                }
                break;
            case State.KNOCKBACK:
                break;

            default:
                break;
        }
    }

    private void UpdateState()
    {
        if (isHeadCollided)
        {
            state = State.FALL;
        }

        // SLIDE => FALL
        if (state == State.SLIDE && (isRightCollided || isLeftCollided) == false)
        {
            state = State.FALL;
        }

        // FALL => SLIDE
        if ((isRightCollided || isLeftCollided)
            && state == State.FALL)
        {
            state = State.SLIDE;
        }

        //GROUND => FALL
        if (state == State.GROUND && !isGrounded)
        {
            state = State.FALL;
        }

        //FALL || SLIDE => GROUND
        if ((state == State.FALL || state == State.SLIDE) && isGrounded)
        {
            state = State.GROUND;
        }

        if (health <= 0)
        {
            OnDeath();
        }
    }

    private IEnumerator Jump(float duration)
    {
        state = State.JUMP;
        float airTimer = 0;
        while (airTimer < duration)
        {
            if (isHeadCollided)
            {
                break;
            }
            airTimer += Time.deltaTime;
            MoveUp(1);
            yield return null;
        }
        state = State.FALL;
    }

    private IEnumerator WallJump(float wallJumpDuration, float jumpDuration)
    {
        float airTimer = 0;
        float wallSide = isLeftCollided ? -1 : 1;

        while (airTimer < wallJumpDuration)
        {
            if (isHeadCollided)
            {
                break;
            }
            yield return null;
            airTimer += Time.deltaTime;
            PushBack(-1 * wallSide);
            MoveUp(1);
        }

        // add a small normal jump
        state = State.JUMP;
        airTimer = 0;
        while (airTimer < jumpDuration)
        {
            if (isHeadCollided)
            {
                break;
            }
            airTimer += Time.deltaTime;
            MoveUp(1);
            yield return null;
        }
        state = State.FALL;
    }

    private IEnumerator Dash(float dashDuration, float side)
    {
        float dashTimer = 0;
        state = State.DASH;
        while (dashTimer < dashDuration 
            && !isRightCollided 
            && !isLeftCollided)
        {
            yield return null;
            dashTimer += Time.deltaTime;
            FastMoveDirection(side);
        }

        if (isRightCollided || isLeftCollided)
        {
            state = State.SLIDE;
        } else
        {
            state = State.GROUND;
        }
    }

    private void HandleHorizontalMovement()
    {
        if (joystick.Horizontal != 0)
        {
            MoveHorizontalJoystick();
        }

        if (Input.GetKey("left") || Input.GetKey("right"))
        {
            MoveHorizontalKeyboard();
        }
    }

    private void MoveDown()
    {
        player.Translate(Vector3.down * Time.deltaTime * fallSpeed);
    }

    private void MoveUp(float scale)
    {
        player.Translate(Vector3.up * scale * Time.deltaTime * jumpSpeed);
    }

    private void PushBack(float side)
    {
        player.Translate(new Vector3(side * Time.deltaTime * jumpSpeed, 0, 0));
    }

    private void MoveHorizontalKeyboard()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            facing = Mathf.Sign(Input.GetAxis("Horizontal"));
        }

        if (isLeftCollided)
        {
            player.Translate(new Vector3((Input.GetAxis("Horizontal") > 0 ? 1 : 0) * Time.deltaTime * moveSpeed,
                                            0,
                                            0));
        }
        else if (isRightCollided)
        {
            player.Translate(new Vector3((Input.GetAxis("Horizontal") < 0 ? -1 : 0) * Time.deltaTime * moveSpeed,
                                            0,
                                            0));
        }
        else
        {
            //body.MovePosition(new Vector2(body.position.x + facing * Time.deltaTime * moveSpeed, body.position.y));
            player.Translate(new Vector3(facing * Time.deltaTime * moveSpeed, 0, 0));
        }
    }

    private void MoveHorizontalJoystick()
    {
        if (joystick.Horizontal != 0)
        {
            facing = Mathf.Sign(joystick.Horizontal * -1);
        }
        
        if (isLeftCollided)
        {
            player.Translate(new Vector3((joystick.Horizontal > 0 ? 1 : 0) * Time.deltaTime * moveSpeed,
                                            0,
                                            0));
        }
        else if (isRightCollided)
        {
            player.Translate(new Vector3((joystick.Horizontal < 0 ? -1 : 0) * Time.deltaTime * moveSpeed,
                                            0,
                                            0));
        }
        else
        {
            //body.MovePosition(new Vector2(body.position.x + facing * Time.deltaTime * moveSpeed, body.position.y));
            player.Translate(new Vector3(facing * Time.deltaTime * moveSpeed, 0, 0));
        }
    }

    private void FastMoveDirection(float side)
    {
        player.Translate(new Vector3(side * moveSpeed * 2 * Time.deltaTime, 0, 0));
    }

    private void SlideDown()
    {
        player.Translate(Vector3.down * Time.deltaTime * slideSpeed);
    }

    public void ReceiveDamage(float damage)
    {
        if (isInvulnerable)
        {
            return;
        } else
        {
            health -= damage;
            Knockback();
            StartCoroutine(ActivateInvulnerability());
        }
    }

    private void Knockback()
    {
        state = State.KNOCKBACK;
        transform.DOMoveX(transform.position.x + -1 * facing * 0.5f, 0.5f)
            .OnComplete(() => state = State.FALL);
    }

    private IEnumerator ActivateInvulnerability()
    {
        isInvulnerable = true;
        DOTween.Sequence()
            .Append(rend.DOFade(0.5f, 0.05f))
            .Append(rend.DOFade(1, 0.05f))
            .SetLoops(15);

        yield return new WaitForSeconds(1.5f);
        isInvulnerable = false;
    }

    public void OnDeath()
    {
        state = State.DEAD;
    }

    public void BasicAttack()
    {
        Bullet firingBullet = SimplePool.Spawn(
            bullet,
            transform.position,
            Quaternion.identity
        );

        firingBullet.SetBulletDirection(Vector3.right * facing);
        firingBullet.SetOwner(gameObject.name);
    }
}
