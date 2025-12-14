using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    [SerializeField] private GameObject key;
    private bool isKeyPickedUp = false;
    public static PickUpKey Instance;
    public KeyType keyType;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            key.SetActive(true);
            // the keyNumber retrieve the value from the inspector
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isKeyPickedUp = true;
            key.SetActive(false);
            PlayerKeyInventory inventory = other.GetComponent<PlayerKeyInventory>();
            if (inventory != null)
            {
                inventory.AddKey(keyType);
                Destroy(gameObject);
            }
        }
    }

    public bool IsKeyPickedUp()
    {
        return isKeyPickedUp;
    }
}
