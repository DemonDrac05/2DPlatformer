using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillCasting : MonoBehaviour
{
    //GLOBAL
    public PlayerMovement player;
    public class Skill
    {
        public string skillName { get; set; }
        public bool skillCasted { get; set; }
        public float skillDuration {  get; set; }
        public float skillCD { get; set; }
        public float skillCDTimer;

        public float skillTimer;
        public void setSkillName(string Name)
        {
            skillName = Name;
        }
        public string getSkillName()
        {
            return skillName;
        }

        public void setSkillCasted(bool Casted)
        {
            skillCasted = Casted;
        }
        public bool getSkillCasted()
        {
            return skillCasted;
        }

        public void setDuration(float Duration)
        {
            skillDuration = Duration;
        }
        public float getDuration()
        {
            return skillDuration;
        }

        public void setCooldown(float CD)
        {
            skillCD = CD;
        }
        public float getCooldown()
        {
            return skillCD;
        }

        public Skill(string Name,
                    bool Casted,
                    float Duration,
                    float CD)
        {
            skillName = Name;
            skillCasted = Casted;
            skillDuration = Duration;
            skillCD = CD;
        }
        public void updateSkillActivation()
        {
            if(skillCasted == true)
            {
                skillTimer -= Time.deltaTime;
                if(skillTimer <= 0f)
                {
                    skillCasted = false;
                    skillCDTimer -= Time.deltaTime;
                }
            }
        }
        public void skillActivate()
        {
            if (skillCasted == false && skillCDTimer <= 0f)
            {
                skillCasted = true;
                skillTimer = skillDuration;
                skillCDTimer = skillCD;
            }
        }
    }

    //SKILL ### === *** DEMON SLASH *** === ### \\
    string demonSlash = "Player_Attack";
    [SerializeField]
    public bool DScasted = false;
    private float DSduration = 0.3f;

    public float DStimer;
    public float DSCD = 5f;

    //SKILL ### === *** DEMON DANCE *** === ### \\
    string demonDance = "Player_Skill_DemonDance";
    public bool DDcasted = false;
    private float DDduration = 1.6f;

    public float DDtimer;
    public float DDCD = 7f;
    
    public Skill[] skill;
    public short index = 0, checkpointIndex = 0;
    private void Start()
    {
        Time.timeScale = 1.0f;

        skill = new Skill[100];

        skill[0] = new Skill(demonSlash,DScasted, DSduration, DSCD);

        skill[1] = new Skill(demonDance,DDcasted, DDduration, DDCD);

        while (skill[index] != null)
        {
            skill[index].skillCDTimer = skill[index].skillCD;
            index++;
        }
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < index; i++)
        {
            skill[i].skillCDTimer -= Time.deltaTime;

            if (skill[i].skillCDTimer < 0f)
            {
                skill[i].skillCDTimer = 0f;
            }
        }
    }
    private void Update()
    {
        player = FindObjectOfType<PlayerMovement>();

        Debug.Log(skill[1].skillCDTimer);

        if (Input.GetKeyDown(KeyCode.J) && skill[0].skillCDTimer <= 0f)
        {
            checkpointIndex = 0;
            skill[checkpointIndex].skillActivate();
            CheckSkillCondition(checkpointIndex);
        }

        if (Input.GetKeyDown(KeyCode.K) && skill[1].skillCDTimer <= 0f)
        {
            checkpointIndex = 1;
            skill[checkpointIndex].skillActivate();
            CheckSkillCondition(checkpointIndex);
        }
        //Activation
        skill[checkpointIndex].updateSkillActivation();
        AnimationPlay(checkpointIndex);
    }

    public void CheckSkillCondition(int i)
    {
        for (int j = 0; j < index; j++)
        {
            if (i == j) continue;
            else
            {
                if (skill[j].skillCDTimer <= skill[i].skillDuration)
                {
                    skill[j].skillCDTimer = skill[i].skillDuration;
                }
            }
        }
    }

    public void AnimationPlay(int skillIndex)
    {
        if (skill[skillIndex].skillCasted == true)
        {
            player.changeAnimationState(skill[skillIndex].skillName);
        }
    }
}
