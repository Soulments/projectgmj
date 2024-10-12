using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //public Inventory inventory;

    // ������ ���� �˾� ����
    public GameObject MessagePanel;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStatus;

    public void OpenMessagePanel(string text, Status status)
    {
        MessagePanel.SetActive(true);
        itemName.text = text;

        if(status.MaxHP > 0)
        {
            itemStatus.text = "HP + " + status.MaxHP.ToString();
        }
        else if(status.AttackDamage > 0)
        {
            itemStatus.text = "���� + " + status.AttackDamage.ToString();
        }
        else if(status.Defense > 0)
        {
            itemStatus.text = "��� + " + status.Defense.ToString();
        }
        else if(status.AttackSpeed > 0)
        {
            itemStatus.text = "���ݼӵ� + " + status.AttackSpeed.ToString();
        }
        else if (status.SkillPercent[0] > 0)
        {
            itemStatus.text = "��ų% + " + status.SkillPercent[0].ToString();
        }
    }
    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
    }
}
