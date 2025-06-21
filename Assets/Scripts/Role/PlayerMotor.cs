using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))] 
public class PlayerMotor2D : MonoBehaviour
{
    [Header("�������")]
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header("�ƶ�����")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float fallMultiplier = 2.5f;   // ����ʱ������������
    [SerializeField] private float lowJumpMultiplier = 2f;    // ��ǰ�ɿ���Ծ��ʱ������������

    [Header("������")]
    [SerializeField] private Transform groundCheck; // ���ڼ�����Ŀ�����
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;  // �����ͼ��

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
        // �� Input �ű���ȡ����ֵ
        //horizontalMove = playerInput.HorizontalInput * moveSpeed;

        // �� Update �д�����Ծ���룬��Ϊ����һ�����¼�
        //if (playerInput.JumpInput && isGrounded)
        //{
        //    // ֱ������y���ٶ���ʵ����Ծ����AddForce�ָи��ɿ�
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        // �� Update �д�����Ծ���룬��Ϊ����һ�����¼�
        //if (playerInput.JumpInputDown && isGrounded)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    playerInput.UseJumpInput(); // �����Ծ�ѱ�ʹ��
        //}
    }

    void FixedUpdate()
    {
        // �� FixedUpdate �д�������������صĲ���

        // 1. ������
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. ˮƽ�ƶ�
        HandleMovement();
        HandleGravity();

        // 3. ��ɫ��ת
        HandleFlip();
    }

    private void HandleGravity()
    {
        // 1. ������� (Fall Multiplier)
        // �����ɫ�������� (y���ٶ�Ϊ��)
        if (rb.velocity.y < 0)
        {
            // ����ʩ��һ������ġ����µ�����������ǻ������������ı�����
            // (fallMultiplier - 1) ����Ϊϵͳ�Ѿ�Ӧ����1��������������ֻ��Ҫʩ�Ӷ���Ĳ��֡�
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // 2. �ɱ�߶���Ծ (Low Jump)
        // �����ɫ�������� (y���ٶ�Ϊ��) �����û�а�ס��Ծ��
        //else if (rb.velocity.y > 0 && !playerInput.JumpInputHeld)
        //{
        //    // ����ͬ��ʩ��һ����������µ���������ǰ��ֹ��Ծ���������̡�
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        //}
    }

    private void HandleMovement()
    {
        // ����Ŀ���ٶȡ�������ǰ�Ĵ�ֱ�ٶȣ���ֹ��Ծ������������
        Vector2 targetVelocity = new Vector2(horizontalMove, rb.velocity.y);
        // Ӧ���ٶ�
        rb.velocity = targetVelocity;
    }

    private void HandleFlip()
    {
        // ������뷽���뵱ǰ����һ�£���ת��ɫ
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
        // ��ת����״̬
        facingRight = !facingRight;

        // ��ת��ɫ�� localScale.x
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // (��ѡ) �ڱ༭���л��Ƶ����ⷶΧ���������
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}