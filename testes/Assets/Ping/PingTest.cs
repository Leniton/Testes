using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PingTest : MonoBehaviour {

    [SerializeField]
    bool repeat = true;
    public float LimitTimeOut;
    string pingIp = "8.8.8.8";

    [SerializeField]
    List<int> ListaPing = new List<int>();

    Ping P;

    [SerializeField] TextMeshProUGUI Media;
    [SerializeField] TMP_InputField IP;

	void Start () {
        if(LimitTimeOut <= 0)
        {
            LimitTimeOut = 500;
        }
        IP.text = pingIp;

        //GetComponent<AudioSource>().Play();

        //PingMe("8.8.8.8");
        //StartCoroutine("TimeOut");
        //StartCoroutine("PingMe", P.ip);
    }
	
	void Update ()
    {
        if(P != null)
        {
            if (P.isDone && repeat)
            {
                /*print(P.ip);
                print(P.time);*/

                ListaPing.Add(P.time);
                if (ListaPing.Count > 10)
                {
                    ListaPing.RemoveAt(0);
                }

                StopCoroutine("TimeOut");
                PingMe();
                Media.text = "Média: " + MediaPing();
            }
        }
        
	}

    public void setIP(string ip)
    {
        pingIp = ip;
    }

    public void setRepeat()
    {
        repeat = !repeat;
    }

    public void PingMe()
    {
        P = new Ping(pingIp);
        StartCoroutine("TimeOut");
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(LimitTimeOut/1000);
        print("timeout");

        ListaPing.Add(Mathf.FloorToInt(LimitTimeOut));
        if (ListaPing.Count > 10)
        {
            ListaPing.RemoveAt(0);
        }

        if (repeat)
        {
            PingMe();
        }

        yield return null;
    }

    float MediaPing()
    {
        float media = 0;

        for (int i = 0; i < ListaPing.Count; i++)
        {
            media += ListaPing[i];
        }
        media /= ListaPing.Count;
        return media;
    }


}
