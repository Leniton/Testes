using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private DestructionMethod destructionMethod;

    public void DestroyObject(Vector3? breakPoint = null, Vector3? breakDirection = null)
    {
        destructionMethod.Destroy(this, breakPoint, breakDirection);
    }
}
