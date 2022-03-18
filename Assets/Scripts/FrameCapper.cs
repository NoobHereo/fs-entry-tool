using UnityEngine;

public class FrameCapper : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 144;
    }
}