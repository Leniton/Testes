using System.Collections;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    [SerializeField] OShiftMaze maze;

    [SerializableMethods.SerializeMethod]
    public void ShowcaseMaze(int repetitions=1, float delay=.2f)
    {
        StartCoroutine(MazeShowcase(repetitions, delay));
    }

    IEnumerator MazeShowcase(int repetitions, float delay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        for (int i = 0; i < repetitions; i++)
        {
            maze.OriginShift();
            yield return wait;
        }
        Debug.Log("done");
    }
}
