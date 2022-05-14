using UnityEngine;


public abstract class Item  : MonoBehaviour {

    public int id;
    public string name;
    [TextArea(minLines:2, maxLines:4)]
    public string description;
    public Sprite icon;

    public abstract void OnPickUp(Inventory inventory);

    public abstract void OnRemove(Inventory inventory);

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            OnPickUp(collision.gameObject.GetComponent<Inventory>());
            gameObject.SetActive(false);
        }
    }
}