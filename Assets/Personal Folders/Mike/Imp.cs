using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Imp : MonoBehaviour, IInteractable
{
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (!collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetCurrentInteractable(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetCurrentInteractable(null);
        }
    }

    public void Interact()
    {
        
    }
}
