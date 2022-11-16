using System.Collections.Generic;

namespace SharpIA.Search.Trees;

public interface ITreeState : IState
{
    IEnumerable<ITreeState> NextMoves();
    ITreeState Parent { get; }
    float Avaliation { get; }
}