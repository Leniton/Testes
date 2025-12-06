using MessagePack;

[MessagePackObject(true)]
public class PrimitiveInput
{
    public string s;
    public int i;
    public float f;
    public double d;
    public bool b;
}