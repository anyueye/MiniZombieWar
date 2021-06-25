public struct ImpactData
{
    private readonly int _hp;
    private readonly int _attack;
    private readonly int _defense;

    public ImpactData(int hp, int attack, int defense)
    {
        _hp = hp;
        _attack = attack;
        _defense = defense;
    }

    public int Hp => _hp;
    public int Attack => _attack;
    public int Defense => _defense;
}