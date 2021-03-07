using System.Collections;
using System.Collections.Generic;
using System;

public class NodeGene : Gene
{
    private double x, y;

    public NodeGene(int innovationNumber) : base(innovationNumber)
    {
        
    }

    public double GetX()
    {
        return x;
    }

    public double GetY()
    {
        return y;
    }

    public void SetX(double x)
    {
        this.x = x;
    }

    public void SetY(double y)
    {
        this.y = y;
    }

    public bool EqualsObject(Object o)
    {
        if (o.GetType() != typeof(NodeGene))
        {
            return false;
        }

        return innovationNumber == ((NodeGene)o).GetInnovationNumber();
    }

    public int HashCode()
    {
        return innovationNumber;
    }
}
