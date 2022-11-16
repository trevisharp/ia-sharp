using SharpIA.Search;
using SharpIA.Search.Trees;
using System.Text;

SuperTicTacToeState initial = new SuperTicTacToeState();
var ia = initial.AlphaBetaPrunningSearch(true);
foreach (var x in ia.GetFullPath())
    Console.WriteLine(x);

public class SuperTicTacToeState : ITreeState
{
    private SuperTicTacToeState parent = null;
    private float avaliation = 0f;
    private byte[] dataState = new byte[9 * 9];
    private byte crr = 1;
    private int _i = -1;
    private int _j = -1;
    sbyte[] winInfo = new sbyte[9 * 8];
    byte[] fullWinInfo = new byte[9];

    public ITreeState Parent => parent;

    public float Avaliation => avaliation;

    public IEnumerable<ITreeState> NextMoves()
    {
        int si = 0, sj = 0, ei = 9, ej = 9;
        if (_i != -1 && _j != -1)
        {
            si = 3 * _i;
            ei = si + 3;
            sj = 3 * _j;
            ej = sj + 3;
        }
        for (int j = sj; j < ej; j++)
        {
            for (int i = si; i < ei; i++)
            {
                var move = Move(i, j);
                if (move == null)
                    continue;
                yield return move;
            }
        }
    }

    public SuperTicTacToeState Move(int i, int j)
    {
        if (dataState[i + 9 * j] != 0)
            return null;
        
        if (this.avaliation == float.NegativeInfinity)
            return null;
        
        if (this.avaliation == float.PositiveInfinity)
            return null;
        
        SuperTicTacToeState newState = new SuperTicTacToeState();

        newState.parent = this;

        for (int k = 0; k < 81; k++)
            newState.dataState[k] = this.dataState[k];
        
        for (int k = 0; k < 72; k++)
            newState.winInfo[k] = this.winInfo[k];
        
        for (int k = 0; k < 9; k++)
            newState.fullWinInfo[k] = this.fullWinInfo[k];
        
        newState.dataState[i + 9 * j] = this.crr;

        newState.crr = (byte)(3 - this.crr);
        
        int _i = i / 3,
            _j = j / 3;
        
        int pi = i % 3,
            pj = j % 3;
        
        int baseIndex = 8 * (_i + 3 * _j);
        sbyte contribuition = (sbyte)(2 * (this.crr - 1) - 1);
        newState.winInfo[baseIndex + pi] += contribuition;
        newState.winInfo[baseIndex + 3 + pj] += contribuition;
        if (pi == pj)
            newState.winInfo[baseIndex + 6] += contribuition;
        else if (pi + pj == 2)
            newState.winInfo[baseIndex + 7] += contribuition;
        
        for (int k = baseIndex; k < baseIndex + 8; k++)
        {
            if (newState.winInfo[k] > 2)
            {
                newState.fullWinInfo[_i + 3 * _j] = 2;
                break;
            }
            else if (newState.winInfo[k] < -2)
            {
                newState.fullWinInfo[_i + 3 * _j] = 1;
                break;
            }
        }

        if (newState.fullWinInfo[_i + 3 * _j] != 0)
        {
            newState._i = -1;
            newState._j = -1;
        }
        else
        {
            newState._i = i;
            newState._j = j;
        }

        newState.avaliate();

        return newState;
    }

    private void avaliate()
    {
        this.avaliation = compute();
    }

    private float compute()
    {
        float value = testWin();
        if (value != 0f)
            return value;
        
        return computeHeuristic();
    }

    private float computeHeuristic()
    {
        float value = 0f;
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                float subValue = -2 * (fullWinInfo[i + 3 * j] - 1) + 1;
                if (j == 1)
                    subValue *= 1.414f;
                if (i == 1)
                    subValue *= 1.414f;
                value += subValue;
            }
        }
        return value;
    }

    private float testWin()
    {
        for (int j = 0; j < 3; j++)
        {
            if (fullWinInfo[3 * j] == 1 && 
                fullWinInfo[3 * j + 1] == 1 && 
                fullWinInfo[3 * j + 2] == 1)
                return float.PositiveInfinity;
            else if (
                fullWinInfo[3 * j] == 2 && 
                fullWinInfo[3 * j + 1] == 2 && 
                fullWinInfo[3 * j + 2] == 2)
                return float.NegativeInfinity;
        }
        for (int i = 0; i < 3; i++)
        {
            if (fullWinInfo[0 + i] == 1 && 
                fullWinInfo[3 + i] == 1 && 
                fullWinInfo[6 + i] == 1)
                return float.PositiveInfinity;
            else if (
                fullWinInfo[0 + i] == 2 && 
                fullWinInfo[3 + i] == 2 && 
                fullWinInfo[6 + i] == 2)
                return float.NegativeInfinity;
        }
        if (fullWinInfo[0 + 0] == 1 && 
            fullWinInfo[3 + 1] == 1 && 
            fullWinInfo[6 + 2] == 1)
            return float.PositiveInfinity;
        else if (
            fullWinInfo[0 + 0] == 2 && 
            fullWinInfo[3 + 1] == 2 && 
            fullWinInfo[6 + 2] == 2)
            return float.NegativeInfinity;
            
        if (fullWinInfo[0 + 2] == 1 && 
            fullWinInfo[3 + 1] == 1 && 
            fullWinInfo[6 + 0] == 1)
            return float.PositiveInfinity;
        else if (
            fullWinInfo[0 + 2] == 2 && 
            fullWinInfo[3 + 1] == 2 && 
            fullWinInfo[6 + 0] == 2)
            return float.NegativeInfinity;
        
        return 0f;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("╔═╤═╤═╦═╤═╤═╦═╤═╤═╗");
        for (int j = 0; j < 9; j++)
        {
            builder.Append("║");

            for (int i = 0; i < 9; i++)
            {
                int index = i + 9 * j;
                var intValue = dataState[index];
                var strValue = intValue == 0 ? " " :
                    (intValue == 1 ? "X" : "O");
                
                builder.Append(strValue);
                if ((i + 1) % 3 != 0)
                    builder.Append("│");
                else builder.Append("║");
            }
            builder.AppendLine();

            if ((j + 1) % 3 != 0)
                builder.AppendLine("╟─┼─┼─╫─┼─┼─╫─┼─┼─╢");
            else if (j < 8) 
                builder.AppendLine("╠═╪═╪═╫═╪═╪═╫═╪═╪═╣");
        }
        builder.Append("╚═╧═╧═╩═╧═╧═╩═╧═╧═╝");

        return builder.ToString();
    }
}