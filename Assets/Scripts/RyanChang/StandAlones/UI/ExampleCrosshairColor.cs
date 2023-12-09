using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleCrosshairColor : MonoBehaviour
{
    [SerializeField] private Image crosshair;
    [SerializeField] private Slider RSlider;
    [SerializeField] private Slider GSlider;
    [SerializeField] private Slider BSlider;
    public Texture2D test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        crosshair.color = new Color(RSlider.value, GSlider.value, BSlider.value);
    }

    public void SetCrosshair()
    {
        Texture2D texture = new Texture2D(32, 32);

        for (int i = 0; i<=test.width; i++)
        {
            for(int j = 0; j<=test.height; j++)
            {
                if (test.GetPixel(i, j).a != 0)
                {
                    texture.SetPixel(i, j, new Color(RSlider.value, GSlider.value, BSlider.value, test.GetPixel(i, j).a));
                }
                else
                {
                    texture.SetPixel(i, j, new Color(0,0,0,0));
                }
            }
        }

        Cursor.SetCursor(texture, new Vector2(15, 15), CursorMode.Auto);
        
    }
}
