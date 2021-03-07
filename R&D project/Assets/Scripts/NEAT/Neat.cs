using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neat
{
    public static int MAX_NODES = (int)Mathf.Pow(2, 20);

    private double c1 = 1.45f, c2 = 1.45f, c3 = 1.45f;
    private double CP = 2.75f;

    private double weightShiftStrength = 0.5f;
    private double weightRandomStrength = 1;

    private double survivors = 0.75f;

    private double probabilityMutateLink = 0.2f;
    private double probabilityMutateNode = 0.2f;
    private double probabilityMutateWeightShift = 0.06f;
    private double probabilityMutateWeightRandom = 0.06f;
    private double probabilityMutateToggleLink = 0.01f;

    private int amountOfClientsNotEvolving = 2;

    private Dictionary<ConnectionGene, ConnectionGene> allConnections = new Dictionary<ConnectionGene, ConnectionGene>();
    private RandomHashSet<NodeGene> allNodes = new RandomHashSet<NodeGene>();

    private RandomHashSet<Client>  clients = new RandomHashSet<Client>();
    private RandomHashSet<Species> species = new RandomHashSet<Species>();

    private int inputSize, outputSize, maxClients;

    private double[] array;

    public Neat (int inputSize, int outputSize, int clients)
    {
        Reset(inputSize, outputSize, clients);
    }

    public void PrintSpecies()
    {
        Debug.Log("#########################################");
        foreach(Species s in species.GetData())
        {
            Debug.Log(s.GetName() + " " + s.GetScore() + " " + s.Size());
        }
    }

    public Genome EmptyGenome()
    {
        Genome g = new Genome(this);

        for(int i = 0; i < inputSize + outputSize; i++)
        {
            g.GetNodes().Add(GetNode(i + 1));
        }

        return g;
    } 

    public void Reset(int inputSize, int outputSize, int maxClients)
    {
        this.inputSize = inputSize;
        this.outputSize = outputSize;
        this.maxClients = maxClients;

        allConnections.Clear();
        allNodes.Clear();
        clients.Clear();

        for (int i = 0; i < inputSize; i++)
        {
            NodeGene n = GetNode();
            n.SetX(0.1f);
            n.SetY((i + 1) / (double)(inputSize + 1));
        }

        for (int i = 0; i < outputSize; i++)
        {
            NodeGene n = GetNode();
            n.SetX(0.9f);
            n.SetY((i + 1) / (double)(outputSize + 1));
        }

        for (int i = 0; i < maxClients; i++)
        {
            Client c = new Client();
            c.SetGenome(EmptyGenome());
            c.GenerateCalculator();
            clients.Add(c);
        }
    }

    public Client GetClient(int index)
    {
        return clients.Get(index);
    }

    public ConnectionGene GetConnection(ConnectionGene con)
    {
        ConnectionGene c = new ConnectionGene(con.GetFrom(), con.GetTo());
        c.SetInnovationNumber(con.GetInnovationNumber());
        c.SetWeight(con.GetWeight());
        c.SetEnabled(con.IsEnabled());

        return c;
    }

    public ConnectionGene GetConnection(NodeGene node1, NodeGene node2)
    {
        ConnectionGene connectionGene = new ConnectionGene(node1, node2);

        if (allConnections.ContainsKey(connectionGene))
        {
            connectionGene.SetInnovationNumber(allConnections[connectionGene].GetInnovationNumber());
        } else
        {
            connectionGene.SetInnovationNumber(allConnections.Count + 1);
            allConnections[connectionGene] = connectionGene;
        }

        return connectionGene;
    }

    public NodeGene GetNode()
    {
        NodeGene n = new NodeGene(allNodes.Size() + 1);
        allNodes.Add(n);
        return n;
    }

    public NodeGene GetNode(int id)
    {
        if (id <= allNodes.Size() && id >= 0)
        {
            return allNodes.Get(id - 1);
        }

        return GetNode();
    }

    public void Evolve()
    {
        GenSpecies();
        Kill();
        RemoveExtinctSpecies();
        Reproduce();
        Mutate();

        foreach(Client c in clients.GetData())
        {
            c.GenerateCalculator();
        }
    }

    private void GenSpecies()
    {
        foreach (Species s in species.GetData())
        {
            s.ResetSpecies();
        }

        foreach (Client c in clients.GetData())
        {
            if (c.GetSpecies() != null) continue;

            bool found = false;

            foreach (Species s in species.GetData())
            {
                if (s.Put(c))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                species.Add(new Species(c));
            }
        }

        foreach(Species s in species.GetData())
        {
            s.EvaluateScore();
        }
    }

    private void Kill()
    {
        foreach (Species s in species.GetData())
        {
            s.Kill(1 - survivors);
        }
    }

    private void RemoveExtinctSpecies()
    {
        for (int i = species.Size() - 1; i >= 0; i--)
        {
            if (species.Get(i).Size() <= 1)
            {
                species.Get(i).GoExtinct();
                species.Remove(i);
            }
        }
    }

    private void Reproduce()
    {
        RandomSelector<Species> selector = new RandomSelector<Species>();
        foreach(Species s in species.GetData())
        {
            selector.Add(s, s.GetScore());
        }

        foreach(Client c in clients.GetData())
        {
            if (c.GetSpecies() == null)
            {
                Species s = selector.RandomT();
                c.SetGenome(s.Breed());
                s.ForcePut(c);
            }
        }
    }

    private void Mutate()
    {
        foreach (Client c in clients.GetData())
        {
            if (!c.isBestClient())
            {
                c.Mutate();
            }
            c.SetScore(0);
        }
    }

    public void SetReplaceIndex(NodeGene node1, NodeGene node2, int index)
    {
        ConnectionGene con = new ConnectionGene(node1, node2);
        con.SetReplaceIndex(index);
    }

    public int GetReplaceIndex(NodeGene node1, NodeGene node2)
    {
        ConnectionGene con = new ConnectionGene(node1, node2);
        ConnectionGene data = null;

        bool contains = allConnections.ContainsKey(con);

        if (contains)
        {
            data = allConnections[con];
        }

        if (data == null)
        {
            return 0;
        }
        return data.GetReplaceIndex();
    }

    public double GetC1()
    {
        return c1;
    }

    public double GetC2()
    {
        return c2;
    }

    public double GetC3()
    {
        return c3;
    }

    public double GetWeightShiftStrength()
    {
        return weightShiftStrength;
    }

    public double GetWeightRandomStrength()
    {
        return weightRandomStrength;
    }

    public double GetProbabilityMutateLink()
    {
        return probabilityMutateLink;
    }

    public double GetProbabilityMutateNode()
    {
        return probabilityMutateNode;
    }

    public double GetProbabilityMutateToggleLink()
    {
        return probabilityMutateToggleLink;
    }

    public double GetProbabilityMutateWeightRandom()
    {
        return probabilityMutateWeightRandom;
    }

    public double GetProbabilityMutateWeightShift()
    {
        return probabilityMutateWeightShift;
    }

    public int GetOutputSize()
    {
        return outputSize;
    }

    public int GetInputSize()
    {
        return inputSize;
    }

    public double GetCP()
    {
        return CP;
    }

    public void SetCP(double CP)
    {
        this.CP = CP;
    }
}
