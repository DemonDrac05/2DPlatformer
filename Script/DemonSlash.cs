using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemonSlash : MonoBehaviour
{
    public PlayerMovement player;
    public Transform slashSpawnPoint;
    public GameObject slashSpawnPointPrefab;

    public short skill_s1_CD;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        skill_s1_CD = 1;
    }
    private void Update()
    {
        //if (player.skill.skillCDTimer < 0f && player.skill.DScasted == false) 
        //    skill_s1_CD = 1;

        if (Input.GetKeyDown(KeyCode.J) && skill_s1_CD == 1)
        {
            Instantiate(slashSpawnPointPrefab, slashSpawnPoint.position, transform.rotation);
            skill_s1_CD--;
        }
    }
}
