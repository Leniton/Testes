using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Possess : MonoBehaviour
{
    [SerializeField] float detectionRange;
    [SerializeField] GameObject original;
    private Plataform2d_Input originalInput;
    private Plataform_Script originalPlataform;

    private Plataform2d_Input input;
    private Plataform_Script plataform;

    private void Awake()
    {
        originalInput = original.GetComponent<Plataform2d_Input>();
        originalPlataform = original.GetComponent<Plataform_Script>();
        PossessCharacter(original);
    }

    private void PossessCharacter(GameObject character)
    {
        if (input)
        {
            input.enabled = false;
            plataform.levelOfControl = 0;
            plataform.input = Vector3.zero;
        }

        if(character == original)
        {
            original.SetActive(true);
            original.transform.SetParent(input ? input.transform.parent : null);
            transform.SetParent(original.transform);
            input = originalInput;
            plataform = originalPlataform;
        }
        else
        {
            transform.SetParent(character.transform);
            transform.localPosition = Vector3.zero;
            original.transform.SetParent(transform);
            original.SetActive(false);
            original.transform.localPosition = Vector3.zero;

            input = character.GetComponent<Plataform2d_Input>();
            plataform = character.GetComponent<Plataform_Script>();
        }

        input.enabled = true;
        plataform.levelOfControl = 1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DebugDraw.Circle(transform.position, detectionRange, Color.cyan, 2);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.tag == "Player" && hits[i].transform.gameObject != input.gameObject)
                {
                    PossessCharacter(hits[i].transform.gameObject);
                    return;
                }
            }
            PossessCharacter(original);
        }
    }
}
