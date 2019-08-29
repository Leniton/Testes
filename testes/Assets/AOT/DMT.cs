using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMT : MonoBehaviour
{

    [SerializeField] GameObject hook;
    [SerializeField] Transform shootPoint;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(shootPoint.position, shootPoint.forward, 1000))
            {
                Debug.DrawRay(shootPoint.position, shootPoint.forward*1000, Color.red, 2);
                print("opa");
            }
        }
    }
}
