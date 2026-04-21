using System;
using System.IO;
using EngineApp;

namespace EngineApp.SequentialSim;

public class SequentialSimulator
{
    private readonly GridConfig _config;
    private State[,] _currentGrid;
    private State[,] _nextGrid;

    public SequentialSimulator(GridConfig config)
    {
        _config = config;
        _currentGrid = new State[_config.Size, _config.Size];
        _nextGrid = new State[_config.Size, _config.Size];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        _currentGrid[_config.Size / 2, _config.Size / 2] = State.Infected;
    }

    public void Run(string runType = null)
    {
        for (int day = 0; day < _config.Days; day++)
        {
            int totalInfectedToday = 0;
            Random rand = new Random();

            for (int i = 0; i < _config.Size; i++)
            {
                for (int j = 0; j < _config.Size; j++)
                {
                    _nextGrid[i, j] = _currentGrid[i, j];

                    if (_currentGrid[i, j] == State.Infected)
                    {
                        double roll = rand.NextDouble();
                        if (roll < _config.RecoveryProb)
                            _nextGrid[i, j] = State.Removed;
                        else if (roll < _config.RecoveryProb + _config.DeathProb)
                            _nextGrid[i, j] = State.Removed;

                        totalInfectedToday++;
                    }
                    else if (_currentGrid[i, j] == State.Susceptible)
                    {
                        int infectedNeighbors = CountInfectedNeighbors(i, j);
                        if (infectedNeighbors > 0)
                        {
                            double infectionChance = 1 - Math.Pow(1 - _config.InfectionProb, infectedNeighbors);
                            if (rand.NextDouble() < infectionChance)
                            {
                                _nextGrid[i, j] = State.Infected;
                            }
                        }
                    }
                }
            }

            SwapGrids();

            if (runType != null)
            {
                SaveGridState(day, runType);
            }
        }
    }

    private int CountInfectedNeighbors(int x, int y)
    {
        int count = 0;
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int k = 0; k < 8; k++)
        {
            int nx = x + dx[k];
            int ny = y + dy[k];

            if (nx >= 0 && nx < _config.Size && ny >= 0 && ny < _config.Size)
            {
                if (_currentGrid[nx, ny] == State.Infected)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void SwapGrids()
    {
        var temp = _currentGrid;
        _currentGrid = _nextGrid;
        _nextGrid = temp;
    }

    private void SaveGridState(int day, string runType)
    {
        if (day % 5 != 0 && day != _config.Days - 1) return;

        string baseDir = "/Users/mori/RiderProjects/MonteCarloSIR/Data";
        string dir = Path.Combine(baseDir, runType);
        Directory.CreateDirectory(dir);
        string path = Path.Combine(dir, $"day_{day:D3}.bin");

        using (var fs = new FileStream(path, FileMode.Create))
        using (var bw = new BinaryWriter(fs))
        {
            for (int i = 0; i < _config.Size; i++)
            {
                for (int j = 0; j < _config.Size; j++)
                {
                    bw.Write((byte)_currentGrid[i, j]);
                }
            }
        }
    }
}
