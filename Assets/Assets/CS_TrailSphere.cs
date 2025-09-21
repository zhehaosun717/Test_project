using UnityEngine;

/// <summary>
/// Manages the lifecycle of a trail sphere, ensuring it is destroyed after a set time
/// and that it has a unique material instance.
/// </summary>
public class CS_TrailSphere : MonoBehaviour
{
    [Tooltip("The lifetime of this sphere in seconds.")]
    public float lifetime = 5f;

    void Start()
    {
        Renderer sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            // Create a new material instance for this sphere. This is necessary 
            // because the PlayerController sets a unique, ever-changing color for each sphere. 
            // Without this, all spawned spheres would change color whenever a new one is created.
            sphereRenderer.material = new Material(sphereRenderer.material);
        }
        
        // Schedule the destruction of this game object after 'lifetime' seconds.
        Destroy(gameObject, lifetime);
    }
}
