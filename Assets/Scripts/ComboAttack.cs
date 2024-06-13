using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    public Animator animator;

    private bool bComboExist = false;
    private bool bComboEnable = false;
    private int comboIndex;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void Attacking()
    {
        if (bComboEnable)
        {
            bComboEnable = false;
            bComboExist = true;
        }
        
        animator.SetTrigger("doAttack1");
    }
    public void Combo_Enable()
    {
        bComboEnable = true;
    }

    public void Combo_Disable()
    {
        bComboEnable = false;
    }

    public void Combo_Exist()
    {
        if (bComboExist == false)
            return;

        bComboExist = false;

        animator.SetTrigger("NextConbo");
    }

    public void End_Attack()
    {

    }
}
