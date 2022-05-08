using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOverlay : MonoBehaviour
{
    Slider HealthSlider;
    Controller EnemyController;
    // Text HealthText;
    
    // Start is called before the first frame update
    void Start()
    {
        EnemyController = gameObject.GetComponent<Controller>();
        Component[] sliderObjects = GetComponentsInChildren(typeof(Slider), true);
        // Component[] textObjects = GetComponentsInChildren(typeof(Text), true);

        foreach (Slider slider in sliderObjects) {
            switch (slider.name) {
                case "HealthSlider":
                    HealthSlider = slider; break;
            }
        }

        // foreach (Text text in textObjects) {
        //     switch (text.name) {
        //         case "HealthText":
        //             HealthText = text; break;
        //     }
        // }
    }

    void SetPercent(float percentage) {
        HealthSlider.value = percentage;
    }

    // void SetText(string text) {
    //     HealthText.text = text;
    // }

    // Update is called once per frame
    void Update()
    {
        float[] healthInfo = EnemyController.GetHealthInfo();
        SetPercent(healthInfo[0] / healthInfo[2]);
        // SetText("" + healthInfo[0] + " / " + healthInfo[2]);
    }
}
