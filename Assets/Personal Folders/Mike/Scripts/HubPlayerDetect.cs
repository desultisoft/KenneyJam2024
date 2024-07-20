using System;
using System.Linq;
using UnityEngine;

public class HubPlayerDetect : MonoBehaviour
{
    public float radius;
    public ItemSlot targetSlot;
    public Interactable targetItem;

    public void Update()
    {
        targetSlot = FindClosestObject<ItemSlot>();
        targetItem = FindClosestObject<Interactable>();
    }

    T FindClosestObject<T>() where T : Component
    {
        Vector2 position = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius);

        if (hits.Length == 0)
        {
            return null;
        }

        T closestComponent = hits
            .Where(hit => hit.gameObject != gameObject)
            .Select(hit => hit.GetComponent<T>())
            .Where(component => component != null)
            .OrderBy(component => Vector2.Distance(position, component.transform.position))
            .FirstOrDefault();

        return closestComponent;
    }

    void OnDrawGizmosSelected()
    {
        // Draw the circle in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
