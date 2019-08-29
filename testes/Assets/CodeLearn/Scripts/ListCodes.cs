using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCodes : MonoBehaviour
{
	public static ListCodes Tlist;

	public string[] LCodigos;

	void Awake()
	{
		if (Tlist != null && Tlist != this)
		{
			Destroy(gameObject);
		}
		else if (Tlist == null)
		{
            DontDestroyOnLoad(gameObject);
			Tlist = this;
		}
	}
}
