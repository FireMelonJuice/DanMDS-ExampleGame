using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerAction : int
{
    MoveLeft = 1 << 0,
    MoveRight = 1 << 1,
    Jump = 1 << 2,
    JumpDown = 1 << 3,
    Attack = 1 << 4,
}

public class Player : MonoBehaviour
{
    public float gravity;
    public float speed;
    public float jumpPower;
    public float offset;
    public Vector2 AttackSize;

    private float movement = 0;

    private bool left = false;
    private bool right = false;
    private bool down = false;
    private bool jump = false;
    private int attack = 0;

    private bool jumpdown = false;
    private bool attackDelay = false;


    private Vector2 velocity;

    private Rigidbody2D rigidbody2d;
    private BoxCollider2D collider2d;
    private Transform transformComponent;
    private Logging logger;
    private bool ground;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
        transformComponent = GetComponent<Transform>();
        logger = GetComponent<Logging>();
        logger.InitLogging();
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask mask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster"));

        if (left && right)
        {
        }
        else if (left)
        {
            velocity.x = -speed;
        }
        else if (right)
        {
            velocity.x = speed;
        }

        if (down && jump && ground && !jumpdown)
        {
            jumpdown = true;
            Invoke("JumpDownEnd", 0.05f);
        }
        else if (jump && ground)
        {
            velocity.y = jumpPower;
        }

        if (jumpdown)
        {
            mask &= ~(1 << LayerMask.NameToLayer("Floor"));
        }

        velocity.y -= gravity * Time.deltaTime;

        float minX = collider2d.bounds.min.x;
        float maxX = collider2d.bounds.max.x;
        float minY = collider2d.bounds.min.y;
        float maxY = collider2d.bounds.max.y;

        if (velocity.x < 0)
        {
            float rayDistance = Mathf.Abs(velocity.x) * Time.deltaTime + offset;

            Vector2 lb = new Vector2(minX, minY) + new Vector2(offset, offset);
            Vector2 lt = new Vector2(minX, maxY) + new Vector2(offset, -offset);

            float distance = rayDistance;
            RaycastHit2D hit = Physics2D.Raycast(lb, Vector2.left, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            hit = Physics2D.Raycast(lt, Vector2.left, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            transformComponent.position -= new Vector3(distance - offset, 0);
        }
        else
        {
            float rayDistance = Mathf.Abs(velocity.x) * Time.deltaTime + offset;

            Vector2 rb = new Vector2(maxX, minY) + new Vector2(-offset, offset);
            Vector2 rt = new Vector2(maxX, maxY) + new Vector2(-offset, -offset);

            float distance = rayDistance;
            RaycastHit2D hit = Physics2D.Raycast(rb, Vector2.right, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            hit = Physics2D.Raycast(rt, Vector2.right, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            transformComponent.position += new Vector3(distance - offset, 0);
        }

        if (velocity.y < 0)
        {
            float rayDistance = Mathf.Abs(velocity.y) * Time.deltaTime + offset;

            Vector2 lb = new Vector2(minX, minY) + new Vector2(offset, offset);
            Vector2 rb = new Vector2(maxX, minY) + new Vector2(-offset, offset);

            float distance = rayDistance;
            RaycastHit2D hit = Physics2D.Raycast(lb, Vector2.down, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            hit = Physics2D.Raycast(rb, Vector2.down, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            transformComponent.position -= new Vector3(0, distance - offset);

            ground = distance != rayDistance;
        }
        else
        {
            float rayDistance = Mathf.Abs(velocity.y) * Time.deltaTime + offset;

            Vector2 lt = new Vector2(minX, maxY) + new Vector2(offset, -offset);
            Vector2 rt = new Vector2(maxX, maxY) + new Vector2(-offset, -offset);

            float distance = rayDistance;

            RaycastHit2D hit = Physics2D.Raycast(lt, Vector2.up, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            hit = Physics2D.Raycast(rt, Vector2.up, rayDistance, mask);
            if (hit)
            {
                if (hit.distance < distance)
                {
                    distance = hit.distance;
                }
            }

            transformComponent.position += new Vector3(0, distance - offset);
            ground = false;
        }

        if (ground)
        {
            velocity.y = 0;
        }
        velocity.x = 0;

        if (attack != 0 && !attackDelay)
        {
            attackDelay = true;
            Invoke("AttackDelayEnd", 0.3f);

            Collider2D[] hits = Physics2D.OverlapBoxAll(transformComponent.position, AttackSize, 0, 1 << LayerMask.NameToLayer("Monster"));
            if (hits.Length != 0)
            {
                Debug.Log("hit");
                foreach(var hit in hits)
                {
                    Debug.Log(hit);
                    hit.GetComponent<Monster>().Kill();
                }
            }
            Debug.DrawLine(transformComponent.position - (Vector3)AttackSize / 2, transformComponent.position + (Vector3)AttackSize / 2, Color.red, 1);
        }
    }

    public void CommandPlayer(int next)
    {
        Debug.Log(next);
        if ((next & (int)PlayerAction.MoveLeft) != 0)
        {
            movement = -1;
        }
        else if ((next & (int)PlayerAction.MoveRight) != 0)
        {
            movement = 1;
        }
        else
        {
            movement = 0;
        }

        if ((next & (int)PlayerAction.JumpDown) != 0)
        {

        }
        else if ((next & (int)PlayerAction.Jump) != 0)
        {
            jump = true;
        }

        if ((next & (int)PlayerAction.Attack) != 0)
        {
            attack = 1;
        }
    }

    public void SetLeft(bool value)
    {
        left = value;
        if (logger)
        {
            if (value)
            {
                logger.LogPlayer("PUSH_Move_Left", transformComponent.position);
            }
            else
            {
                logger.LogPlayer("RELEASE_Move_Left", transformComponent.position);
            }
        }
    }
    public void SetRight(bool value)
    {
        right = value;
        if (logger)
        {
            if (value)
            {
                logger.LogPlayer("PUSH_Move_Right", transformComponent.position);
            }
            else
            {
                logger.LogPlayer("RELEASE_Move_Right", transformComponent.position);
            }
        }
    }
    public void SetDown(bool value)
    {
        down = value;
        if (logger)
        {
            if (value)
            {
                logger.LogPlayer("PUSH_Down", transformComponent.position);
            }
            else
            {
                logger.LogPlayer("RELEASE_Down", transformComponent.position);
            }
        }
    }
    public void SetJump(bool value)
    {
        jump = value;
        if (logger)
        {
            if (value)
            {
                logger.LogPlayer("PUSH_Jump", transformComponent.position);
            }
            else
            {
                logger.LogPlayer("RELEASE_Jump", transformComponent.position);
            }
        }
    }
    public void SetAttack(bool value)
    {
        attack = value ? 1 : 0;
        if (logger)
        {
            if (value)
            {
                logger.LogPlayer("PUSH_Attack", transformComponent.position);
            }
            else
            {
                logger.LogPlayer("RELEASE_Attack", transformComponent.position);
            }
        }
    }

    public void Left(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetLeft(true);
        }
        if (context.canceled)
        {
            SetLeft(false);
        }
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetRight(true);
        }
        if (context.canceled)
        {
            SetRight(false);
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetDown(true);
        }
        if (context.canceled)
        {
            SetDown(false);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetJump(true);
        }
        if (context.canceled)
        {
            SetJump(false);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetAttack(true);
        }
        if (context.canceled)
        {
            SetAttack(false);
        }
    }
    public void JumpDownEnd()
    {
        jumpdown = false;
    }

    public void AttackDelayEnd()
    {
        attackDelay = false;
    }
}
