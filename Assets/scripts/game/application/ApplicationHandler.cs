using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ApplicationHandler : HandlerBase<ApplicationHandler>
{
    public override void LoadData()
    {
        GameHandler.Instance.Initialization();
    }

    public void DoLoadAwake()
    {
        GameHandler.Instance.DoLoadAwake();
    }

    public void DoLoadStart()
    {
        GameHandler.Instance.DoLoadStart();
    }

    public void DoUpdateFrame()
    {
        GameHandler.Instance.DoUpdateFrame();
    }

    public void DoUpdateFrameEnd()
    {
        GameHandler.Instance.DoUpdateFrameEnd();
    }

    public void DoUpdateFixed()
    {
        GameHandler.Instance.DoUpdateFixed();
    }
}