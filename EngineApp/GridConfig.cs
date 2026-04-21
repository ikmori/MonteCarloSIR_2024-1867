namespace EngineApp;

public class GridConfig
{
    public int Size { get; set; } = 1000;
    public int Days { get; set; } = 365;
    public double InfectionProb { get; set; } = 0.3;
    public double RecoveryProb { get; set; } = 0.1;
    public double DeathProb { get; set; } = 0.02;
}