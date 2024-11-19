using UnityEngine;

public abstract class PickUpItem : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSound;


    private void OnTriggerEnter2D(Collider2D other) 
    {
        OnPickedUp(other.gameObject);

        if(pickUpSound != null) SoundManager.PlayClip(pickUpSound);

        Destroy(gameObject);
    }

    protected abstract void OnPickedUp(GameObject go);
}
