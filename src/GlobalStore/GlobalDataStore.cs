using Godot;

namespace ArmyGenerator.GlobalStore;

public partial class GlobalDataStore : Node
{
    public static GlobalDataStore S;
    public override void _EnterTree()
    {
        S = this;
    }
    public override void _ExitTree()
    {
        S = null;
    }

    public GlobalDataStore()
    {
        fillOptions = new ArmyFillOptions();
    }

    private ArmyFillOptions fillOptions;

    public ArmyFillOptions GetFillOptions() => fillOptions;
}