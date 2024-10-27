using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class SkillControl : MonoBehaviour
{
    public GameObject hideImg;
    private Image hideImgFill;
    public GameObject textPros;
    private TextMeshProUGUI hideSkillTimeTexts;

    public bool isUseSkill = false;
    public float coolTime = 3;
    private float startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        hideSkillTimeTexts = textPros.GetComponent<TextMeshProUGUI>();
        hideImgFill = hideImg.GetComponent<Image>();
        hideImg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUseSkill)
        {
            StartCoroutine("SkillTimeChk");
        }
    }
    public void StartCooltime()
    {
        hideImg.SetActive(true);
        startTime = coolTime;
        isUseSkill = true;
    }
    IEnumerator SkillTimeChk()
    {
        yield return null;

        if(startTime > 0)
        {
            startTime -= Time.deltaTime;

            if (startTime < 0)
            {
                startTime = 0;
                isUseSkill = false;
                hideImg.SetActive(false);
            }
            hideSkillTimeTexts.text = startTime.ToString("00");

            float time = startTime / coolTime;
            hideImgFill.fillAmount = time;
        }
    }
}
