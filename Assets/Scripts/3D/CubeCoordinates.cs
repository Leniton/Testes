using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCoordinates : MonoBehaviour
{
    [SerializeField] float centerDistance;

    private Cube[,,] coordinates = new Cube[3, 3, 3];

    private void Awake()
    {
        Cube[] squares = GetComponentsInChildren<Cube>();
        int id = 0;

        for (int x = 0; x < coordinates.GetLength(0); x++)
        {
            for (int y = 0; y < coordinates.GetLength(1); y++)
            {
                for (int z = 0; z < coordinates.GetLength(2); z++)
                {
                    coordinates[x, y, z] = squares[id];
                    Vector3 coordinate = Vector3.zero;
                    coordinate.x = x - 1;
                    coordinate.y = y - 1;
                    coordinate.z = z - 1;

                    squares[id].transform.localPosition = coordinate * centerDistance;
                    id++;
                }
            }
        }
    }
}
