using UnityEngine;

public struct ContactData
{
    public Vector3 point, normal;
    public float separation;

    public ContactData(Vector3 _point, Vector3 _normal,float _separation)
    {
        point = _point;
        normal = _normal;
        separation = _separation;
    }
}
