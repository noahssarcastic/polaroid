using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField] int targetFrameRate = 60;

    public void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
