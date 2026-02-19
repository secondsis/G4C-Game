using System;
using System.Collections;
using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class NotificationManager : MonoBehaviour
{
    [FormerlySerializedAs("WarningObject")] [SerializeField] private GameObject WarningPrefab;
    [FormerlySerializedAs("InfoObject")] [SerializeField] private GameObject InfoPrefab;
    
    private Queue<Notification> queue = new Queue<Notification>();

    private void DisableObject(GameObject gameObj)
    {
        gameObj.SetActive(false);
    }
    
    private void DisplayNotification()
    {
        Notification notification = queue.Dequeue();
        if (notification.GetNotificationLevel() == NotificationLevel.WARNING)
        {
            GameObject warningObj = Instantiate(WarningPrefab);
            // The prefab will destroy itself after like 4 s
            warningObj.GetComponent<TextMeshProUGUI>().text = notification.GetMessage();
            WarningPrefab.gameObject.SetActive(true);
        } else if (notification.GetNotificationLevel() == NotificationLevel.INFO)
        {
            GameObject infoObj = Instantiate(InfoPrefab);
            infoObj.GetComponent<TextMeshProUGUI>().text = notification.GetMessage();
            InfoPrefab.gameObject.SetActive(true);
        }
        
    }

    IEnumerator QueueLoop()
    {
        while (queue.Count != 0)
        {
            DisplayNotification();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddNotificationToQueue(Notification notification)
    {
        bool queueAfter = false;
        if (queue.Peek() == null)
        {
            // Start the DisplayNotification queue (cooldown of like, .1 seconds)
            queueAfter = true;
        }
        queue.Enqueue(notification);
        if(queueAfter) StartCoroutine(QueueLoop());
    }
}
