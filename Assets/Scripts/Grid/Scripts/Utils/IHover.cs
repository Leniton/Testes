using System;

public interface IHover
{
    public Action onEnter {  get; set; }
    public Action onExit { get; set; }
}
