using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class GravityController : MonoBehaviour
{
    private enum GravityDirection { Down, Left, Up, Right }

    [Header("�������")]
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header("�ƶ�����")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float rotationSpeed = 10f; // ��ɫ��ת���·�����ٶ�

    [Header("��������")]
    [SerializeField] private float gravityStrength = 9.81f; // �����Զ���������С

    [Header("������")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private GravityDirection currentGravityDirection = GravityDirection.Down;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // **�ǳ���Ҫ**: �����ֶ���������������Ҫ���� Rigidbody �Դ�������
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

    // 1. ���������л�
    private void HandleGravitySwitch()
    {
        if (playerInput.SwitchGravityDown) currentGravityDirection = GravityDirection.Down;
        else if (playerInput.SwitchGravityUp) currentGravityDirection = GravityDirection.Up;
        else if (playerInput.SwitchGravityLeft) currentGravityDirection = GravityDirection.Left;
        else if (playerInput.SwitchGravityRight) currentGravityDirection = GravityDirection.Right;

        // ���ݵ�ǰ������������ȫ������
        Physics2D.gravity = GetGravityVector() * gravityStrength;
    } 

    // 2. �����ɫ��ת
    private void HandleRotation()
    {
        // ����Ŀ����ת�Ƕ�
        float targetAngle = 0;
        switch (currentGravityDirection)
        {
            case GravityDirection.Down: targetAngle = 0; break;
            case GravityDirection.Up: targetAngle = 180; break;
            case GravityDirection.Left: targetAngle = -90; break;
            case GravityDirection.Right: targetAngle = 90; break;
        }

        // ƽ���ؽ���ɫ��ת��Ŀ��Ƕ�
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    // 3. ��������ƶ�
    private void HandleMovement()
    {
        // ��ȡ�����ƶ���������ڽ�ɫ�Լ���
        Vector2 localMoveDirection = new Vector2(playerInput.HorizontalInput, 0);

        // �������ƶ�����ת��Ϊ����ռ䷽��
        Vector2 worldMoveVelocity = transform.TransformDirection(localMoveDirection) * moveSpeed;

        // ����ֻ��Ӱ����������ֱ���ƶ����������������ϵ��ٶ�
        Vector2 gravityDirection = GetGravityVector();
        Vector2 velocityAlongGravity = Vector3.Project(rb.velocity, gravityDirection);
        Vector2 perpendicularVelocity = worldMoveVelocity;

        // �ϳ������ٶ�
        rb.velocity = perpendicularVelocity + velocityAlongGravity;
    }

    // 4. ������Ծ
    private void HandleJump()
    {
        if (playerInput.JumpInputDown && isGrounded)
        {
            // ��Ծ������Զ�������෴
            Vector2 jumpDirection = -GetGravityVector();
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            playerInput.UseJumpInput();
        }
    }

    // 5. ������
    private bool CheckGrounded()
    {
        // ��ⷽ����Զ�ǵ�ǰ���¡��ķ���
        Vector2 checkDirection = GetGravityVector();
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, checkDirection, groundCheckDistance, groundLayer);

        // (��ѡ) ���������Թ�����
        Debug.DrawRay(groundCheck.position, checkDirection * groundCheckDistance, Color.red);

        return hit.collider != null;
    }

    // ������������ȡ��ǰ��������ĵ�λ����
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