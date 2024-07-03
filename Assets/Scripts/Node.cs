using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2Int dir;
    public Node pointNode;

    [Space, SerializeField] SpriteRenderer render;
    [SerializeField] Sprite arrow, blank;

    public void SetPointer(Vector2Int newDir = default, Node node = null)
    {
        dir = newDir;
        pointNode = node;

        render.sprite = dir == default ? blank : arrow;
        if(dir == Vector2Int.right)
        {
            transform.eulerAngles = Vector3.forward * 0;
        }
        else if(dir == Vector2Int.left)
        {
            transform.eulerAngles = Vector3.forward * 180;
        }
        else if(dir == Vector2Int.up)
        {
            transform.eulerAngles = Vector3.forward * 90;
        }
        else if (dir == Vector2Int.down)
        {
            transform.eulerAngles = Vector3.forward * 270;
        }
        else
        {
            transform.eulerAngles = Vector3.forward * 0;
        }
    }
}
