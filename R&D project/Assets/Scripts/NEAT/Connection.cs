using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    private Node from;
    private Node to;

    private double weight;
    private bool enabledGene = true;

    public Connection(Node from, Node to)
    {
        this.from = from;
        this.to = to;
    }

    public Node GetFrom()
    {
        return from;
    }

    public Node GetTo()
    {
        return to;
    }

    public void SetFrom(Node from)
    {
        this.from = from;
    }

    public void SetTo(Node to)
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
}
