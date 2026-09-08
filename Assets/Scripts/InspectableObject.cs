using UnityEngine;

public class InspectableObject : MonoBehaviour
{
    [Header("조사 내용")]
    [TextArea(2, 5)]
    [SerializeField] private string message;

    [Header("공포 텍스트")]
    [SerializeField] private bool useHorrorText = false;


    public string GetMessage()
    {
        return message;
    }


    public bool UseHorrorText()
    {
        return useHorrorText;
    }
}