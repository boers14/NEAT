using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector<T>
{
    private List<T> objects = new List<T>();
    private List<double> scores = new List<double>();

    private double totalScore = 0;

    public void Add(T element, double score)
    {
        objects.Add(element);
        scores.Add(score);
        totalScore += score;
    }

    public T RandomT()
    {
        double v = Random.Range(0f, (float)totalScore);
        double c = 0;

        for (int i = 0; i < objects.Count; i++)
        {
            c += scores[i];
            if (c >= v)
            {
                return objects[i];
            }
        }

        return default;
    }

    public void Reset()
    {
        objects.Clear();
        scores.Clear();
        totalScore = 0;
    }
}
