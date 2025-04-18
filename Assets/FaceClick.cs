using UnityEngine;

public class FaceClick : MonoBehaviour
{
    public GameManager GameController;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                // Check if the clicked object is this face
                
                    // GameController.FaceClicked(gameObject);
                
            }
        }
    }
}
