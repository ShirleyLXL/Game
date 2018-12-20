using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gamekit3D;
using Common;
using Gamekit3D.Network;

public class ChatUI : MonoBehaviour
{
    //将聊天窗口信息都存入该结构体中
    struct chat_message {
        public GameObject chat_window;
        public ArrayList chat_my_message;
        public ArrayList chat_friend_message;
    }
    //用好友名字获取聊天窗口信息
    private Dictionary<string, chat_message> chat_windows = null;

    public GameObject messageView;
    public GameObject myMessage;
    public GameObject friendMessage;

    private chat_message cur_chatWindow;
    public Text cur_friendName;

    


    // my message info content layout | ----------------- message text | image |
    // friend's info content layout   | image | message text ----------------- |


    private void Awake()
    {
        myMessage.SetActive(false);
        friendMessage.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        
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

    //目前需要自己点开对应朋友的聊天窗口，此函数为当前聊天窗口收到消息。
    public void ReceiveFriendMessage(string name, string text)
    {
        GameObject cloned = GameObject.Instantiate(friendMessage);
        if (cloned == null) return;
        cloned.SetActive(true);
        cur_chatWindow.chat_friend_message.Add(cloned);
        AddElement(cloned, text);
    }

    public void SendMyMessage(string text)
    {
        //发送消息给后端
        CChatMessage chatMsg = new CChatMessage();
        chatMsg.sender_userName = FGlobal.username;
        chatMsg.receiver_userName = cur_friendName.text;
        Debug.Log("current friend : " + cur_friendName.text);
        chatMsg.content = text;
        Client.Instance.Send(chatMsg);
        
        //自己的窗口显示自己发的信息
        GameObject cloned = GameObject.Instantiate(myMessage);
        if (cloned == null)
            return;
        cloned.SetActive(true);
        cur_chatWindow.chat_my_message.Add(cloned);
        AddElement(cloned, text);
    }

    public void OnSendButtonClick(GameObject inputField)
    {
        InputField input = inputField.GetComponent<InputField>();
        if (input == null)
            return;

        if (input.text.Trim().Length == 0)
            return;


        SendMyMessage(input.text);

        input.text = "";
    }

    //点击好友切换聊天窗口
    public void OnFriendInfoClick(Text FriendName)
    {
        Debug.Log("I receive the click");
        Debug.Log(FriendName.text);
        Debug.Log(cur_friendName.text);

        //当前聊天窗口即为点击的好友，则直接返回
        if (FriendName.text == cur_friendName.text)
        {
            return;
        }
        if (chat_windows == null) chat_windows = new Dictionary<string, chat_message>();

        //关闭当前聊天窗口
        if (chat_windows.ContainsKey(cur_friendName.text)) 
        {
            Debug.Log("prepare to set active false");
            chat_windows[cur_friendName.text] = cur_chatWindow;
            chat_message cur_msg = chat_windows[cur_friendName.text];
            cur_msg.chat_window.SetActive(false);
            foreach(GameObject m in cur_msg.chat_my_message) {
                m.SetActive(false);
            }
            foreach (GameObject m in cur_msg.chat_friend_message)
            {
                m.SetActive(false);
            }
            Debug.Log("finish setting active false");

        }

        //如果对应好友的chat_window已经创建（即之前已经点开过了），则找到对应窗口和消息显示，并返回
        if (chat_windows.ContainsKey(FriendName.text))
        {
            chat_message msg = chat_windows[FriendName.text];
            msg.chat_window.SetActive(true);
            foreach (GameObject m in msg.chat_my_message)
            {
                m.SetActive(true);
            }
            foreach (GameObject m in msg.chat_friend_message)
            {
                m.SetActive(true);
            }
            cur_friendName.text = FriendName.text;
            cur_chatWindow = msg;
            return;
        }

        //创建新的chat_window
        cur_friendName.text = FriendName.text;
        GameObject cloned = GameObject.Instantiate(messageView);
        cur_chatWindow.chat_window = cloned;
        cur_chatWindow.chat_my_message = new ArrayList();
        cur_chatWindow.chat_friend_message = new ArrayList();
        cloned.SetActive(true);
        
        chat_windows.Add(FriendName.text, cur_chatWindow);

    }

    void AddElement(GameObject element, string text)
    {
        TextMeshProUGUI textMesh = element.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
            return;
        //float width = textMesh.GetPreferredValues().x; // get preferred width before assign text
        textMesh.text = text;
        RectTransform rectTransform = element.GetComponent<RectTransform>();
        if (rectTransform == null)
            return;

        RectTransform parentRect = this.GetComponent<RectTransform>();
        if (parentRect == null)
            return;

        if (textMesh.preferredWidth < parentRect.rect.width)
        {
            ContentSizeFitter filter = textMesh.GetComponent<ContentSizeFitter>();
            if (filter != null)
            {
                filter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                textMesh.rectTransform.sizeDelta = new Vector2(textMesh.preferredWidth, textMesh.preferredHeight);
            }
        }

        element.transform.SetParent(this.transform, false);

        ScrollRect scrollRect = messageView.GetComponent<ScrollRect>();
        if (scrollRect == null)
            return;

        scrollRect.normalizedPosition = new Vector2(0, 0);
    }


}
