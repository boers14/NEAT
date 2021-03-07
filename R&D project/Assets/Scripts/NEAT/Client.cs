using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Client : IComparable<Client>
{
    private Calculator calculator;

    private Genome genome;
    private double score;
    private Species species;

    private bool bestClient = false;

    public void GenerateCalculator()
    {
        calculator = new Calculator(genome);
    }

    public double[] Calculate(double[] input)
    {
        if (calculator == null)
        {
            GenerateCalculator();
        }

        return calculator.Calculate(input);
    }

    public double Distance(Client other)
    {
        return genome.Distance(other.GetGenome());
    }

    public void Mutate()
    {
        genome.Mutate();
    }

    public Calculator GetCalculator()
    {
        return calculator;
    }

    public Genome GetGenome()
    {
        return genome;
    }

    public double GetScore()
    {
        return score;
    }

    public Species GetSpecies()
    {
        return species;
    }

    public void SetGenome(Genome genome)
    {
        this.genome = genome;
    }

    public void SetScore(double score)
    {
        this.score = score;
    }

    public void SetSpecies(Species species)
    {
        this.species = species;
    }

    public int CompareTo(Client other)
    {
        if (GetScore() > other.GetScore())
        {
            return 1;
        }
        else if (GetScore() < other.GetScore())
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public bool isBestClient()
    {
        return bestClient;
    }

    public void setBestClient(bool bestClient)
    {
        this.bestClient = bestClient;
    }
}
