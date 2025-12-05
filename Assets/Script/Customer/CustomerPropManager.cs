using UnityEngine;

public class CustomerPropertyManager : MonoBehaviour
{
    public CustomerProperty[] customerProperties;

    public int NiceCustomer = 0;
    public int BadCustomer = 0;
    public int TotalCustomer = 0;

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

    public CustomerProperty Specialpropertypositive(int Good)
    {
        CustomerProperty special = customerProperties[6];
        for (int i = 0; i < special.truevalue.Length; i++)
        {
            float upperBound = special.uppertruevalue[i] + Good * 5;
            float lowerBound = special.lowertruevalue[i] + Good * 5;
            special.truevalue[i] = Random.Range(lowerBound, upperBound);
        }
        special.eatingDuration = 1f;
        special.energy = 60f + Good * 1;
        special.tipsratio = 0.2f + Good * 0.01f;
        special.addreputation = Good * 1;
        special.minusreputation = 0;
        return special;
    }

    public CustomerProperty Specialpropertynegative(int Bad)
    {
        CustomerProperty special = customerProperties[6];
        for (int i = 0; i < special.truevalue.Length; i++)
        {
            float upperBound = special.uppertruevalue[i] + Bad * 10;
            float lowerBound = special.lowertruevalue[i] + Bad * 10;
            special.truevalue[i] = Random.Range(lowerBound, upperBound);
        }
        special.eatingDuration = 15f;
        special.energy = 100f - Bad * 1;
        special.tipsratio = 1f + Bad * 0.05f;
        special.addreputation = 0;
        special.minusreputation = Bad;
        return special;
    }

    public void Addsatisfactory(CustomerProperty prop, int number)
    {
        prop.satisfactory = Mathf.Clamp(prop.satisfactory + number, 0, 5);

    }

    public void Updateeveryone(float multiplier, string name, bool normal)
    {
        float applyMultiplier = normal ? multiplier : (1f / multiplier);

        foreach (var prop in customerProperties)
        {
            switch (name)
            {
                case "Price":
                    for (int i = 0; i < prop.lowertruevalue.Length; i++)
                    {
                        prop.uppertruevalue[i] *= applyMultiplier;
                        prop.lowertruevalue[i] *= applyMultiplier;
                    }
                    break;

                case "EatingSpeed":
                    prop.eatingDuration *= applyMultiplier;
                    break;

                case "MovingSpeed":
                    prop.maxspeed *= applyMultiplier;
                    break;

                case "Energy":
                    prop.energy *= applyMultiplier;
                    break;

                case "TipsRatio":
                    prop.tipsratio *= applyMultiplier;
                    break;

                case "Reputation":
                    prop.addreputation = (int)(prop.addreputation * applyMultiplier);
                    prop.minusreputation = (int)(prop.minusreputation * applyMultiplier);
                    break;

                case "Satisfactory":
                    prop.satisfactory = normal
                        ? (int)(prop.satisfactory + multiplier)
                        : (int)(prop.satisfactory - multiplier);
                    break;

                default:
                    Debug.LogWarning($"Unknown property name: {name}");
                    break;
            }
        }

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
