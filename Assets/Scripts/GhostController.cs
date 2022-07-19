using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : PortalableObject
{
    // Start is called before the first frame update
    private GhostMovement cameraMove;

    protected override void Awake()
    {
        base.Awake();

        cameraMove = GetComponent<GhostMovement>();
    }

    public override void Warp()
    {
        base.Warp();
        cameraMove.ResetTargetRotation();
    }
}
