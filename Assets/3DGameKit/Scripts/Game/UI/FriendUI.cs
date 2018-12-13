using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;
using Common;
using Gamekit3D.Network;

public class FriendUI : MonoBehaviour
{
    public GameObject FriendInfo;

    private void Awake()
    {
        FriendInfo.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        CFriendsList friendsList = new CFriendsList();
        friendsList.userID = 1;
        Client.Instance.Send(friendsList);    //只改了这里，在点开好友列表的时候发送一个消息给后端
        Debug.Log("send");
        Test();
    }

    private void OnEnable()
    {
        PlayerMyController.Instance.EnabledWindowCount++;
    }

    private void OnDisable()
    {
        PlayerMyController.Instance.EnabledWindowCount--;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Test()
    {
        
        for (int i = 0; i < 100; i++)
        {
            GameObject cloned = GameObject.Instantiate(FriendInfo);
            cloned.transform.SetParent(transform, false);
            cloned.SetActive(true);
        }
    }
}
