using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReadSign : MonoBehaviour
{
    [SerializeField] private GameObject SpeakingMenu;
    [SerializeField] private GameObject SpeakerName;
    [SerializeField] private GameObject Dialogue;
    [SerializeField] private string SpeakingName;
    [SerializeField] private string SpeakingDialogue;
    public void Read()
    {
        SpeakerName.GetComponent<TextMeshProUGUI>().text = SpeakingName;
        Dialogue.GetComponent<TextMeshProUGUI>().text = SpeakingDialogue;

        SpeakingMenu.SetActive(true);
    }
    public void EndRead()
    {
        SpeakingMenu.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().isNearSign = true;
            other.gameObject.GetComponent<Player>().currentSign = this.GameObject();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().isNearSign = false;
        }
    }
}
