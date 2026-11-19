using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class HashTableBucketView : MonoBehaviour
{
    public Image backgroundImage;
    public TMP_Text indexText;
    public TMP_Text contentText;

    private int bucketIndex;

    private static readonly Color EmptyColor = Color.grey;
    private static readonly Color OccupiedColor = Color.green;
    private static readonly Color DeletedColor = Color.orange;

    public void SetIndex(int index)
    {
        bucketIndex = index;
        indexText.text = $"I : {bucketIndex}";
    }

    public void SetEmpty()
    {
        backgroundImage.color = EmptyColor;
        contentText.text =  string.Empty;
    }
    public void SetOccupied(string key, string value)
    {
        backgroundImage.color = OccupiedColor;
        contentText.text = $"K: {key}\t\t V: {value}";
    }

    public void SetDeleted()
    {
        backgroundImage.color = DeletedColor;
        contentText.text =  "(deleted)";
    }

    public  void SetChainedItems(List<string> items)
    {
        if(items == null || items.Count == 0)
        {
            SetEmpty();
            return;
        }

        backgroundImage.color = OccupiedColor;
        contentText.text = string.Join(" -> ", items);
    }
}
