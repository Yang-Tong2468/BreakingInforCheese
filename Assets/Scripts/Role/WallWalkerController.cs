using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������&&����ճ��������
/// </summary>
public class WallWalkerController : MonoBehaviour
{
    // --- ���б������������ֲ��� ---
    [Header("�������")]
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private List<GameObject> groundContacts = new List<GameObject>();

    [Header("״̬")]
    [SerializeField]private bool isGrounded = false;
    [SerializeField]private bool canSwitchDirection;
    [SerializeField]private Vector2 fallDirection = Vector2.down;
    [SerializeField]private bool facingRight = true;

    [Header("�ƶ�����")]
    [SerializeField] private float fallForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float moveSpeedOnWall;

    [Header("ճ������")]
    [SerializeField]private bool isAttached = false; // �Ƿ�ճ����������
    [SerializeField]private Rigidbody2D attachedObjectRb;
    [SerializeField] private Vector2 disengagementSpeed = new Vector2(5,5);
    [SerializeField]private Vector2 lastFallDirection;
    private Vector3 attachedObjectOffset;//��ճ�������λ��


    [Header("ͼ������")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask stickyLayer;

    [Header("ճ������")]
    private float attachmentCooldownTimer = 0f;
    [SerializeField] private float attachmentCooldown = 0.2f; // ��ȴʱ�䣬0.2��ͨ���㹻

    // --- Awake �������ֲ��� ---
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        rb.gravityScale = 0;
    }

    // --- Update ��������Ҫ�޸ĵ� ---
    void Update()
    {
        // �����߼���ֻ���ڽӴ������ճ������ʱ����������ȡWASD���ı䷽��
        canSwitchDirection = isGrounded || (attachedObjectRb != null);

        // ����ճ����ȴ��ʱ��
        if (attachmentCooldownTimer > 0)
        {
            attachmentCooldownTimer -= Time.deltaTime;
        }

        if (canSwitchDirection)
        {
            // ��ȡWASD���룬��ȷ��һ��ֻ��һ������
            Vector2 newFallDirection = GetDirectionFromInput();

            // ������µ���Ч�������룬����� fallDirection
            if (newFallDirection != Vector2.zero && newFallDirection != fallDirection)
            {
                lastFallDirection = fallDirection; // ��¼�ɷ���
                fallDirection = newFallDirection;
                rb.velocity = Vector2.zero;
            }
        }
    }

    // --- FixedUpdate ����Ҳ����Ҫ�޸� ---
    void FixedUpdate()
    {
        // --- 1. �������� ---
        // ����������ȼ��Ĳ����������Ҫ���룬�ͷ��벢�������θ��¡�
        if (attachedObjectRb != null && fallDirection != lastFallDirection)
        {
            DetachObject();
            return; // �ؼ��������˳� FixedUpdate����ֹ��ͬһ֡ʩ��׹������
        }

        // --- 2. ������ת ---
        // ��ת������Ҫִ�е�
        HandleRotation();

        // --- 3. ����״ִ̬�в�ͬ��Ϊ ---
        if (isGrounded)
        {
            // �ڵ�����ʱ��������ǽ���ƶ�
            HandleWallMovement();
        }
        else if (isAttached)
        {
            // ճ��ʱ����ճ���������ң�ͬ���ٶȣ�
            // ���ң���ұ���ҲӦ���ܵ�׹������Ӱ��
            HandleFallingOrAttached(); // �����Ҫ��
            if (attachedObjectRb != null) attachedObjectRb.velocity = rb.velocity; // ճ�������
        }
        else
        {
            // �ڿ���ʱ������׹��
            HandleFallingOrAttached();
        }
    }

    // �µĺ������ϲ���׹���ճ���ƶ����߼�
    private void HandleFallingOrAttached()
    {
        // ���û��ճ�����壬��������׹��
        if (attachedObjectRb == null)
        {
            // ʩ��׹���������Ҫ��������ٶ�
            if (rb.velocity.magnitude < maxFallSpeed)
            {
                rb.AddForce(fallDirection * fallForce);
            }
        }
    }

    // A. ������ת (˲ʱ��ת��)

    private void HandleRotation()
    {
        // ���ǵ�Ŀ�����ý�ɫ�ġ�ͷ����(transform.up)ָ����׹�䷽���෴�ķ���
        Vector2 targetUpDirection = -fallDirection;

        // ʹ�� Mathf.Atan2 ֱ�Ӽ���Ŀ��Ƕ�
        float targetAngle = Mathf.Atan2(targetUpDirection.y, targetUpDirection.x) * Mathf.Rad2Deg;

        // ����Ŀ����ת (��Ȼ��Ҫ-90��У��)
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90f);

