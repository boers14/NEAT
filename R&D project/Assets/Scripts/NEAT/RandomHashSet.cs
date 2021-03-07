using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RandomHashSet<T>
{
    private HashSet<T> set;
    private List<T> data;

    public RandomHashSet()
    {
        set = new HashSet<T>();
        data = new List<T>();
    }

    public bool Contains(T obj)
    {
        return set.Contains(obj);
    }

    public T RandomElement()
    {
        if (set.Count > 0)
        {
            int dataSize = Size();
            int randomData = UnityEngine.Random.Range(0, dataSize);
            return data[randomData];
        }

        return default;
    }

    public void AddSorted(Gene obj)
    {
        for (int i = 0; i < Size(); i++)
        {
            Gene gene = data[i] as Gene;
            int innovation = gene.GetInnovationNumber();
            if (obj.GetInnovationNumber() < innovation)
            {
                data.Insert(i, (T)Convert.ChangeType(obj, typeof(T)));
                set.Add((T)Convert.ChangeType(obj, typeof(T)));
                return;
            }
        }

        data.Add((T)Convert.ChangeType(obj, typeof(T)));
        set.Add((T)Convert.ChangeType(obj, typeof(T)));
    }

    public int Size()
    {
        return data.Count;
    }

    public void Add(T obj)
    {
        if (!set.Contains(obj))
        {
            set.Add(obj);
            data.Add(obj);
        }
    }

    public void Clear()
    {
        set.Clear();
        data.Clear();
    }

    public T Get(int index)
    {
        if (index < 0 || index >= Size())
        {
            return default;
        }
        return data[index];
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= Size())
        {
            return;
        }

        set.Remove(data[index]);
        data.Remove(data[index]);
    }

    public void Remove(T obj)
    {
        set.Remove(obj);
        data.Remove(obj);
    }

    public List<T> GetData()
    {
        return data;
    }
}
