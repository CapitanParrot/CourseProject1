using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtifact
{

    string Name { get; }
    string Description { get; }
    void Action(Managers args);

    void UndoAction(Managers args);
}
