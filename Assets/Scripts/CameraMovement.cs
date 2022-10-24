using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    Camera Cam;

    float OriginalSize = 5;

    public float MaxSize;

    Vector3 MoveDir;

    public Transform canvasTransform;

    public float MoveScale;

    // Start is called before the first frame update
    void Start()
    {
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");
        float Z = Input.mouseScrollDelta.y;

        MoveDir.x = X;
        MoveDir.y = Y;
        MoveDir /= 1.5f;
        Cam.orthographicSize += Z;

        Cam.orthographicSize = Mathf.Clamp(Cam.orthographicSize, 2, MaxSize);

        float MoveLerp = (Cam.orthographicSize - 2) / 18;

        MoveScale = Mathf.Lerp(40, 4, MoveLerp);

        transform.position += MoveDir / MoveScale;

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if(hit.collider != null)
            {
                Container container = hit.collider.gameObject.GetComponent<Container>();

                if(container != null && container.inventoryPrefab != null)
                {
                    container.SpawnInventory(canvasTransform);
                }
            }
        }
    }
}
