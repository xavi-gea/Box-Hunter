using UnityEngine;

/// <summary>
/// When this is enabled, dont destroy the parent gameObject
/// </summary>
public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
