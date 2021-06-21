using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : BaseMono
{
    public static FXManager Instance;

    public override void Initialize()
    {
        Instance ??= this;
    }
}
