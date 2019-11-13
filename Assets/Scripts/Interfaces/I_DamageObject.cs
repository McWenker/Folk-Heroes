public interface I_DamageObject
{
    int Damage { get; set; }
    int DamageCooldown { get; set; }
    //DamageType DamType { get; set; }
    void InflictDamage();
    void CheckWhoToDamage();
}
