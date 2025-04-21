using PhysicsHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSetPickup : PlataformPickUp
{
    [SerializeField] private Jump jump;

    protected override void OnPlataformerPickup(Plataform_Script plataformer)
    {
        plataformer.Jump = jump;
        Destroy(gameObject);
    }
}
