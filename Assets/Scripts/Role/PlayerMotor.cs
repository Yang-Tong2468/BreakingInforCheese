using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))] 
public class PlayerMotor2D : MonoBehaviour
{
    [Header("组件引用")]
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float fallMultiplier = 2.5f;   // 下落时的重力倍增器
    [SerializeField] private float lowJumpMultiplier = 2f;    // 提前松开跳跃键时的重力倍增器

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck; // 用于检测地面的空物体
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;  // 地面的图层

    private bool isGrounded;
    private float horizontalMove = 0f;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // 从 Input 脚本获取输入值
        //horizontalMove = playerInput.HorizontalInput * moveSpeed;

        // 在 Update 中处理跳跃输入，因为它是一次性事件
        //if (playerInput.JumpInput && isGrounded)
        //{
        //    // 直接设置y轴速度来实现跳跃，比AddForce手感更可控
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        // 在 Update 中处理跳跃输入，因为它是一次性事件
        //if (playerInput.JumpInputDown && isGrounded)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    playerInput.UseJumpInput(); // 标记跳跃已被使用
        //}
    }

    void FixedUpdate()
    {
        // 在 FixedUpdate 中处理所有物理相关的操作

        // 1. 地面检测
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. 水平移动
        HandleMovement();
        HandleGravity();

        // 3. 角色翻转
        HandleFlip();
    }

    private void HandleGravity()
    {
        // 1. 下落加速 (Fall Multiplier)
        // 如果角色正在下落 (y轴速度为负)
        if (rb.velocity.y < 0)
        {
            // 我们施加一个额外的、向下的力。这个力是基于现有重力的倍数。
            // (fallMultiplier - 1) 是因为系统已经应用了1倍的重力，我们只需要施加额外的部分。
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 2. 可变高度跳跃 (Low Jump)
        // 如果角色正在上升 (y轴速度为正) 且玩家没有按住跳跃键
        //else if (rb.velocity.y > 0 && !playerInput.JumpInputHeld)
        //{
        //    // 我们同样施加一个额外的向下的力，来提前终止跳跃的上升过程。
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        //}
    }

    private void HandleMovement()
    {
        // 创建目标速度。保留当前的垂直速度，防止跳跃和重力被覆盖
        Vector2 targetVelocity = new Vector2(horizontalMove, rb.velocity.y);
        // 应用速度
        rb.velocity = targetVelocity;
    }

    private void HandleFlip()
    {
        // 如果输入方向与当前朝向不一致，则翻转角色
        if (horizontalMove > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalMove < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // 翻转朝向状态
        facingRight = !facingRight;

        // 翻转角色的 localScale.x
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // (可选) 在编辑器中绘制地面检测范围，方便调试
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}