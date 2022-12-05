using UnityEngine;
using System.Collections;

public class ApplicationStart : MonoBehaviour
{
    public void Awake()
    {
        ApplicationHandler.Instance.LoadData();
        ApplicationHandler.Instance.DoLoadAwake();
    }

    public void Start()
    {
        ApplicationHandler.Instance.DoLoadStart();
    }

    public void Update()
    {
        ApplicationHandler.Instance.DoUpdateFrame();
    }

    public void FixedUpdate()
    {
        ApplicationHandler.Instance.DoUpdateFixed();
    }

    public void LateUpdate()
    {
        ApplicationHandler.Instance.DoUpdateFrameEnd();
    }
}