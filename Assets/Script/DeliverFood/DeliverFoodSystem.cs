using UnityEngine;

//this script is attached to the player to deal with delivering food to customers
public class DeliverFoodSystem : MonoBehaviour
{
    private string customerTag = "Customer";
    private GameObject customer;

    public int pickedUpIdx = -1;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("pick up idx: " + pickedUpIdx);
        if (customer != null && pickedUpIdx != -1)
        {
            Customer cus = customer.GetComponent<Customer>();
            if (pickedUpIdx == cus.GetOrderedFoodIndex())
            {
                foreach (Transform child in gameObject.transform)
                {
                    if (child.CompareTag("FoodItem"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                cus.DoneDining();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(customerTag))
        {
            customer = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(customerTag))
        {
            customer = null;
        }
    }

}
