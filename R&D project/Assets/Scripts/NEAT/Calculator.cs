using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Calculator
{
    private List<Node> inputNodes = new List<Node>();
    private List<Node> outputNodes = new List<Node>();
    private List<Node> hiddenNodes = new List<Node>();

    public Calculator(Genome g)
    {
        RandomHashSet<NodeGene> nodes = g.GetNodes();
        RandomHashSet<ConnectionGene> cons = g.GetConnections();

        Dictionary<int, Node> nodeDictionary = new Dictionary<int, Node>();

        foreach (NodeGene n in nodes.GetData())
        {
            Node node = new Node(n.GetX());
            nodeDictionary[n.GetInnovationNumber()] = node;

            if (n.GetX() <= 0.1f)
            {
                inputNodes.Add(node);
            }
            else if (n.GetX() >= 0.9f)
            {
                outputNodes.Add(node);
            }
            else
            {
                hiddenNodes.Add(node);
            }
        }

        hiddenNodes.Sort();

        foreach(ConnectionGene c in cons.GetData())
        {
            NodeGene from = c.GetFrom();
            NodeGene to = c.GetTo();

            Node nodeFrom = nodeDictionary[from.GetInnovationNumber()];
            Node nodeTo = nodeDictionary[to.GetInnovationNumber()];

            Connection con = new Connection(nodeFrom, nodeTo);
            con.SetWeight(c.GetWeight());
            con.SetEnabled(c.IsEnabled());

            nodeTo.GetConnections().Add(con);
        }
    }

    public double[] Calculate(double[] input)
    {
        if(input.Length != inputNodes.Count)
        {
            Debug.LogWarning("Data doenst fit");
        }

        for(int i = 0; i < inputNodes.Count; i++)
        {
            inputNodes[i].SetOutput(input[i]);
        }

        foreach(Node n in hiddenNodes)
        {
            n.Calculate();
        }

        double[] output = new double[outputNodes.Count];
        for (int i = 0; i < outputNodes.Count; i++)
        {
            outputNodes[i].Calculate();
            output[i] = outputNodes[i].GetOutput();
        }

        return output;
    }
}
