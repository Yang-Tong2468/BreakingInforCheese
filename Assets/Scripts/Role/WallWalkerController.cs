using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重力控制&&箱子粘附控制器
/// </summary>
public class WallWalkerController : MonoBehaviour
{
    // --- 所有变量声明都保持不变 ---
    [Header("组件引用")]
    public LevelController leveController;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private List<GameObject> groundContacts = new List<GameObject>();
    private Animator animator;//子物体上的动画

    [Header("状态")]
    [SerializeField]private bool isGrounded = false;
    [SerializeField]private bool canSwitchDirection;
    [SerializeField]private Vector2 fallDirection = Vector2.down;
    [SerializeField]private bool facingRight = true;

    [Header("移动参数")]
    [SerializeField] private float fallForce = 50f;
    [SerializeField] private float maxFallSpeed = 15f;
    [SerializeField] private float moveSpeedOnWall = 5f;

    [Header("粘附参数")]
    [SerializeField]private bool isAttached = false; // 是否粘附在物体上
    [SerializeField]private Rigidbody2D attachedObjectRb;
    [SerializeField] private Vector2 disengagementSpeed = new Vector2(5,5);
    [SerializeField]private Vector2 lastFallDirection;
    private Vector3 attachedObjectOffset;//和粘连物相对位移


    [Header("图层设置")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask stickyLayer;

    [Header("粘附参数")]
    private float attachmentCooldownTimer = 0f;
    [SerializeField] private float attachmentCooldown = 0.2f; // 冷却时间，0.2秒通常足够

    // --- Awake 函数保持不变 ---
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        rb.gravityScale = 0;
        animator = GetComponentInChildren<Animator>();
    }

    // --- Update 函数是主要修改点 ---
    void Update()
    {
        // 核心逻辑：只有在接触地面或粘附物体时，才允许读取WASD来改变方向
        canSwitchDirection = isGrounded || (attachedObjectRb != null);

        // 更新粘附冷却计时器
        if (attachmentCooldownTimer > 0)
        {
            attachmentCooldownTimer -= Time.deltaTime;
        }

        if (canSwitchDirection)
        {
            // 读取WASD输入，并确保一次只有一个方向
            Vector2 newFallDirection = GetDirectionFromInput();

            // 如果有新的有效方向输入，则更新 fallDirection
            if (newFallDirection != Vector2.zero && newFallDirection != fallDirection)
            {
                lastFallDirection = fallDirection; // 记录旧方向
                fallDirection = newFallDirection;
                rb.velocity = Vector2.zero;
            }
        }
    }

    // --- FixedUpdate 函数也有重要修改 ---
    void FixedUpdate()
    {
        // --- 1. 处理分离 ---
        // 这是最高优先级的操作。如果需要分离，就分离并结束本次更新。
        if (attachedObjectRb != null && fallDirection != lastFallDirection)
        {
            DetachObject();
            return; // 关键！立即退出 FixedUpdate，防止在同一帧施加坠落力。
        }

        // --- 2. 处理旋转 ---
        // 旋转总是需要执行的
        HandleRotation();

        // --- 3. 根据状态执行不同行为 ---
        if (isGrounded)
        {
            // 在地面上时，可以在墙上移动
            HandleWallMovement();
        }
        else if (isAttached)
        {
            // 粘附时，让粘附物跟随玩家（同步速度）
            // 并且，玩家本身也应该受到坠落力的影响
            HandleFallingOrAttached(); // 玩家需要力
            if (attachedObjectRb != null) attachedObjectRb.velocity = rb.velocity; // 粘附物跟随
        }
        else
        {
            // 在空中时，自由坠落
            HandleFallingOrAttached();
        }
    }

    // 新的函数，合并了坠落和粘附移动的逻辑
    private void HandleFallingOrAttached()
    {
        // 如果没有粘附物体，就是自由坠落
        if (attachedObjectRb == null)
        {
            // 施加坠落的力，但要限制最大速度
            if (rb.velocity.magnitude < maxFallSpeed)
            {
                rb.AddForce(fallDirection * fallForce);
            }
        }
    }

    // A. 处理旋转 (瞬时旋转版)

    private void HandleRotation()
    {
        // 我们的目标是让角色的“头顶”(transform.up)指向与坠落方向相反的方向。
        Vector2 targetUpDirection = -fallDirection;

        // 使用 Mathf.Atan2 直接计算目标角度
        float targetAngle = Mathf.Atan2(targetUpDirection.y, targetUpDirection.x) * Mathf.Rad2Deg;

        // 创建目标旋转 (依然需要-90度校正)
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90f);

