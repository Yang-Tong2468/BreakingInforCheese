using UnityEngine;

/// <summary>
/// ����ű���Ψһְ���Ǳ��水���ĵ�ǰ״̬
/// ���������κ��߼���ֻ�ṩԭʼ����������
/// </summary>
public class PlayerInput : MonoBehaviour
{
    // --- ����׹������ (WASD) ---
    public bool UpPressed { get; private set; }
    public bool DownPressed { get; private set; }
    public bool LeftPressed { get; private set; }
    public bool RightPressed { get; private set; }

    // --- ǽ���ƶ����� (��ͷ��) ---
    public bool MoveLeftPressed { get; private set; }
    public bool MoveRightPressed { get; private set; }

    // Update is called once per frame
    void Update()
    {
        // ���� WASD ������״̬
        UpPressed = Input.GetKey(KeyCode.W);
        DownPressed = Input.GetKey(KeyCode.S);
        LeftPressed = Input.GetKey(KeyCode.A);
        RightPressed = Input.GetKey(KeyCode.D);

        // �������Ҽ�ͷ����״̬
        MoveLeftPressed = Input.GetKey(KeyCode.LeftArrow);
        MoveRightPressed = Input.GetKey(KeyCode.RightArrow);
    }
}