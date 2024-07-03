using AddressableAsyncInstances;
using UnityEngine;

public class OShiftMaze : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] Vector2Int spacing;

    private Node[] maze;
    private Vector2Int origin;

    private void Awake()
    {
        CreateMaze();
    }

    private void CreateMaze()
    {
        maze = new Node[size.x * size.y];
        for (int i = 0; i < maze.Length; i++)
        {
            //get id, position, and spawn node
            int id = i;
            Vector2Int position = IdToCoordinate(id);
            new AAComponent<Node>("Node", transform).QueueAction((node) =>
            {
                //setup node in maze
                maze[id] = node;
                node.transform.localPosition = (Vector2)position;

                //change its pointer position to the base one
                Vector2Int pointDirection = Vector2Int.zero;
                if (position.x == 0)
                {
                    if (position.y > 0) pointDirection = Vector2Int.down;
                    else origin = position;
                }
                else pointDirection = Vector2Int.left;
                Node conectingNode = pointDirection == Vector2Int.zero ? null : maze[CoordinateToId(position + pointDirection)];
                node.SetPointer(pointDirection, conectingNode);
            });
        }
    }

    [SerializableMethods.SerializeMethod]
    private void OriginShift(int repetitions = 1)
    {
        //pick random direction from current origin
        Vector2Int shiftDir = RandomDirection();
        while (!ValidCoordinate(origin + shiftDir))
        {
            shiftDir = RandomDirection();
        }

        //id of new origin
        int id = CoordinateToId(origin + shiftDir);
        //point old origin to new origin
        maze[CoordinateToId(origin)].SetPointer(shiftDir, maze[id]);
        //set node at id as new origin
        origin += shiftDir;
        //new origin points at nothing
        maze[id].SetPointer();

        if (repetitions > 1) OriginShift(repetitions - 1);
    }

    //helpers
    private Vector2Int RandomDirection()
    {
        int rand = Random.Range(0, 4);
        Vector2Int rDir = Vector2Int.zero;

        switch (rand)
        {
            case 0:
                rDir = Vector2Int.up;
                break;
            case 1:
                rDir = Vector2Int.down;
                break;
            case 2:
                rDir = Vector2Int.left;
                break;
            default:
                rDir = Vector2Int.right;
                break;
        }

        return rDir;
    }
    private bool ValidCoordinate(Vector2Int coordinate) => (coordinate.x >= 0 && coordinate.x < size.x) && (coordinate.y >= 0 && coordinate.y < size.y);
    private int CoordinateToId(Vector2Int coordinate) => (coordinate.y * size.x) + coordinate.x;//possibly has errors
    private Vector2Int IdToCoordinate(int id) => new Vector2Int(id % size.x, id / size.x);
}
