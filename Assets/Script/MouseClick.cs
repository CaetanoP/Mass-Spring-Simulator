
using UnityEngine;
public class MouseClick : MonoBehaviour
{
    [SerializeField]
     private Camera mainCamera; // Reference to the main camera in the scene
    private Vector3 mousePosition; // Stores the position of the mouse in world space
    public Simulator simuladorScript; // Reference to a Simulator script attached to the game object
    void Update()
    {
        // Casts a ray from the camera to the mouse position and checks if it hits anything
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            // Updates the mousePosition with the hit point of the raycast
            mousePosition = hit.point;
            // Retrieves the position of the object hit by the raycast
            Vector3 boxPosition = hit.collider.gameObject.transform.position;

            // Checks if the right mouse button (button 1) is pressed
            if (Input.GetMouseButton(1))
            {
                // Switches based on the name of the hit object
                switch (hit.collider.gameObject.name)
                {
                    case "BlocoL": // If the object is named "BlocoL"
                        // Sets the first position to the mouse position
                        simuladorScript.positions[0] = new Vector3(mousePosition.x, boxPosition.y, boxPosition.z);
                        // Sets the second position relative to the first, using a predefined offset in x
                        simuladorScript.positions[1] = new Vector3(mousePosition.x + simuladorScript.x_0[1], boxPosition.y, boxPosition.z);
                        break;
                    case "BlocoR": // If the object is named "BlocoR"
                        // Sets the first position to the left of the mouse position, using a predefined offset in x
                        simuladorScript.positions[0] = new Vector3(mousePosition.x - simuladorScript.x_0[1], boxPosition.y, boxPosition.z);
                        // Sets the second position to the mouse position
                        simuladorScript.positions[1] = new Vector3(mousePosition.x, boxPosition.y, boxPosition.z);
                        break;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                switch (hit.collider.gameObject.name)
                {
                    case "BlocoL":
                        simuladorScript.positions[0] = new Vector3(mousePosition.x, boxPosition.y, boxPosition.z);
                        break;
                    case "BlocoR":
                        simuladorScript.positions[1] = new Vector3(mousePosition.x, boxPosition.y, boxPosition.z);
                        break;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                switch (hit.collider.gameObject.name)
                {
                    case "BlocoL":
                        simuladorScript.positions[0] = new Vector3(mousePosition.x, boxPosition.y, boxPosition.z);
                        simuladorScript.positions[1] = new Vector3(simuladorScript.partitionWall[1].x - (mousePosition.x - simuladorScript.partitionWall[0].x), boxPosition.y, boxPosition.z);
                        break;
                    case "BlocoR":
                        simuladorScript.positions[1] = new Vector3(mousePosition.x, boxPosition.y, boxPosition.z);
                        simuladorScript.positions[0] = new Vector3(simuladorScript.partitionWall[0].x + (-mousePosition.x + simuladorScript.partitionWall[1].x), boxPosition.y, boxPosition.z);
                        break;
                }
            }

        }
    }
}
