using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan_Button : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private Fan fan; // ��Inspector����קҪ���Ƶķ���

    // ֻҪ��������봥�����ʹ򿪷���
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (fan != null)
        {
            fan.StartMovingCheese();
        }
    }
}
