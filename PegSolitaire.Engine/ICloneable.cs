namespace PegSolitaire.Engine;

public interface ICloneable<out T> : ICloneable
{
    new T Clone();
    object ICloneable.Clone() => Clone();
}