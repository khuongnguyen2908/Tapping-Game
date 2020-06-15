using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemy : MonoBehaviour
{
    private SpriteRenderer rend;
    private Sprite a1, a2;

    // Start is called before the first frame update
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        a1 = Resources.Load<Sprite>("001");
        a2 = Resources.Load<Sprite>("002");
        rend.sprite = a1;
    }

    // Update is called once per frame
    private void Update()
    {
        if( Input.GetMouseButtonDown(0))
        {
            if (rend.sprite == a1)
                rend.sprite = a2;
            else if (rend.sprite == a2)
                rend.sprite = a1;
        }
    }
}
