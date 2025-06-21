using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;           // 对话框面板
    public TextMeshProUGUI dialogueText;       // 对话框文本
    public Image characterImage;               // 人物头像
    public Button continueButton;              // 继续按钮
    public List<string> dialogues;             // Inspector中填写对话内容
    public string nextSceneName;               // 结束后跳转的场景名

    private int currentIndex = 0;

    void Start()
    {
        currentIndex = 0;
        dialoguePanel.SetActive(true);
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (currentIndex < dialogues.Count)
        {
            dialogueText.text = dialogues[currentIndex];
            // 如需切换头像，可在此处添加逻辑
        }
        else
        {
            dialoguePanel.SetActive(false); // 隐藏对话框
            SceneManager.LoadScene(nextSceneName); // 跳转到下一个场景
        }
    }

    public void NextDialogue()
    {
        currentIndex++;
        ShowDialogue();
    }
}