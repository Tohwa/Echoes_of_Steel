using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AState
{
    virtual public void UpdateState() { }
    virtual public void EnterState() { }
    virtual public void ExitState() { }
 }
