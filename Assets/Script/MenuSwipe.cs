using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSwipe : MonoBehaviour
{
    public GameObject scrollBar;
    private float scrollPosition;
    private float[] position;

    private Scrollbar scrollbar;

    private void Swipe()
    {
        position = new float[transform.childCount];
        float distance = 1f / (position.Length - 1f);

        for (int i = 0; i < position.Length; i++)
        {
            position[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scrollPosition = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (scrollPosition < position[i] + distance / 2 && scrollPosition > position[i] - distance / 2)
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, position[i], 0.2f);
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), .1f);
                }
            }
        }

        for (int i = 0; i < position.Length; i++)
        {
            if (scrollPosition < position[i] + distance / 2 && scrollPosition > position[i] - distance / 2)
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), .2f);

                for (int a = 0; i < position.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.8f, 0.8f), .2f); ;
                    }
                }
            }
        }
    }

    private void Start() => scrollbar = GetComponent<Scrollbar>();
}
