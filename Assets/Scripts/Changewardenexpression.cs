using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Changewardenexpression : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprite;
    private Collider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider>(); 
    }

    public void DisableCollider()
    {
        boxCollider.enabled = false;

    }

    public void WardenExpressionSmile(bool isSmile)
    {
        if (isSmile)
        {
            spriteRenderer.sprite = sprite[1];
        }
        else
        {
            spriteRenderer.sprite = sprite[0];
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
