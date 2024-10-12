using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //public Inventory inventory;

    // 아이템 정보 팝업 변수
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
            itemStatus.text = "공격 + " + status.AttackDamage.ToString();
        }
        else if(status.Defense > 0)
        {
            itemStatus.text = "방어 + " + status.Defense.ToString();
        }
        else if(status.AttackSpeed > 0)
        {
            itemStatus.text = "공격속도 + " + status.AttackSpeed.ToString();
        }
        else if (status.SkillPercent[0] > 0)
        {
            itemStatus.text = "스킬% + " + status.SkillPercent[0].ToString();
        }
    }
    public void CloseMessagePanel()
    {
        MessagePanel.SetActive(false);
    }
}
