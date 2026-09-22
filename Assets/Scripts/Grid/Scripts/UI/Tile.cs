using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GridSystem.UI
{
    public class Tile : MonoBehaviour, ITile, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public List<IPiece> pieces { get; set; } = new();

        public int pieceID
        {
            get
            {
                if (pieces.Count <= 0) return (int)PieceType.none;
                int value = (int)PieceType.generic;
                for (int i = 0; i < pieces.Count; i++)
                {
                    value |= pieces[i].id;
                }

                return value;
            }
        }

        private Image img;
        public ITile.State state { get; set; }

        [SerializeField] private Color DefaultColor, SelectableColor, ValidColor, InvalidColor;
        public Color defaultColor => DefaultColor;

        public Color selectableColor => SelectableColor;

        public Color validColor => ValidColor;

        public Color invalidColor => InvalidColor;
        public List<Color> colors { get; set; } = new();
        private Color? selectedColor;

        //callbacks
        public Action<ITile> onPickTile { get; set; }
        public Action<ITile> onSelectionEnter { get; set; }
        public Action<ITile> onSelectionExit { get; set; }
        public Action<IPiece> onPiecePlaced { get; set; }
        public Action<IPiece> onPieceRemoved { get; set; }

        private GraphicRaycaster raycaster;
        private bool left = true;

        private void Awake()
        {
            img = GetComponent<Image>();
            img.color = DefaultColor;
            raycaster = GetComponentInParent<GraphicRaycaster>();
        }

        public void ApplyColor(Color color)
        {
            img.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            for (int i = 0; i < pieces.Count; i++) pieces[i].onClick?.Invoke();
            onPickTile?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Contains(eventData.position - eventData.delta) && !left) return;
            left = false;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onEnter?.Invoke();
            onSelectionEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Contains(eventData.position)) return;
            left = true;

            for (int i = 0; i < pieces.Count; i++) pieces[i].onExit?.Invoke();
            onSelectionExit?.Invoke(this);
        }

        public void SetColor(Color color)
        {
            colors.Clear();
            AddColor(color);
        }

        public void AddColor(Color color)
        {
            Color invertedColor = ColorExtension.InvertColor(color);
            invertedColor.a = 0;

            colors.Add(invertedColor);
            UpdateColor();
        }

        public void UpdateColor()
        {
            Color newColor = Color.white;
            for (int i = 0; i < colors.Count; i++)
            {
                newColor -= colors[i] * (.25f + (1f / (colors.Count + 1f)));
            }

            ApplyColor(newColor);
        }

        public void RemoveColor(Color color)
        {
            Color invertedColor = ColorExtension.InvertColor(color);
            invertedColor.a = 0;

            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i] == invertedColor)
                {
                    colors.RemoveAt(i);
                    UpdateColor();
                    break;
                }
            }
        }

        private bool Contains(Vector2 mousePosition)
        {
            PointerEventData eventData = new(EventSystem.current) { position = mousePosition };
            List<RaycastResult> results = new();
            raycaster.Raycast(eventData, results);

            for (int i = 0; i < results.Count; i++)
                if (results[i].gameObject == img.gameObject)
                    return true;

            return false;
        }
    
        private Color StateColor() => state switch
        {
            ITile.State.selectable => selectableColor,
            ITile.State.invalid => invalidColor,
            _ => defaultColor,
        };
        public void SetState(ITile.State newState)
        {
            RemoveColor(StateColor());
            state = newState;
            AddColor(StateColor());
        }
        
        public void Select(int filter)
        {
            ITile tile = this;
            selectedColor = IGrid.IsInFilter(tile.pieceID, filter) ? validColor : invalidColor;
            AddColor(selectedColor.Value);
        }
        public void Deselect()
        {
            if (!selectedColor.HasValue) return;
            RemoveColor(selectedColor.Value);
        }
    }
}