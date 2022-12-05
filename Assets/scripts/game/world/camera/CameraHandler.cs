using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraHandler : HandlerBase<CameraHandler>
{
    public CameraController EnvironmentCameraController;

    public override void LoadData()
    {
    }

    public void DoUpdateFrame()
    {
        EnvironmentCameraController?.DoUpdateFrame();
    }
}