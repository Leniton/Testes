using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJ_Spawner : MonoBehaviour
{

    [SerializeField] GameObject[] BJ = new GameObject[0];

    [SerializeField] int size = 1;
    Vector2 TamanhoBJ;

    void Start()
    {
        TamanhoBJ = BJ[0].GetComponent<BoxCollider2D>().bounds.size;
        print("size: " + TamanhoBJ.y);
        StartCoroutine("fill");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator fill()
    {
        if(size > 1)
        {
            bool full = true;
            int[] NspawnA = new int[size];
            int NspawnT = 0;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
            for (int i = 0; i < size; i++)
            {
                Vector2 pos = transform.position;
                pos.x = pos.x + TamanhoBJ.x * i;
                hit = Physics2D.Raycast(pos, -transform.up);
                NspawnA[i] = Mathf.FloorToInt(hit.distance / (TamanhoBJ.y));
                print("coluna " + i + ": " + NspawnA[i]);
                NspawnT += NspawnA[i];
                if(NspawnA[i] != 0)
                {
                    full = false;
                }
            }
            print("total: " + NspawnT);
            if (!full)
            {
                for (int i = 0; i < NspawnT; i++)
                {
                    int n = 0;
                    for (int u = 0; u < size; u++)
                    {
                        if(NspawnA[u] > 0)
                        {

                            int rand = Random.Range(0, BJ.Length);
                            Vector3 spawnPoint = transform.position;
                            //spawnPoint.y -= TamanhoBJ.y;
                            spawnPoint.x = transform.position.x + TamanhoBJ.x * u;
                            Instantiate(BJ[rand], spawnPoint, Quaternion.identity);
                            n += 1;
                        }
                    }
                    i += n -1;
                    yield return new WaitForSeconds(1);
                }
            }
            else
            {
                print("wait a minute...");
                yield return null;
            }
        }
        else if(size == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
            if(hit.distance >= TamanhoBJ.y)
            {
                int nspawn = Mathf.FloorToInt(hit.distance / (TamanhoBJ.y));
                print(nspawn);
                for (int i = 0; i < nspawn; i++)
                {
                    int rand = Random.Range(0, BJ.Length);
                    Vector3 spawnPoint = transform.position;
                    spawnPoint.y -= TamanhoBJ.y;
                    Instantiate(BJ[rand], spawnPoint, Quaternion.identity);
                    yield return new WaitForSeconds(1);
                }
            }
            else
            {
                print("wait a minute...");
                yield return null;
            }

        }
        else
        {
            Debug.LogError("Spawner sem tamanho!!!");
        }
    }
}
