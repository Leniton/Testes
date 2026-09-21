using System;
using System.Collections.Generic;
using UI.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace GridSystem.VisualElements
{
    public class GridManager : MonoBehaviour, IGrid
    {
        [SerializeField] private UIDocument document;
        [Space] [SerializeField] private int cellSize = 50;
        [SerializeField] private int spacing = 5;
        [SerializeField] private int padding = 10;
        [Space] [SerializeField] int width;
        [SerializeField] int height;

        public int Width => width;
        public int Height => height;

        public List<ITile> tiles { get; set; } = new();
        public ITile hoveredTile { get; set; }
        public bool currentlySelecting { get; set; }
        public Action<ITile> onClick { get; set; }
        public Action<ITile> onEnter { get; set; }
        public Action<ITile> onExit { get; set; }

        private VisualElement grid;

        private void Awake()
        {
            if (IGrid.Instance != this)
            {
                if (IGrid.Instance == null) IGrid.Instance = this;
                else
                {
                    Destroy(gameObject);
                    return;
                }
            }

            SetUpGrid();
        }

        public void SetUpGrid()
        {
            Resolution resolution = Screen.currentResolution;
            Vector2 screenSize = new Vector2(resolution.width, resolution.height);
            int count = Width * Height;

            if (grid != null) document.rootVisualElement.Remove(grid);

            IGrid _grid = this;
            grid = new();
            grid.style.backgroundColor = ColorExtension.GrayShade(.3f);
            grid.style.width = (cellSize * Width) + padding * 2 + (spacing * (Width - 1));
            grid.style.height = (cellSize * Height) + padding * 2 + (spacing * (Height - 1));
            grid.Margin(padding);
            //grid.style.marginLeft = (screenSize.x - grid.style.width.value.value) / 2f;
            grid.style.paddingLeft = padding / 2f;
            grid.style.paddingTop = padding / 2f;
            grid.style.flexDirection = FlexDirection.Row;
            grid.style.flexWrap = Wrap.WrapReverse;
            grid.style.alignItems = Align.FlexStart;
            grid.style.alignSelf = Align.Center;

            for (int i = 0; i < count; i++)
            {
                Tile cell = new Tile();
                cell.name = $"Tile [{i % width},{i / width}]";
                cell.style.width = cellSize;
                cell.style.height = cellSize;
                cell.Margin(spacing);
                cell.style.marginRight = 0;
                if (i >= Width) cell.style.marginTop = 0;
                grid.Add(cell);

                tiles.Add(cell);
                tiles[i].onClick = _grid.OnClick;
                tiles[i].onEnter = _grid.OnEnter;
                tiles[i].onExit = _grid.OnExit;
            }

            document.rootVisualElement.Add(grid);
        }
    }
}