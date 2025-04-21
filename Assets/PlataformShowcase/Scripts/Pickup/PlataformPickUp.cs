using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlataformPickUp : Pickup
{
    protected override void OnPickup(GameObject target)
    {
        if (!target.TryGetComponent<Plataform_Script>(out var plataformer)) return;

        OnPlataformerPickup(plataformer);
    }

    protected abstract void OnPlataformerPickup(Plataform_Script plataformer);
}
