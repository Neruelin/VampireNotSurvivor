using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayScript : MonoBehaviour
{
    private ProgressBar Health;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Health = root.Q<ProgressBar>("Health");
    }

    // Update is called once per frame
    void Update()
    {
        Health.value = player.GetComponent<Controller>().currentHealth;
    }
}
