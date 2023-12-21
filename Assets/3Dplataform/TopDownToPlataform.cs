using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownToPlataform : MonoBehaviour {

    float pace;
    float goal;

    float Rpace;
    float Rgoal;

    [SerializeField]
    /*[Range(0, 10)]*/ float paceRate;

    bool moving;
	
    void Awake()
    {
        goal = 0;
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !moving)
        {
            if(goal == 0)
            {
                goal = 10;
                Rgoal = 90;
            }
            else
            {
                goal = 0;
                Rgoal = 0;
            }
            moving = true;
            pace = (goal - transform.position.y) / paceRate;
            Rpace = (Mathf.DeltaAngle(transform.eulerAngles.x,Rgoal)) / paceRate;
            GetComponent<player_look>().enabled = false;
            print("Pace: " + pace);
            print("rPace: " + Rpace);
        }
        if (moving)
        {
            transform.Translate(new Vector3(0, pace, 0),Space.World);
            transform.Rotate(Rpace,0,0);

            if ((transform.localPosition.y >= goal && goal > 0) || 
                (transform.localPosition.y <= 0 && goal == 0))
            {
                print("STOP!!");
                transform.localPosition = new Vector3(0, goal, 0);
                transform.localEulerAngles = new Vector3(Rgoal, transform.eulerAngles.y, transform.eulerAngles.z);
                moving = false;
                if(Rgoal == 0)
                {
                    GetComponent<player_look>().enabled = true;
                }
            }
        }
	}
}
