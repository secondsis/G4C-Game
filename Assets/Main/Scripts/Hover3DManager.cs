using UnityEngine;

public class Hover3DManager : MonoBehaviour
{
    GameObject lastHit;

    private void LateUpdate()
    {
        int layerMask = LayerMask.GetMask("Default"); // only hit these layers
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // If raycast hits an object
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
            GameObject newHit = hit.collider.gameObject;
            // if the object is new
            if (newHit != lastHit)
            {
                // If there was a lastHit object, tell that object that the mouse left
                if (lastHit != null)
                    lastHit.SendMessage("OnHoverExit", SendMessageOptions.DontRequireReceiver);
                // Debug.Log("Raycast found a new object! New: " + newHit + " Old: " + lastHit);
                // set the new hit to this object
                lastHit = newHit;
                lastHit.SendMessage("OnHoverEnter", SendMessageOptions.DontRequireReceiver);
                lastHit.SendMessage("OnHoverOver", SendMessageOptions.DontRequireReceiver);
            } else
            {
                // If it's the same object
                lastHit.SendMessage("OnHoverOver", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.white);

            // if no raycast, and there was a lastHit object
            if (lastHit != null)
            {
                // Debug.Log("Raycast found nothing!");
                // Tell the lastHit object that the mouse left
                lastHit.SendMessage("OnHoverExit", SendMessageOptions.DontRequireReceiver);
                // This hit is null.
                lastHit = null;
            }
        }
    }
}
