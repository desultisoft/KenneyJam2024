using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Material highlightMaterial;
    private IInteractable currentInteractable;
    private Material originalMaterial;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentInteractable(IInteractable interactable)
    {
        // Reset the material of the previous interactable
        if (currentInteractable != null)
        {
            SpriteRenderer previousRenderer = currentInteractable.gameObject.GetComponent<SpriteRenderer>();
            if (previousRenderer != null && originalMaterial != null)
            {
                previousRenderer.material = originalMaterial;
            }
        }

        // Set the new interactable
        currentInteractable = interactable;

        // Apply the highlight material to the new interactable
        if (currentInteractable != null)
        {
            SpriteRenderer renderer = currentInteractable.gameObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
                renderer.material = new Material(highlightMaterial);
            }
        }

        
    }
}
