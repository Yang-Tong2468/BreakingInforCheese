using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ͳ�
/// </summary>
public class Reservoir : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // --- ������Ķ����Ƿ������ǵ�Ŀ�� ---

        // 1. ����Ƿ��� Player
        bool isPlayer = other.CompareTag("Player");

        // 2. ����Ƿ����� Sticky ��
        // ����Ҫ��ǰ�� WallWalkerController �����ú� stickyLayer ����������
        // ��������������ֱ���ò㼶�������жϣ�������
        bool isStickyObject = other.gameObject.layer == LayerMask.NameToLayer("Sticky");

        // ����Ȳ��� Player Ҳ���� StickyObject�������
        if (!isPlayer && !isStickyObject)
        {
            return;
        }

        // --- �̶�������߼� ---

        // ���Դӽ���Ķ����ϻ�ȡ Rigidbody2D ���
        Rigidbody2D rbToFreeze = other.GetComponent<Rigidbody2D>();

        // ��������� Rigidbody2D ���
        if (rbToFreeze != null)
        {
            Debug.Log($"�Ͳ㲶����: {other.name}");

            // ���ģ�����������ͱ�Ϊ Static��
            // Static ���͵ĸ�����ȫ������������Ӱ�죬Ҳ���ᱻ����ʩ�ӵ������ٶ��ƶ���
            // ����λ�ñ���������������ʵ���ˡ��̶�ס����Ч����
            rbToFreeze.bodyType = RigidbodyType2D.Static;
        }
    }
}
