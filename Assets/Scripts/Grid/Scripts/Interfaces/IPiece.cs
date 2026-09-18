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
        public List<ICharacteristic> characteristics { get; set; }

        public void Initialize()
        {
            RefreshId();
        }

        public void RefreshId()
        {
            var newId = (int)PieceType.generic;
            characteristics ??= new();
            for (int i = 0; i < characteristics.Count; i++)
                characteristics[i].ModifyID(ref newId);
            id = newId;
        }

        public void StylePiece(Sprite sprite, Color color);

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates);

        public bool AddCharacteristic<T>(T characteristic) where T : ICharacteristic
        {
            characteristics ??= new();
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
            characteristic.SetUp(this);
            RefreshId();
            return true;
        }

        public T GetCharacteristic<T>() where T : ICharacteristic
        {
            T returnValue = default(T);
            characteristics ??= new();
            for (int i = 0; i < characteristics.Count; i++)
            {
                try
                {
                    returnValue = (T)characteristics[i];
                    break;
                }
                catch
                {
                    // ignored
                }
            }

            return returnValue;
        }

        public string CharacteristicsInfo()
        {
            characteristics ??= new();
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