using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    protected int innovationNumber;

    public Gene(int innovationNumber)
    {
        this.innovationNumber = innovationNumber;
    }

    public Gene()
    {

    }

    public int GetInnovationNumber()
    {
        return innovationNumber;
    }

    public void SetInnovationNumber(int innovationNumber)
    {
        this.innovationNumber = innovationNumber;
    }
}