        // ֱ�����ø������ת��������ƽ�����ɡ������һ֡�������ת��
        rb.MoveRotation(targetRotation);
    }

    /// B. ������ǽ���ƶ� (���ݼ���������������º�)
    private void HandleWallMovement()
    {
        // �����κβ����׹�������ý�ɫ����ǽ��
        rb.velocity = Vector2.zero;

        // --- �Ӳ�������ֵ�����ƶ����� ---
        // 1. ����������ת��Ϊһ����������ĸ����� (-1, 0, �� 1)
        float moveInput = 0f;
        if (playerInput.MoveLeftPressed)
        {
            moveInput = -1f;
        }
        else if (playerInput.MoveRightPressed)
        {
            moveInput = 1f;
        }

        // 2. ������ƶ����룬����㲢Ӧ���ƶ�
        if (moveInput != 0f)
        {
            // ��������ڽ�ɫ�Լ����ƶ����� (transform.right ָ���ɫ���ұ�)
            Vector2 moveDirection = transform.right * moveInput;

            // ����Ŀ��λ��
            Vector2 targetPosition = rb.position + moveDirection * moveSpeedOnWall * Time.fixedDeltaTime;

            // ʹ�� MovePosition ƽ�����ȶ����ƶ���ɫ
            rb.MovePosition(targetPosition);
        }

        // 3. ������ɫ����ת (�߼����ƶ��������)
        // �������ĺô��ǣ���ʹ��ɫ��ΪĳЩԭ��û���ƶ�����ֻҪ��Ұ����˷��������ɫ�ͻ�ת��
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
        // --- �����������ȴʱ�� ---
        if (attachmentCooldownTimer > 0)
        {
            return; // �������ȴʱ���ڣ�ֱ�Ӻ�������ճ������
        }

        Debug.Log("ײ����");
        if (isGrounded || attachedObjectRb != null || (stickyLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }
        Debug.Log("ճ���ɹ���");

        attachedObjectRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (attachedObjectRb == null) return; // ����Է�û�и��壬���޷�ճ��

        isAttached = true;
        lastFallDirection = fallDirection; // ճ��ʱ��������ǰ����Ϊ���ɷ���

        // --- �ָ��ؼ���ճ���߼� ---
        // 1. ���öԷ�Ϊ�˶�ѧģʽ������������ȫ�����ǵĴ������
        //attachedObjectRb.isKinematic = true;

        // 2. �������߼����ײ�����ⶶ���������ų�
        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);

        // 3. ֹͣ��Һ�ճ����ĵ�ǰ�ٶȣ�׼��һ���ƶ�
        //rb.velocity = Vector2.zero;
        //attachedObjectRb.velocity = Vector2.zero;
    }


    // F. ��������
    private void DetachObject()
    {
        if (attachedObjectRb == null) return;

        Debug.Log("��������");
        // --- ������������ȴ��ʱ�� ---
        attachmentCooldownTimer = attachmentCooldown;

        isAttached = false;

        // 1. ��¼�·���ǰ���ٶ�
        Vector2 lastPlayerVelocity = rb.velocity;
        Vector2 direction = lastPlayerVelocity.normalized;
        if (direction == Vector2.zero)
        {
            // �������ʱ�ٶ�Ϊ0, ��һ��Ĭ�Ϸ��򣨱���������������෴��
            direction = lastFallDirection;
        }
        attachedObjectRb.velocity = direction * disengagementSpeed.x; // ʹ�� disengagementSpeed ��x������Ϊ�ٶȴ�С


        // 2. ������ٶ����㣬����ҿ��ԴӾ�ֹ��ʼ���·������
        rb.velocity = Vector2.zero;

        // �ָ���ײ
        //Collider2D playerCollider = GetComponent<Collider2D>();
        //Collider2D attachedCollider = attachedObjectRb.GetComponent<Collider2D>();
        //if (playerCollider != null && attachedCollider != null)
        //{
        //    Physics2D.IgnoreCollision(playerCollider, attachedCollider, false);
        //}

        // 3. �ñ����������̳���ҷ���ǰ���ٶ�
        //attachedObjectRb.velocity = lastPlayerVelocity;

        // ���Ӽ�����
        if (attachedObjectRb.gameObject.GetComponent<Decelerator>() == null)
        {
            attachedObjectRb.gameObject.AddComponent<Decelerator>();
        }

        // �������
        attachedObjectRb = null;
    }

    // E. ʹ�ô�������������Ӵ�
    // ���� GroundCheck �������ϵ� CircleCollider2D (Trigger) ���õ�
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((groundLayer.value & (1 << other.gameObject.layer)) != 0 && !groundContacts.Contains(other.gameObject))
        {
            groundContacts.Add(other.gameObject);
            isGrounded = true; // ��������״̬
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (groundContacts.Contains(other.gameObject))
        {
            groundContacts.Remove(other.gameObject);
            // ֻ�е����ٽӴ��κε���ʱ���Ÿ���״̬
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

    // ����������������ת��Ϊ����
    private Vector2 GetDirectionFromInput()
    {
        if (playerInput.LeftPressed) return Vector2.left;
        if (playerInput.RightPressed) return Vector2.right;
        if (playerInput.UpPressed) return Vector2.up;
        if (playerInput.DownPressed) return Vector2.down;
        return Vector2.zero; // û������
    }
}