        // 直接设置刚体的旋转，而不是平滑过渡。这会在一帧内完成旋转。
        rb.MoveRotation(targetRotation);
    }

    /// B. 处理在墙上移动 (根据极简输入控制器更新后)
    private void HandleWallMovement()
    {
        // 抵消任何残余的坠落力，让角色紧贴墙面
        rb.velocity = Vector2.zero;

        // --- 从布尔输入值生成移动输入 ---
        // 1. 将布尔输入转换为一个代表方向的浮点数 (-1, 0, 或 1)
        float moveInput = 0f;
        if (playerInput.MoveLeftPressed)
        {
            moveInput = -1f;
        }
        else if (playerInput.MoveRightPressed)
        {
            moveInput = 1f;
        }

        // 更新动画状态 
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        // 2. 如果有移动输入，则计算并应用移动
        if (moveInput != 0f)
        {
            // 计算相对于角色自己的移动方向 (transform.right 指向角色的右边)
            Vector2 moveDirection = transform.right * moveInput;

            // 计算目标位置
            Vector2 targetPosition = rb.position + moveDirection * moveSpeedOnWall * Time.fixedDeltaTime;

            // 使用 MovePosition 平滑且稳定地移动角色
            rb.MovePosition(targetPosition);
        }

        // 3. 处理角色面向翻转 (逻辑与移动输入解耦)
        // 这样做的好处是，即使角色因为某些原因没有移动，但只要玩家按下了方向键，角色就会转向
        if (moveInput < 0 && facingRight)
        {
            Flip();
        }
        else if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // --- 新增：检查冷却时间 ---
        if (attachmentCooldownTimer > 0)
        {
            return; // 如果在冷却时间内，直接忽略所有粘附尝试
        }

        Debug.Log("撞上了");
        if (isGrounded || attachedObjectRb != null || (stickyLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }
        Debug.Log("粘附成功！");

        attachedObjectRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (attachedObjectRb == null) return; // 如果对方没有刚体，则无法粘附

        isAttached = true;
        lastFallDirection = fallDirection; // 粘附时，锁定当前方向为“旧方向”

        // --- 恢复关键的粘附逻辑 ---
        // 1. 设置对方为运动学模式，这样它就完全由我们的代码控制
        //attachedObjectRb.isKinematic = true;

        // 2. 禁用两者间的碰撞，避免抖动和物理排斥
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);

        // 3. 停止玩家和粘附物的当前速度，准备一起移动
        //rb.velocity = Vector2.zero;
        //attachedObjectRb.velocity = Vector2.zero;
    }


    // F. 分离物体
    private void DetachObject()
    {
        if (attachedObjectRb == null) return;

        Debug.Log("分离物体");
        // --- 新增：启动冷却计时器 ---
        attachmentCooldownTimer = attachmentCooldown;

        isAttached = false;

        // 1. 记录下分离前的速度
        Vector2 lastPlayerVelocity = rb.velocity;
        Vector2 direction = lastPlayerVelocity.normalized;
        if (direction == Vector2.zero)
        {
            // 如果分离时速度为0, 给一个默认方向（比如与旧重力方向相反）
            direction = lastFallDirection;
        }
        attachedObjectRb.velocity = direction * disengagementSpeed.x; // 使用 disengagementSpeed 的x分量作为速度大小


        // 2. 将玩家速度清零，让玩家可以从静止开始朝新方向加速
        rb.velocity = Vector2.zero;

        // 恢复碰撞
        //Collider2D playerCollider = GetComponent<Collider2D>();
        //Collider2D attachedCollider = attachedObjectRb.GetComponent<Collider2D>();
        //if (playerCollider != null && attachedCollider != null)
        //{
        //    Physics2D.IgnoreCollision(playerCollider, attachedCollider, false);
        //}

        // 3. 让被分离的物体继承玩家分离前的速度
        //attachedObjectRb.velocity = lastPlayerVelocity;

        // 添加减速器
        if (attachedObjectRb.gameObject.GetComponent<Decelerator>() == null)
        {
            attachedObjectRb.gameObject.AddComponent<Decelerator>();
        }

        // 清除引用
        attachedObjectRb = null;
    }

    // E. 使用触发器来检测地面接触
    // 这是 GroundCheck 子物体上的 CircleCollider2D (Trigger) 调用的
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((groundLayer.value & (1 << other.gameObject.layer)) != 0 && !groundContacts.Contains(other.gameObject))
        {
            groundContacts.Add(other.gameObject);
            isGrounded = true; // 立即更新状态
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (groundContacts.Contains(other.gameObject))
        {
            groundContacts.Remove(other.gameObject);
            // 只有当不再接触任何地面时，才更新状态
            if (groundContacts.Count == 0)
            {
                isGrounded = false;
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // 辅助函数，将输入转换为方向
    private Vector2 GetDirectionFromInput()
    {
        if (playerInput.LeftPressed) return Vector2.left;
        if (playerInput.RightPressed) return Vector2.right;
        if (playerInput.UpPressed) return Vector2.up;
        if (playerInput.DownPressed) return Vector2.down;
        return Vector2.zero; // 没有输入
    }
}