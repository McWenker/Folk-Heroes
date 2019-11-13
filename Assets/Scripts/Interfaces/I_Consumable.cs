public interface I_Consumable
{   
    int ConsumeCharges { get; set; }
    bool RemoveOnConsume { get; set; }
    void Consume();    
}
