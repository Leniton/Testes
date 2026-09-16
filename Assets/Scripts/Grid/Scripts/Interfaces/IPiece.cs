using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GridSystem
{
    public interface IPiece: IHover
    {
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<Characteristic> characteristics { get; set; }

        public void Initialize()
        {
            id = (int)PieceType.generic;
            for (int i = 0; i < characteristics.Count; i++)
            {
                id = characteristics[i].ModifyID(id);
                characteristics[i].SetUp(this);
            }
        }

        public void StylePiece(Sprite sprite, Color color);

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates);

        public bool AddCharacteristic<T>(T characteristic) where T : Characteristic
        {
            for (int i = 0; i < characteristics.Count; i++)
            {
                if (characteristics[i].GetType().IsAssignableFrom(typeof(T)) ||
                    characteristics[i].GetType().IsSubclassOf(typeof(T)))
                {
                    Debug.LogWarning($"Characteristic {characteristics[i].GetType().Name} already exists!");
                    return false;
                }
            }

            //Debug.Log($"adding {characteristic.GetType().Name}");
            characteristics.Add(characteristic);
            return true;
        }

        public T GetCharacteristic<T>() where T : Characteristic
        {
            T returnValue = null;
            for (int i = 0; i < characteristics.Count; i++)
            {
                try
                {
                    returnValue = (T)characteristics[i];
                    break;
                }
                catch
                {
                }
            }

            return returnValue;
        }

        public string CharacteristicsInfo()
        {
            StringBuilder value = new();

            for (int i = 0; i < characteristics.Count; i++)
            {
                value.Append(characteristics[i]);
                if (i < characteristics.Count - 1) value.Append('\n');
            }

            return value.ToString();
        }

        public string ToString()
        {
            string value = Name;
            value += $"({coordinate.x},{coordinate.y})";
            value += CharacteristicsInfo();

            return value;
        }
    }
}