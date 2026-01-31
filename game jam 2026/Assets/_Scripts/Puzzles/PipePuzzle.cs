using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PipePuzzle : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        if (!IsUsable()) return;
        print("using");
    }
    public bool IsUsable() => true;
}
