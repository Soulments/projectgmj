using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //public Inventory inventory;

    // ������ ���� �˾� ����
    public GameObject MessagePanel;

    public Item itemStatus;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI item;

    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);

    }
    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
    }
}
