[System.Serializable]
public class CustomerProperty
{
    public CustomerTypeEnum type;
    public float[] lowertruevalue; // Willing to buy (satisfacory and reputation related)
    public float[] uppertruevalue;
    public float[] truevalue; // Actual willing to buy
    public float energy; // [0,100]  
    public float tipsratio; // Expected tips 
    public float eatingDuration; 
    public int satisfactory; // Manage prob , energy decay and eating speed 
    public int addreputation;
    public int minusreputation;

}
