using UnityEngine;

// [CreateAssetMenu(fileName = "DestructionMethod", menuName = "Scriptable Objects/DestructionMethod")]
public abstract class DestructionMethod : ScriptableObject
{
    public virtual void Destroy(DestructibleObject obj, Vector3? breakPoint = null, Vector3? breakDirection = null)
    {
        Destroy(obj.gameObject);
    }
}
