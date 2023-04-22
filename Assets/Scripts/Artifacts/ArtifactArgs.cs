using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactArgs : EventArgs
{
    public string ArtifactName;
    public string ArtifactDescription;

    public ArtifactArgs(string artifactName, string artifactDescription)
    {
        ArtifactName = artifactName;
        ArtifactDescription = artifactDescription;
    }
}
