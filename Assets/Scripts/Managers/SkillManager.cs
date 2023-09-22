using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum SkillState
{
    Disable,
    Activated,
}
[System.Serializable]
public class CurUsedSkill
{
    public int slotNum;
    public int skillId;
    public CurUsedSkill(int skillId, int slotNum)
    {
        this.slotNum = slotNum;
        this.skillId = skillId;
    }
}
public class SkillManager : Singleton<SkillManager>
{
    public List<SkillData> skilldatas = new();  // skilldatas[i] == skilldata.skillid == i
    public List<bool> activatedSkills = new();  // actavatedskills[i].skilldata == skilldatas[i]
    public List<int> curUsedSkills = new();     // curusedskill[i].value == skilldatas[i].skillid
    public List<float> curCooltimes = new();    // curcooltimes[i] == skilldatas[curusedskill[i]].cooltime

    public Action OnActivateSkill;
    public Action OnSetUsedSkill;
    public Action<int> OnClickDesc;

    public int curSelectSkillindex;
    private void Awake()
    {
        for(int i = 0; i < DataManager.Instance.skillDatas.Length; ++i)
        {
            skilldatas.Add(DataManager.Instance.GetSkillData(i));
            curCooltimes.Add(0);
        }
        for(int i = 0; i < skilldatas.Count;++i)
        {
            activatedSkills.Add(false);         // ��� ����� �Ұ����ϴ� �� �ʱ�ȭ
        }
        int[] tempSkills = { -1, -1, -1, -1 }; // ������� ��ų�� ���� �� �� �ʱ�ȭ
        curUsedSkills.AddRange(tempSkills);
        curSelectSkillindex = -1;
    }
    public void SaveSkillData(ref List<bool> activatedSkills,ref List<int> curUsedSkills)
    {
        // savedata�� �����͵��� ���۷����� �����ͼ� �� ����
        activatedSkills = this.activatedSkills;
        curUsedSkills = this.curUsedSkills;
    }
    public void LoadSkillData()
    {
        SaveData saveData = SaveDataManager.Instance.saveData;
        for(int i= 0; i < saveData.activatedSkills.Count; ++i)
        {
            if (saveData.activatedSkills[i] == true)
                ActiveSkill(i);
        }
        for(int i = 0; i < saveData.curUsedSkills.Count; ++i)
        {
            SetUsedSkill(i, saveData.curUsedSkills[i]);
        }
    }
    public void ActiveSkill(int skillid)
    {
        // ��ų Ȱ��ȭ
        activatedSkills[skillid] = true;
        OnActivateSkill?.Invoke();
    }
    public void SetUsedSkill(int slotnum,int skillid)
    {
        // ����� ��ų�� ����
        for(int i = 0; i < curUsedSkills.Count; ++i)
        {
            // �Է��� ���� �̿ܿ� ������ ��ų�� ��ϵǾ� �ִ� ��� �о
            if (curUsedSkills[i] == skillid)
                curUsedSkills[i] = -1;
        }
        curUsedSkills[slotnum] = skillid;
        OnSetUsedSkill?.Invoke();
    }
    public int GetUsedSkill(int slotnum)
    {
        return curUsedSkills[slotnum];
    }
    public bool IsCoolTime(int slotnum)
    {
        return curCooltimes[GetUsedSkill(slotnum)] < 0.01f;
    }
    public void SetCoolTime(int slotnum)
    {
        curCooltimes[GetUsedSkill(slotnum)] = skilldatas[GetUsedSkill(slotnum)].skillCoolTime;
        StartCoroutine(CoCoolTimeUpdate(slotnum));
    }
    IEnumerator CoCoolTimeUpdate(int slotnum)
    {
        WaitForSeconds updatetime = new WaitForSeconds(0.02f);
        int curSkillindex = GetUsedSkill(slotnum);
        while(curCooltimes[curSkillindex] > 0)
        {
            curCooltimes[curSkillindex] -= 0.02f;
            yield return updatetime;
        }
    }
    public float GetStaminaUsage(int slotnum)
    {
        return skilldatas[GetUsedSkill(slotnum)].staminaUsage;
    }

    // UI
    public void SkillDescButton(int index)
    {
        OnClickDesc.Invoke(index);
        if(activatedSkills[index] == true)
        {
            curSelectSkillindex = index;
        }
        else
        {
            if(skilldatas[index].goldCost <= GameManager.Instance.GetCurGold())
            {
                GameManager.Instance.ModifyGold(skilldatas[index].goldCost,false);
                ActiveSkill(index);
            }
        }
    }
    public void UsedSlotButton(int index)
    {
        SetUsedSkill(index, curSelectSkillindex);
        curSelectSkillindex = -1;
    }
}
