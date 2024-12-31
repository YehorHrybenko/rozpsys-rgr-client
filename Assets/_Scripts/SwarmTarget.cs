using UnityEngine;

public class SwarmTarget : MonoBehaviour
{
    private void Awake()
    {
        SwarmTargetManager.Register(this);
    }
}
