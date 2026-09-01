using System.Collections.Generic;

namespace UI.Utils.IValue
{
    public class AdditiveIntValue : IValue<int>
    {
        public readonly List<IValue<int>> values;

        public AdditiveIntValue(params IValue<int>[] startingValues)
        {
            values = new(startingValues.Length);
            values.AddRange(startingValues);
        }

        public void Add(IValue<int> value) => values.Add(value);
        public void Remove(IValue<int> value) => values.Remove(value);
        
        public int GetValue()
        {
            int finalValue = 0;
            values.ForEach(v => finalValue += v.GetValue());
            return finalValue;
        }
    }
}