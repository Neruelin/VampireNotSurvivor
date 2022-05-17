using UnityEngine;

public class PersistentData : MonoBehaviour {
    public static PersistentData Instance;

    void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }
}