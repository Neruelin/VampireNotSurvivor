using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button play;
    // Start is called before the first frame update
    void Start()
    {
        play.onClick.AddListener(HandlePlayClick);
    }

    void HandlePlayClick() {
        SceneManager.LoadScene(1);
    }
}
