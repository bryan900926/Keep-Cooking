using UnityEngine;

public class CustomerPropertyManager : MonoBehaviour
{
    public CustomerProperty[] customerProperties;
    public static CustomerPropertyManager Instance { get; private set; }
    //public CustomerStateManager customerStateManager;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }   

    public CustomerProperty GetPropertyByTypeNumber(int Number)
    {
        foreach (var prop in customerProperties)
            if ((int)prop.type == Number) return prop;
        return null;
    }

    public void Addsatisfactory(CustomerProperty prop, int number)
    {
        prop.satisfactory += (int)number;
    }

    public void Updateprop(CustomerProperty prop)
    {   
        int satisfactory = prop.satisfactory;
        float reputation = ReputationSystem.Instance.GetReputationLevel();
        // Update truevalue based on satisfactory and reputation
        foreach (var customerProp in customerProperties)
        {
            if (customerProp.type == prop.type)
            {
                if (customerProp.truevalue == null || customerProp.truevalue.Length != customerProp.lowertruevalue.Length)
                {
                    customerProp.truevalue = new float[customerProp.lowertruevalue.Length];
                }
                for (int i = 0; i < customerProp.truevalue.Length; i++)
                {
                    float lowerBound = customerProp.lowertruevalue[i]* (1 + reputation/200) + satisfactory * 10;
                    float upperBound = customerProp.uppertruevalue[i]* (1 + reputation/100);
                    customerProp.truevalue[i] = Random.Range(lowerBound, upperBound);

                }
                Debug.Log($"Updated truevalue for {customerProp.type}: [{string.Join(", ", customerProp.truevalue)}]");


            }
        }


    }


}
