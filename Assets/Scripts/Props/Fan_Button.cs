using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_Button : MonoBehaviour
{
    [Header("关联风扇")]
    [SerializeField] private Fan fan; // 在Inspector中拖拽要控制的风扇

    // 只要有物体进入触发器就打开风扇
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (fan != null)
        {
            fan.StartMovingCheese();
        }
    }
}
