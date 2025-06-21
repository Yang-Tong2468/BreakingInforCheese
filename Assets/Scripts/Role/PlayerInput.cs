using UnityEngine;

/// <summary>
/// 负责接收并处理玩家的输入
/// 这个脚本不包含任何移动逻辑，只提供输入数据
/// </summary>
public class PlayerInput : MonoBehaviour
{
    // 公开属性，让其他脚本可以读取
    public float HorizontalInput { get; private set; }
    //public bool JumpInput { get; private set; }
    public bool JumpInputDown { get; private set; } // 按下瞬间
    public bool JumpInputHeld { get; private set; } // 是否按住

    // 重力切换输入 (使用WASD)
    public bool SwitchGravityUp { get; private set; }
    public bool SwitchGravityDown { get; private set; }
    public bool SwitchGravityLeft { get; private set; }
    public bool SwitchGravityRight { get; private set; }

    // Update 每帧都会被调用
    void Update()
    {
        // --- 处理水平移动输入 ---
        // Input.GetAxisRaw 会返回 -1, 0, 或 1，反应灵敏
        HorizontalInput = Input.GetAxisRaw("Horizontal"); // A/D, 左/右箭头

        // --- 处理跳跃输入 ---
        // Input.GetButtonDown 在按下按键的那一帧返回 true
        //if (Input.GetButtonDown("Jump")) // 默认为空格键
        //{
        //    JumpInput = true;
        //}
        //else
        //{
        //    JumpInput = false; // 在下一帧重置，确保只跳一次
        //}
        JumpInputDown = Input.GetButtonDown("Jump");
        JumpInputHeld = Input.GetButton("Jump");

        // 3. 获取重力切换输入 (WASD) - 使用 GetKeyDown 确保只在按下时触发一次
        SwitchGravityUp = Input.GetKeyDown(KeyCode.W);
        SwitchGravityDown = Input.GetKeyDown(KeyCode.S);
        SwitchGravityLeft = Input.GetKeyDown(KeyCode.A);
        SwitchGravityRight = Input.GetKeyDown(KeyCode.D);
    }

    // 在跳跃后，我们需要手动重置"按下"状态，以防万一
    // PlayerMotor 可以调用这个方法
    public void UseJumpInput() => JumpInputDown = false;
}