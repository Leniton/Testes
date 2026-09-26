using GridSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    [Serializable]
    public struct Area
    {
        //public enum Type { Point, Line, Cone, Square, Half_Square }
        private int range;
        Direction4 usedDirection;
        Direction4 axisDir;
        Direction4 axisCut;
        Direction4 rangeCut;
        Direction4 directionOffset;
        int internalCut;
        List<Coordinate> areaCoordinates;

        public int Range
        {
            get { return range; }
            set
            {
                range = value;
                CalculateArea();
            }
        }

        public Area(int Range, Direction4 direction, Direction4 axis,
            Direction4 cut = default, Direction4 RangeCut = default, Direction4 DirectionOffset = default,
            int innerCut = 0)
        {
            range = Range;
            usedDirection = direction;
            axisDir = axis;
            axisCut = cut;
            rangeCut = RangeCut;
            directionOffset = DirectionOffset;
            areaCoordinates = new List<Coordinate>();
            internalCut = innerCut;
            CalculateArea();
        }

        public static Area Point => new Area(0, Direction4.One, Direction4.One);

        public static Area Square(int range, int innerCut = 0) =>
            new Area(range, Direction4.One, Direction4.One, innerCut: innerCut);

        public static Area Half_Square(int range, Vector2Int direction, int innerCut = 0)
        {
            Direction4 finalDirection = Direction4.VectorToDirection(direction);
            return new(range, finalDirection, finalDirection, innerCut: innerCut);
        }

        public static Area Diamond(int range, int innerCut = 0)
        {
            Direction4 dir;
            dir.x = -1;
            dir.y = -1;
            dir.nX = -1;
            dir.nY = -1;
            return new Area(range, Direction4.One, dir, innerCut: innerCut);
        }

        public static Area Cone(int range, Vector2Int direction, int innerCut = 0)
        {
            Direction4 dir = Direction4.VectorToDirection(direction);
            Direction4 axis;
            if (direction.x * direction.y != 0)
            {
                axis = dir;
                axis.x *= -1;
                axis.y *= -1;
                axis.nX *= -1;
                axis.nY *= -1;
            }
            else
            {
                axis.x = Mathf.Clamp(direction.x, 0, 1);
                axis.y = Mathf.Clamp(direction.y, 0, 1);
                axis.nX = Mathf.Clamp(-direction.x, 0, 1);
                axis.nY = Mathf.Clamp(-direction.y, 0, 1);
            }

            return new Area(range, dir, axis, innerCut: innerCut);
        }

        public static Area Drill(int range, Vector2Int direction, int innerCut = 0)
        {
            Direction4 dir = Direction4.VectorToDirection(direction);
            Direction4 axis;
            if (direction.x * direction.y != 0)
            {
                axis.x = Mathf.Clamp(direction.x, 0, 1);
                axis.y = Mathf.Clamp(direction.y, 0, 1);
                axis.nX = Mathf.Clamp(-direction.x, 0, 1);
                axis.nY = Mathf.Clamp(-direction.y, 0, 1);
            }
            else
            {
                axis = dir;
                axis.x *= -1;
                axis.y *= -1;
                axis.nX *= -1;
                axis.nY *= -1;
            }

            return new Area(range, dir, axis, innerCut: innerCut);
        }

        public static Area X(int range, int thickness = 1, int innerCut = 0)
        {
            if (thickness <= 0) return new Area(range, Direction4.One, Direction4.Zero);
            Direction4 cut = Direction4.Zero;
            cut.x = thickness;
            cut.y = thickness;
            cut.nX = thickness;
            cut.nY = thickness;
            return new Area(range, Direction4.One, Direction4.One, cut, innerCut: innerCut);
        }

        public static Area Line(int range, Vector2Int direction, int thickness = 1, int innerCut = 0)
        {
            Direction4 dir = Direction4.VectorToDirection(direction);
            Direction4 cut = dir;
            if (direction.x * direction.y != 0)
            {
                cut.x *= thickness;
                cut.y *= thickness;
                cut.nX *= thickness;
                cut.nY *= thickness;
                return new Area(range, dir, dir, cut, innerCut: innerCut);
            }

            int cutValue = NumberUtil.Invert(thickness - 1, range);
            cut.x *= cutValue * (direction.x ^ 1);
            cut.y *= cutValue * (direction.y ^ 1);
            cut.nX *= cutValue * (-direction.x ^ 1);
            cut.nY *= cutValue * (-direction.y ^ 1);
            return new Area(range, dir, dir, Direction4.Zero, cut, innerCut: innerCut);
        }

        public static Area Circle(int range, int innerCut = 0)
        {
            Direction4 dir;
            dir.x = -1;
            dir.y = -1;
            dir.nX = -1;
            dir.nY = -1;
            int offsetValue = Mathf.FloorToInt(range / 2f);
            Direction4 offset;
            offset.x = offsetValue;
            offset.y = offsetValue;
            offset.nX = offsetValue;
            offset.nY = offsetValue;
            return new Area(range, Direction4.One, dir, Direction4.Zero, Direction4.Zero, offset, innerCut);
        }

        public static Area PlusSign(int range, int thickness = 1, int innerCut = 0)
        {
            Direction4 offset = Direction4.One;
            offset.x *= range;
            offset.y *= range;
            offset.nX *= range;
            offset.nY *= range;
            Area area = new Area(Range: range, direction: Direction4.One, axis: Direction4.One, DirectionOffset: offset,
                innerCut: innerCut);

            for (int i = innerCut + 1; i <= range; i++)
            {
                for (int u = 1; u < thickness; u++)
                {
                    //x
                    area.AddCoordinate(new(u, i));
                    area.AddCoordinate(new(u, -i));
                    area.AddCoordinate(new(-u, i));
                    area.AddCoordinate(new(-u, -i));
                    //y
                    area.AddCoordinate(new(i, u));
                    area.AddCoordinate(new(i, -u));
                    area.AddCoordinate(new(-i, u));
                    area.AddCoordinate(new(-i, -u));
                }
            }

            return area;
        }

        public void CalculateArea()
        {
            areaCoordinates.Clear();
            Coordinate coordinate;
            int x, y; //coordinate Values on loop
            int xSize = (1 + range * usedDirection.x) - rangeCut.x; //the length of the x value
            int ySize = (1 + range * usedDirection.y) - rangeCut.y; //the length of the y value
            int nxSize = (1 + range * usedDirection.nX) - rangeCut.nX; //the length of the x value
            int nySize = (1 + range * usedDirection.nY) - rangeCut.nY; //the length of the y value
            int offset;
            int xValue;
            int yValue;
            int xCut;
            int yCut;

            //adding on quadrant (1,1)
            for (y = 0; y < ySize; y++)
            {

                yValue = ((Mathf.Clamp(y - directionOffset.y, 0, ySize) * axisDir.y)) * axisDir.y; //y value 
                yValue = NumberUtil.Invert(yValue, xSize * Mathf.Clamp(-axisDir.y, 0, 1));
                yValue += 1 * Mathf.Clamp(axisDir.y, 0, 1); //1 as the center is an extra
                yCut = Mathf.Clamp((yValue - axisCut.y), 0, yValue); //cut value
                yCut *= Mathf.Clamp(axisDir.y, 0, 1) * Mathf.Clamp(axisCut.y, 0, 1); //activating conditions

                yValue = Mathf.Clamp(yValue, 0, xSize) * usedDirection.x; //clamp value
                //Debug.Log($"y: {y} | cut: {internalCut} | yValue: {yValue}");
                for (x = yCut; x < yValue; x++) //adding y Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"adding ({x},{y})");
                    areaCoordinates.Add(coordinate);
                }

                xValue = (y * axisDir.x) * axisDir.x; //x value
                xValue = NumberUtil.Invert(xValue, xSize) * Math.Abs(axisDir.x); //invert always
                xValue -= (directionOffset.x * Mathf.Clamp(y, 0, 1)) *
                          Mathf.Clamp(Mathf.Abs(directionOffset.x), 0, 1); //applying offset
                xCut = Mathf.Clamp((xValue - axisCut.x), 0, xValue); //cut value
                xCut *= Mathf.Clamp(axisDir.x, 0, 1) * Mathf.Clamp(axisCut.x, 0, 1); //activating conditions

                offset = Mathf.Clamp(xSize - (xValue), 0, xSize) *
                         Mathf.Clamp(axisDir.x, 0, 1); //offset it when increasing

                //internalCut = offset;// Mathf.Clamp(offset + (centerCut.x * Mathf.Clamp(centerCut.x - y, 0, 1)), 0, Mathf.Max(centerCut.x, offset));
                //offset = internalCut;//center cut
                xValue += offset; //adding offset to the total
                xValue = Mathf.Clamp(xValue, 0, xSize) * usedDirection.y; //clamp value
                //Debug.Log($"{yValue}, skip {offset} then {xValue}");
                for (x = offset; x < xValue - xCut; x++) //adding x Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }
            }

            //adding on quadrant (-1,1)
            for (y = 0; y < ySize; y++)
            {
                yValue = ((Mathf.Clamp(y - directionOffset.y, 0, ySize) * axisDir.y)) *
                         axisDir.y; //y value (1 as the center is an extra)
                yValue = NumberUtil.Invert(yValue, nxSize * Mathf.Clamp(-axisDir.y, 0, 1));
                yValue += 1 * Mathf.Clamp(axisDir.y, 0, 1);
                yCut = Mathf.Clamp((yValue - axisCut.y), 0, yValue); //cut value
                yCut *= Mathf.Clamp(axisDir.y, 0, 1) * Mathf.Clamp(axisCut.y, 0, 1); //activating conditions

                yValue = Mathf.Clamp(yValue, 0, nxSize) * usedDirection.nX; //clamp value
                for (x = -yValue + 1; x <= -yCut; x++) //adding y Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }

                xValue = (y * axisDir.nX) * axisDir.nX; //nX value
                xValue = NumberUtil.Invert(xValue, nxSize) * Math.Abs(axisDir.nX); //invert always
                xValue -= (((directionOffset.nX)) * Mathf.Clamp(y, 0, 1)) *
                          Mathf.Clamp(Mathf.Abs(directionOffset.nX), 0, 1); //applying offset
                xCut = Mathf.Clamp((xValue - axisCut.nX), 0, xValue); //cut value
                xCut *= Mathf.Clamp(axisDir.nX, 0, 1) * Mathf.Clamp(axisCut.nX, 0, 1); //activating conditions

                xValue = Mathf.Clamp(xValue, 0, nxSize) * usedDirection.y; //clamp value
                offset = ((nxSize - xValue) * Mathf.Clamp(-axisDir.nX, 0, 1) - nxSize + 1); //offset it when increasing
                //Debug.Log($"{yValue}, skip {offset} then {xValue}");
                for (x = offset + xCut; x < offset + xValue; x++) //adding x Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }
            }

            //adding on quadrant (1,-1)
            for (y = -nySize + 1; y <= 0; y++)
            {
                yValue = ((Mathf.Clamp(-y - directionOffset.nY, 0, nySize) * axisDir.nY)) *
                         axisDir.nY; //y value (1 as the center is an extra)
                yValue = NumberUtil.Invert(yValue, xSize * Mathf.Clamp(-axisDir.nY, 0, 1));
                yValue += 1 * Mathf.Clamp(axisDir.nY, 0, 1);
                yCut = Mathf.Clamp((yValue - axisCut.nY), 0, yValue); //cut value
                yCut *= Mathf.Clamp(axisDir.nY, 0, 1) * Mathf.Clamp(axisCut.nY, 0, 1); //activating conditions

                yValue = Mathf.Clamp(yValue, 0, xSize) * usedDirection.x; //clamp value
                for (x = yCut; x < yValue; x++) //adding y Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }

                xValue = (-y * axisDir.x) * axisDir.x; //x value
                xValue = NumberUtil.Invert(xValue, xSize) * Math.Abs(axisDir.x); //invert always
                xValue -= (directionOffset.x * Mathf.Clamp(-y, 0, 1)) *
                          Mathf.Clamp(Mathf.Abs(directionOffset.x), 0, 1); //applying offset
                xCut = Mathf.Clamp((xValue - axisCut.x), 0, xValue); //cut value
                xCut *= Mathf.Clamp(axisDir.x, 0, 1) * Mathf.Clamp(axisCut.x, 0, 1); //activating conditions

                offset = Mathf.Clamp(xSize - (xValue), 0, xSize) *
                         Mathf.Clamp(axisDir.x, 0, 1); //offset it when increasing
                xValue += offset; //adding offset to the total
                xValue = Mathf.Clamp(xValue, 0, xSize) * usedDirection.nY; //clamp value
                //Debug.Log($"{yValue}, skip {offset} then {xValue} | current: {y}\n extra: {1 * Mathf.Clamp(axisDir.nY, 0, 1)}");
                for (x = offset; x < xValue - xCut; x++) //adding x Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }
            }

            //adding on quadrant (-1,-1)
            for (y = -nySize + 1; y <= 0; y++)
            {
                yValue = ((Mathf.Clamp(-y - directionOffset.nY, 0, nySize) * axisDir.nY)) *
                         axisDir.nY; //y value (1 as the center is an extra)
                yValue = NumberUtil.Invert(yValue, nxSize * Mathf.Clamp(-axisDir.nY, 0, 1));
                yValue += 1 * Mathf.Clamp(axisDir.nY, 0, 1);
                yCut = Mathf.Clamp((yValue - axisCut.nY), 0, yValue); //cut value
                yCut *= Mathf.Clamp(axisDir.nY, 0, 1) * Mathf.Clamp(axisCut.nY, 0, 1); //activating conditions

                yValue = Mathf.Clamp(yValue, 0, nxSize) * usedDirection.nX; //clamp value
                for (x = -yValue + 1; x <= -yCut; x++) //adding y Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }

                xValue = (-y * axisDir.nX) * axisDir.nX; //nX value
                xValue = NumberUtil.Invert(xValue, nxSize) * Math.Abs(axisDir.nX); //invert always
                xValue -= (((directionOffset.nX)) * Mathf.Clamp(-y, 0, 1)) *
                          Mathf.Clamp(Mathf.Abs(directionOffset.nX), 0, 1); //applying offset
                xCut = Mathf.Clamp((xValue - axisCut.nX), 0, xValue); //cut value
                xCut *= Mathf.Clamp(axisDir.nX, 0, 1) * Mathf.Clamp(axisCut.nX, 0, 1); //activating conditions

                xValue = Mathf.Clamp(xValue, 0, nxSize) * usedDirection.nY; //clamp value
                offset = ((nxSize - xValue) * Mathf.Clamp(-axisDir.nX, 0, 1) - nxSize + 1); //offset it when increasing
                //totalCells += offset;//adding offset to the total
                //Debug.Log($"{yValue}, skip {offset} then {xValue} | total: {totalCells}");
                for (x = offset + xCut; x < offset + xValue; x++) //adding x Values
                {
                    coordinate.x = x;
                    coordinate.y = y;
                    //Debug.Log($"checking ({x},{y})");
                    if (!areaCoordinates.Contains(coordinate)) areaCoordinates.Add(coordinate);
                }
            }

            if (internalCut <= 0) return;

            Area cut = new Area(internalCut - 1, usedDirection, axisDir, axisCut, rangeCut, directionOffset);
            for (int i = 0; i < cut.areaCoordinates.Count; i++)
            {
                //Debug.Log($"removing ({cut.areaCoordinates[i].x},{cut.areaCoordinates[i].y})");
                areaCoordinates.Remove(cut.areaCoordinates[i]);
            }
        }

        public List<Coordinate> GetCoordinates(Coordinate origin, int width, int height)
        {
            var coordinates = new List<Coordinate>();
            Coordinate coordinate;

            int gridSize = width * height;

            for (int i = 0; i < areaCoordinates.Count; i++)
            {
                coordinate.x = origin.x + areaCoordinates[i].x;
                coordinate.y = origin.y + areaCoordinates[i].y;
                if (coordinate.x >= 0 && coordinate.y < gridSize) coordinates.Add(coordinate);
            }

            return coordinates;
        }

        public List<Coordinate> GetCoordinates(int originIndex, int width, int height)
        {
            Coordinate coordinate;
            coordinate.x = originIndex % width;
            coordinate.y = originIndex / width;
            return GetCoordinates(coordinate, width, height);
        }

        public void CopyForm(List<Coordinate> coordinates)
        {
            areaCoordinates.Clear();
            for (int i = 0; i < coordinates.Count; i++)
            {
                areaCoordinates.Add(coordinates[i]);
            }
        }

        public void AddCoordinate(Coordinate coordinate)
        {
            if (areaCoordinates.Contains(coordinate)) return;
            areaCoordinates.Add(coordinate);
        }

        public void ChangeDirection(Vector2Int newDirection)
        {
            Direction4 finalDirection = Direction4.VectorToDirection(newDirection);

        }
    }
}