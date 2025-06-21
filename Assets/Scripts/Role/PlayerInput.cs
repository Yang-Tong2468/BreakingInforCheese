using UnityEngine;

/// <summary>
/// ������ղ�������ҵ�����
/// ����ű��������κ��ƶ��߼���ֻ�ṩ��������
/// </summary>
public class PlayerInput : MonoBehaviour
{
    // �������ԣ��������ű����Զ�ȡ
    public float HorizontalInput { get; private set; }
    //public bool JumpInput { get; private set; }
    public bool JumpInputDown { get; private set; } // ����˲��
    public bool JumpInputHeld { get; private set; } // �Ƿ�ס

    // �����л����� (ʹ��WASD)
    public bool SwitchGravityUp { get; private set; }
    public bool SwitchGravityDown { get; private set; }
    public bool SwitchGravityLeft { get; private set; }
    public bool SwitchGravityRight { get; private set; }

    // Update ÿ֡���ᱻ����
    void Update()
    {
        // --- ����ˮƽ�ƶ����� ---
        // Input.GetAxisRaw �᷵�� -1, 0, �� 1����Ӧ����
        HorizontalInput = Input.GetAxisRaw("Horizontal"); // A/D, ��/�Ҽ�ͷ

        // --- ������Ծ���� ---
        // Input.GetButtonDown �ڰ��°�������һ֡���� true
        //if (Input.GetButtonDown("Jump")) // Ĭ��Ϊ�ո��
        //{
        //    JumpInput = true;
        //}
        //else
        //{
        //    JumpInput = false; // ����һ֡���ã�ȷ��ֻ��һ��
        //}
        JumpInputDown = Input.GetButtonDown("Jump");
        JumpInputHeld = Input.GetButton("Jump");

        // 3. ��ȡ�����л����� (WASD) - ʹ�� GetKeyDown ȷ��ֻ�ڰ���ʱ����һ��
        SwitchGravityUp = Input.GetKeyDown(KeyCode.W);
        SwitchGravityDown = Input.GetKeyDown(KeyCode.S);
        SwitchGravityLeft = Input.GetKeyDown(KeyCode.A);
        SwitchGravityRight = Input.GetKeyDown(KeyCode.D);
    }

    // ����Ծ��������Ҫ�ֶ�����"����"״̬���Է���һ
    // PlayerMotor ���Ե����������
    public void UseJumpInput() => JumpInputDown = false;
}