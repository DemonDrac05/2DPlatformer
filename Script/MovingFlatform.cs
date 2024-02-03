using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] pos;

    private int currenPosIndex = 0;

    [SerializeField] public float speed = 2f;
    private void Update()
    {
        if (Vector2.Distance(pos[currenPosIndex].transform.position, transform.position) < .1f)
        {
            currenPosIndex++;
            if(currenPosIndex >= pos.Length)
            {
                currenPosIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, pos[currenPosIndex].transform.position, Time.deltaTime * speed);
    }
}
