using UnityEngine;

/// <summary>
/// 这个脚本的唯一职责是报告按键的当前状态
/// 它不包含任何逻辑，只提供原始的输入数据
/// </summary>
public class PlayerInput : MonoBehaviour
{
    // --- 方向坠落输入 (WASD) ---
    public bool UpPressed { get; private set; }
    public bool DownPressed { get; private set; }
    public bool LeftPressed { get; private set; }
    public bool RightPressed { get; private set; }

    // --- 墙面移动输入 (箭头键) ---
    public bool MoveLeftPressed { get; private set; }
    public bool MoveRightPressed { get; private set; }

    // Update is called once per frame
    void Update()
    {
        // 报告 WASD 按键的状态
        UpPressed = Input.GetKey(KeyCode.W);
        DownPressed = Input.GetKey(KeyCode.S);
        LeftPressed = Input.GetKey(KeyCode.A);
        RightPressed = Input.GetKey(KeyCode.D);

        // 报告左右箭头键的状态
        MoveLeftPressed = Input.GetKey(KeyCode.LeftArrow);
        MoveRightPressed = Input.GetKey(KeyCode.RightArrow);
    }
}