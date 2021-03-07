using System.Collections;
using System.Collections.Generic;
using System;

public class ConnectionGene : Gene
{
    private NodeGene from;
    private NodeGene to;

    private double weight;
    private bool enabledGene = true;

    private int replaceIndex;

    public ConnectionGene(NodeGene from, NodeGene to)
    {
        this.from = from;
        this.to = to;
    }

    public NodeGene GetFrom()
    {
        return from;
    }

    public NodeGene GetTo()
    {
        return to;
    }

    public void SetFrom(NodeGene from)
    {
        this.from = from;
    }

    public void SetTo(NodeGene to)
    {
        this.to = to;
    }

    public double GetWeight()
    {
        return weight;
    }

    public void SetWeight(double weight)
    {
        this.weight = weight;
    }

    public bool IsEnabled()
    {
        return enabledGene;
    }

    public void SetEnabled(bool enabledGene)
    {
        this.enabledGene = enabledGene;
    }

    public int GetReplaceIndex()
    {
        return replaceIndex;
    }

    public void SetReplaceIndex(int replaceIndex)
    {
        this.replaceIndex = replaceIndex;
    }

    public bool EqualsGene(Object o)
    {
        if (o.GetType() != typeof(ConnectionGene))
        {
            return false;
        }

        ConnectionGene c = (ConnectionGene)o;
        return from.EqualsObject(c.from) && to.EqualsObject(c.to);
    }

    public int HashCode()
    {
        return from.GetInnovationNumber() * Neat.MAX_NODES + to.GetInnovationNumber();
    }
}
