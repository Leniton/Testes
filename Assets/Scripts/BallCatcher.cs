using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    private BallScript ballScript;
    
    void Update()
    {
        if(!Input.GetKeyDown(KeyCode.Z)) return;
        ballScript?.DisconnectBall();
        ballScript = null;
    }

    public void ConnectBall(BallScript ball)
    {
        ballScript = ball;
    }
}
