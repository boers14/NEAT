using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable<Node>
{
    private double x;
    private double output;
    private List<Connection> connections = new List<Connection>();

    public Node(double x)
    {
        this.x = x;
    }

    public void Calculate()
    {
        double s = 0;
        foreach(Connection c in connections)
        {
            if (c.IsEnabled())
            {
                s += c.GetWeight() * c.GetFrom().GetOutput();
            }
        }

        output = ActivationFunction(s);
    }

    private double ActivationFunction(double x)
    {
        return 1d / (1 + Mathf.Exp((float)-x));
    }

    public void SetX(double x)
    {
        this.x = x;
    }

    public void SetOutput(double output)
    {
        this.output = output;
    }

    public void SetConnections(List<Connection> connections)
    {
        this.connections = connections;
    }

    public double GetX()
    {
        return x;
    }

    public double GetOutput()
    {
        return output;
    }

    public List<Connection> GetConnections()
    {
        return connections;
    }

    public int CompareTo(Node other)
    {
        if (x > other.x) return -1;
        if (x < other.x) return 1;
        return 0;
    }
}
