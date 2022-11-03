using System.Collections.Generic;

namespace SharpIA.Search;

public interface IState
{
    IEnumerable<IState> NextMoves();
    float Avaliation { get; }
}