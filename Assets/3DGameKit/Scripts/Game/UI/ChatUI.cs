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
    struct chat_message {
        public GameObject chat_window;
        public ArrayList chat_my_message;
        public ArrayList chat_friend_message;
    }
    public GameObject messageView;
    public GameObject myMessage;
    public GameObject friendMessage;
    //public GameObject cur_chatWindow;
    private chat_message cur_chatWindow;
    public Text cur_friendName;
   // public string friendName_show;
   // private Dictionary<string, GameObject> chat_windows = null;
    private Dictionary<string, chat_message> chat_windows = null;
    // private Dictionary<string, ArrayList> chat_my_message = null;
    // private Dictionary<string, ArrayList> chat_friend_message = null;

    // my message info content layout | ----------------- message text | image |
    // friend's info content layout   | image | message text ----------------- |


    private void Awake()
    {
       // messageView.SetActive(false);
        myMessage.SetActive(false);
        friendMessage.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        //Test();
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

    public void ReceiveFriendMessage(string name, string text)
    {
        GameObject friendMessage111;
        //if (cur_chatWindow == null) return;
        //else
        //{
        //    friendMessage111 = cur_chatWindow.transform.Find("FriendMessageInfo").gameObject;
        //}
        //if (friendMessage111 == null)
        //{ 
        //    Debug.Log("friendMessage111 is null");
        //    return;
        //}
        //if (name != cur_friendName.text) {
        //    OnFriendInfoClick(cur_friendName);
        //}
        friendMessage111 = friendMessage;
        GameObject cloned = GameObject.Instantiate(friendMessage111);
        if (cloned == null) return;
        cloned.SetActive(true);
        cur_chatWindow.chat_friend_message.Add(cloned);
        AddElement(cloned, text);
    }

    public void SendMyMessage(string text)
    {
        GameObject myMessage111;
        //if (cur_chatWindow == null)
        //{
        //    return;
        //}
        //else {
        //    myMessage111 = cur_chatWindow.transform.Find("MyMessageInfo").gameObject;
        //}
        //if (myMessage111 == null)
        //{
        //    Debug.Log("myMessage111 is null");
        //    return;
        //}
            

        myMessage111 = myMessage;
        CChatMessage chatMsg = new CChatMessage();
        //NetworkEntity entity = PlayerMyController.Instance.Entity;
        //chatMsg.sender_entityId = entity.entityId;
        chatMsg.sender_userName = FGlobal.username;
        chatMsg.receiver_userName = cur_friendName.text;
        Debug.Log("current friend : " + cur_friendName.text);
        chatMsg.content = text;
        Client.Instance.Send(chatMsg);
        
        GameObject cloned = GameObject.Instantiate(myMessage111);
        if (cloned == null)
            return;
        cloned.SetActive(true);
        cur_chatWindow.chat_my_message.Add(cloned);
        AddElement(cloned, text);
        //ReceiveFriendMessage("momo", text);
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

    public void OnFriendInfoClick(Text FriendName)
    {
        //messageView.SetActive(false);
        //Destroy(messageView);
        Debug.Log("I receive the click");
        Debug.Log(FriendName.text);
        Debug.Log(cur_friendName.text);
        if (FriendName.text == cur_friendName.text)
        {
            return;
        }
        if (chat_windows == null) chat_windows = new Dictionary<string, chat_message>();
        //关闭当前窗口

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

            //GameObject allmymessage = cur_chatWindow.transform.Find("MyMessageInfo").gameObject;
            //allmymessage.SetActive(false);
            //GameObject allfriendmessage = cur_chatWindow.transform.Find("FriendMessageInfo").gameObject;
            //allfriendmessage.SetActive(false);
            //return;
        }

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

        cur_friendName.text = FriendName.text;
        GameObject cloned = GameObject.Instantiate(messageView);
        cur_chatWindow.chat_window = cloned;
        cur_chatWindow.chat_my_message = new ArrayList();
        cur_chatWindow.chat_friend_message = new ArrayList();
        //使用GameObject.Find的话会找不到隐藏的物体
        // GameObject name = cloned.transform.Find("MyMessageInfo").gameObject;

        //for (int i = 0; i < cloned.transform.childCount; i++)
        //{
        //    GameObject go = cloned.transform.GetChild(i).gameObject;
        //    Destroy(go);
        //}
       // cloned.transform.SetParent(transform, false);
        cloned.SetActive(true);
       // name.SetActive(true);
        
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

    void Test()
    {
        //AddNewMessage(true, "my message send");
        //AddNewMessage(false, "friend message receive");

        SendMyMessage("hello");
        ReceiveFriendMessage("momo", "hello");
    }

    void Test2() {

    }

    /*
    void AddNewMessage(bool mine, string message)
    {
        GameObject newContent = GameObject.Instantiate(content);
        if (newContent == null)
            return;
        GameObject newImage = GameObject.Instantiate(image);
        if (newImage == null)
            return;
        GameObject newText = GameObject.Instantiate(text);
        if (newText == null)
            return;

        HorizontalLayoutGroup layout = newContent.GetComponent<HorizontalLayoutGroup>();
        if (mine)
            layout.childAlignment = TextAnchor.UpperRight;
        else
            layout.childAlignment = TextAnchor.UpperLeft;

        TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
            return;

        //float width = textMesh.GetPreferredValues().x; // get preferred width before assign text
        textMesh.text = message;
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        if (rectTransform == null)
            return;

        RectTransform viewRect = messageContent.GetComponent<RectTransform>();
        if (viewRect == null)
            return;

        if (textMesh.preferredWidth < viewRect.rect.width)
        {
            ContentSizeFitter filter = textMesh.GetComponent<ContentSizeFitter>();
            if (filter != null)
            {
                filter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                rectTransform.sizeDelta = new Vector2(textMesh.preferredWidth, textMesh.preferredHeight);
            }
        }

        newImage.transform.SetParent(newContent.transform, false);
        newText.transform.SetParent(newContent.transform, false);
        newContent.transform.SetParent(messageContent.transform, false);
    }
    */
}
