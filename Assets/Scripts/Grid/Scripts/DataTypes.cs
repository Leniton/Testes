using UnityEngine;

namespace GridSystem
{
    [System.Serializable]
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static int Distance(Coordinate a, Coordinate b) => Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
        public static implicit operator Vector2(Coordinate c) => new Vector2(c.x, c.y);

        public int Distance(Coordinate coordinate) =>
            Mathf.Max(Mathf.Abs(x - coordinate.x), Mathf.Abs(y - coordinate.y));

        /// <summary>
        /// The coordinate (0, 0)
        /// </summary>
        public static Coordinate Zero => new Coordinate();

        public override string ToString() => $"coordinate:({x},{y})";
    }

    [System.Serializable]
    public struct Direction4
    {
        public int x, y, nX, nY;

        public Direction4(int _x, int _y, int _nX, int _nY)
        {
            x = _x;
            y = _y;
            nX = _nX;
            nY = _nY;
        }

        public static Direction4 One => new Direction4(1, 1, 1, 1);
        public static Direction4 Zero => new Direction4(0, 0, 0, 0);
        public static Direction4 Half_Up => new Direction4(1, 1, 1, 0);
        public static Direction4 Half_Down => new Direction4(1, 0, 1, 1);
        public static Direction4 Half_Right => new Direction4(1, 1, 0, 1);
        public static Direction4 Half_Left => new Direction4(0, 1, 1, 1);
        public static Direction4 Horizontal => new Direction4(1, 0, 1, 0);
        public static Direction4 Vertical => new Direction4(0, 1, 0, 1);
        public static Direction4 Up => new Direction4(0, 1, 0, 0);
        public static Direction4 Down => new Direction4(0, 0, 0, 1);
        public static Direction4 Right => new Direction4(1, 0, 0, 0);
        public static Direction4 Left => new Direction4(0, 0, 1, 0);

        public static Direction4 VectorToDirection(Vector2Int vector)
        {
            //for positives sign(direction)
            //for negatives clamp(sign(direction*-1))
            Direction4 dir = One;
            dir.x = (int)Mathf.Clamp01(Mathf.Sign(vector.x));
            dir.y = (int)Mathf.Clamp01(Mathf.Sign(vector.y));
            dir.nX = (int)Mathf.Clamp01(Mathf.Sign(vector.x * -1));
            dir.nY = (int)Mathf.Clamp01(Mathf.Sign(vector.y * -1));

            return dir;
        }
    }

    [System.Flags]
    public enum PieceType
    {
        none = 1,
        generic = 2,
        entity = 4,
        destructible = 8,
        unit = 16,
        all = 255
    }
}