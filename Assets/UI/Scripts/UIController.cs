using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Inventory inventory;

    public GameObject MessagePanel;

    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);
    }
    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
    }
}
