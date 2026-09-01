using System;
namespace UI.Utils.IValue
{
    public interface IValue<T>
    {
        public T GetValue();
    }

    public class Value<T> : IValue<T>
    {
        public T value;
        public Value(T value) => this.value = value;
        public T GetValue() => value;
    }

    public class ReferenceValue<T> : IValue<T>
    {
        public Func<T> valueMethod;
        public ReferenceValue(Func<T> valueMethod) => this.valueMethod = valueMethod;
        public T GetValue() => valueMethod();
    }
}
