using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientsMenu : MonoBehaviour
{
    [SerializeField]
    private Manager manager;

    private List<string> clients = new List<string>();
    private List<Text> weights = new List<Text>();

    private Dropdown clientsDropdown;

    private float canvasWidth, canvasHeight;

    [SerializeField]
    private float lineHeight = 5;

    [SerializeField]
    private int amountOfDigits = 4;

    [SerializeField]
    private Image line, oval;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Canvas canvas;

    private List<Image> images = new List<Image>();

    public void FillDropDown()
    {
        clientsDropdown = GetComponent<Dropdown>();

        clientsDropdown.ClearOptions();
        for (int i = 0; i < manager.GetPopulationSize(); i++)
        {
            int actualNumber = i + 1;
            string clientName = "Client " + actualNumber.ToString();
            clients.Add(clientName);
        }

        clientsDropdown.AddOptions(clients);
        clientsDropdown.onValueChanged.AddListener(ShowClient);

        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
    }

    private void ShowClient(int setting)
    {
        for (int i = 0; i < images.Count; i++)
        {
            Destroy(images[i].gameObject);
        }
        images.Clear();

        for (int i = 0; i < weights.Count; i++)
        {
            Destroy(weights[i].gameObject);
        }
        weights.Clear();

        Genome genome = manager.GetNeat().GetClient(setting).GetGenome();

        foreach(ConnectionGene c in genome.GetConnections().GetData())
        {
            DrawConnection(c);
        }

        foreach (NodeGene n in genome.GetNodes().GetData())
        {
            DrawNode(n);
        }
    }

    private void DrawConnection(ConnectionGene c)
    {
        Image connection = Instantiate(line);
        RectTransform connectionTransform = connection.GetComponent<RectTransform>();

        double lineWidth = ((c.GetTo().GetX() - c.GetFrom().GetX()) * canvasWidth);

        connectionTransform.sizeDelta = new Vector2((float)lineWidth, lineHeight);

        Vector3 pos = new Vector3(((float)c.GetFrom().GetX() * canvasWidth) + ((float)lineWidth / 2), 
            (float)(c.GetFrom().GetY() + c.GetTo().GetY()) / 2 * canvasHeight, 0);

        connection.transform.position = pos;

        Vector3 lookAt = new Vector3((float)c.GetTo().GetX() * canvasWidth, (float)c.GetTo().GetY() * canvasHeight, 0);
        float angleRad = Mathf.Atan2(lookAt.y - connection.transform.position.y, lookAt.x - connection.transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;

        connection.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        if (c.IsEnabled())
        {
            connection.color = Color.blue;
        }
        else
        {
            connection.color = Color.red;
        }

        connection.transform.SetParent(canvas.transform);
        images.Add(connection);

        Text weight = Instantiate(text);

        weight.transform.position = pos;
        float digits = Mathf.Pow(10.0f, amountOfDigits);
        float weightNumber = Mathf.Round((float)c.GetWeight() * digits) / digits;
        weight.text = weightNumber.ToString();
        weight.color = Color.white;

        weight.transform.SetParent(canvas.transform);
        weights.Add(weight);
    }

    private void DrawNode(NodeGene n)
    {
        Image node = Instantiate(oval);

        Vector3 pos = new Vector3((float)n.GetX() * canvasWidth, (float)n.GetY() * canvasHeight, 0);

        node.transform.position = pos;

        node.color = Color.gray;

        node.transform.SetParent(canvas.transform);
        images.Add(node);
    }

    public void changeActiveState(bool activate)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].gameObject.SetActive(activate);
        }

        for (int i = 0; i < weights.Count; i++)
        {
            weights[i].gameObject.SetActive(activate);
        }
    }
}
