using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class GravityController : MonoBehaviour
{
    private enum GravityDirection { Down, Left, Up, Right }

    [Header("组件引用")]
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float rotationSpeed = 10f; // 角色旋转到新方向的速度

    [Header("重力参数")]
    [SerializeField] private float gravityStrength = 9.81f; // 可以自定义重力大小

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private GravityDirection currentGravityDirection = GravityDirection.Down;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // **非常重要**: 我们手动控制重力，所以要禁用 Rigidbody 自带的重力
        rb.gravityScale = 0;
    }

    void Update()
    {
        HandleGravitySwitch();
        HandleJump();
    }

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        HandleRotation();
        HandleMovement();
    }

    // 1. 处理重力切换
    private void HandleGravitySwitch()
    {
        if (playerInput.SwitchGravityDown) currentGravityDirection = GravityDirection.Down;
        else if (playerInput.SwitchGravityUp) currentGravityDirection = GravityDirection.Up;
        else if (playerInput.SwitchGravityLeft) currentGravityDirection = GravityDirection.Left;
        else if (playerInput.SwitchGravityRight) currentGravityDirection = GravityDirection.Right;

        // 根据当前重力方向，设置全局重力
        Physics2D.gravity = GetGravityVector() * gravityStrength;
    } 

    // 2. 处理角色旋转
    private void HandleRotation()
    {
        // 计算目标旋转角度
        float targetAngle = 0;
        switch (currentGravityDirection)
        {
            case GravityDirection.Down: targetAngle = 0; break;
            case GravityDirection.Up: targetAngle = 180; break;
            case GravityDirection.Left: targetAngle = -90; break;
            case GravityDirection.Right: targetAngle = 90; break;
        }

        // 平滑地将角色旋转到目标角度
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    // 3. 处理相对移动
    private void HandleMovement()
    {
        // 获取本地移动方向（相对于角色自己）
        Vector2 localMoveDirection = new Vector2(playerInput.HorizontalInput, 0);

        // 将本地移动方向转换为世界空间方向
        Vector2 worldMoveVelocity = transform.TransformDirection(localMoveDirection) * moveSpeed;

        // 我们只想影响与重力垂直的移动，保留重力方向上的速度
        Vector2 gravityDirection = GetGravityVector();
        Vector2 velocityAlongGravity = Vector3.Project(rb.velocity, gravityDirection);
        Vector2 perpendicularVelocity = worldMoveVelocity;

        // 合成最终速度
        rb.velocity = perpendicularVelocity + velocityAlongGravity;
    }

    // 4. 处理跳跃
    private void HandleJump()
    {
        if (playerInput.JumpInputDown && isGrounded)
        {
            // 跳跃方向永远与重力相反
            Vector2 jumpDirection = -GetGravityVector();
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            playerInput.UseJumpInput();
        }
    }

    // 5. 地面检测
    private bool CheckGrounded()
    {
        // 检测方向永远是当前“下”的方向
        Vector2 checkDirection = GetGravityVector();
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, checkDirection, groundCheckDistance, groundLayer);

        // (可选) 绘制射线以供调试
        Debug.DrawRay(groundCheck.position, checkDirection * groundCheckDistance, Color.red);

        return hit.collider != null;
    }

    // 辅助函数：获取当前重力方向的单位向量
    private Vector2 GetGravityVector()
    {
        switch (currentGravityDirection)
        {
            case GravityDirection.Down: return Vector2.down;
            case GravityDirection.Up: return Vector2.up;
            case GravityDirection.Left: return Vector2.left;
            case GravityDirection.Right: return Vector2.right;
            default: return Vector2.down;
        }
    }
}