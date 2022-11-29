using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private Vector2 origin;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        origin = boxCollider.bounds.min + new Vector3(0, 1.105f);
        distance = boxCollider.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, distance, 1 << LayerMask.NameToLayer("Player"));
        if (hit)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
        Debug.DrawRay(origin, Vector3.right * distance, Color.red);
    }
}
