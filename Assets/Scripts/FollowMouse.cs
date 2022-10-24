using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    private Vector2 Offset = new Vector2(0.5f, -0.5f);

    public List<SpriteRenderer> Sprites = new List<SpriteRenderer>();

    public Color32 ClearColor = new Color32(255, 255, 255, 255);
    public Color32 NotClearColor = new Color32(255, 0, 0, 255);

    public bool IsColliding;

    public List<ObjectTag> CollidingObjects = new List<ObjectTag>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        ObjectTag HitTag = other.GetComponent<ObjectTag>();

        if(HitTag != null)
        {
            if(HitTag.Obstacle)
            {
                CollidingObjects.Add(HitTag);
                IsColliding = true;
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        

        ObjectTag HitTag = other.GetComponent<ObjectTag>();

        if(HitTag != null)
        {
            if (CollidingObjects.Contains(HitTag))
            {
                CollidingObjects.Remove(HitTag);
                if(CollidingObjects.Count == 0) IsColliding = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePos = Input.mousePosition;

        Vector2 NewPos = Camera.main.ScreenToWorldPoint(MousePos);

        NewPos.x = Mathf.RoundToInt(NewPos.x);
        NewPos.y = Mathf.RoundToInt(NewPos.y);

        

        transform.position = NewPos;

        if(!IsColliding) foreach (SpriteRenderer s in Sprites) s.color = ClearColor;

        else foreach (SpriteRenderer s in Sprites) s.color = NotClearColor;

        if(Input.GetKeyDown(KeyCode.R)) transform.eulerAngles += new Vector3(0, 0, 90);

    }
}
