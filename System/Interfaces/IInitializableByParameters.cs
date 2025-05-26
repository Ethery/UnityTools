using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializableByParameters<PARAMS_TYPE>
{
    public PARAMS_TYPE Parameters { get; }
    public void Initialize(PARAMS_TYPE parameters);
}
