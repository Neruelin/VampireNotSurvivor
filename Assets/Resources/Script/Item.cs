using UnityEngine;


public abstract class Item  : MonoBehaviour {

    public string id;
    public string name;
    public string description;
    public Sprite icon;

    public abstract void Add(Inventory inventory);

    public abstract void Remove(Inventory inventory);

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            Add(collision.gameObject.GetComponent<Inventory>());
            gameObject.SetActive(false);
        }
    }
}