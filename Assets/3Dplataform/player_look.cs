using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_look : MonoBehaviour {

	Vector3 angle;
	[Range(1f,4f)]
	public float sen = 1f;

    [SerializeField] GameObject ToFollow; 
    //nota: to follow em cima do obj
    [Space]
    [SerializeField] GameObject FollowMe;
    [SerializeField] bool FreezeY, FreezeXZ;


    void Start () {
		Cursor.lockState = CursorLockMode.Locked;

        /*if (ToFollow != null)
        {
            distancia = ToFollow.transform.position - transform.position;
        }*/
    }

	// Update is called once per frame
	void Update () 
	{
        #region rotate
        angle = new Vector3 (-Input.GetAxis ("Mouse Y")*sen + transform.eulerAngles.x, Input.GetAxis ("Mouse X")*sen + transform.eulerAngles.y, 0);
		transform.eulerAngles = (angle);

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (Cursor.lockState == CursorLockMode.Locked) 
			{
				Cursor.lockState = CursorLockMode.None;
			} else if (Cursor.lockState == CursorLockMode.None) 
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		transform.eulerAngles = (angle);
		if(Input.GetKeyDown(KeyCode.T)){
			sen += 0.5f;
		}
		if(Input.GetKeyDown(KeyCode.Y)){
			sen -= 0.5f;
		}
        #endregion

        if(ToFollow != null)
        {
            transform.position = ToFollow.transform.position;// + distancia;
        }

        if(FollowMe != null)
        {
            Vector3 r = Vector3.zero;
            if (!FreezeXZ)
            {
                r = new Vector3(transform.eulerAngles.x, r.y, transform.eulerAngles.z);
            }
            if (!FreezeY)
            {
                r = new Vector3(r.x, transform.eulerAngles.y, r.z);
            }
            if(FreezeXZ && FreezeY)
            {
                r = FollowMe.transform.eulerAngles;
            }

            FollowMe.transform.eulerAngles = r;
        }
    }
}
