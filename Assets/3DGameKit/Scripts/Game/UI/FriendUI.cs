using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;
using Common;
using Gamekit3D.Network;
using UnityEngine.UI;

public class FriendUI : MonoBehaviour
{
    public GameObject FriendInfo;
    public Text FriendName;

    private void Awake()
    {
        

    }
    // Use this for initialization
    void Start()
    {
        FriendInfo.SetActive(false);
        //发送消息给后端
        CFriendsList friendsList = new CFriendsList();
        friendsList.username = FGlobal.username;
        Client.Instance.Send(friendsList);
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

    public void AddFriendList()
    {
        if (FGlobal.friendList == null)
        {
            Debug.Log("friendList null");
            return;
        }

        foreach (string names in FGlobal.friendList) {
            FriendName.text = names;
            GameObject cloned = GameObject.Instantiate(FriendInfo);
            cloned.transform.SetParent(transform, false);
            cloned.SetActive(true);
        }
    }
}
