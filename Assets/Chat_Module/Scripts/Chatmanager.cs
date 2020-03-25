using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using SendBird;
using SendBird.SBJson;

public class Chatmanager : MonoBehaviour
{
    public static Chatmanager Instance;

    private UserListQuery mUserListQuery;
    private GroupChannelListQuery mGroupChannelListQuery;

    public CommunityScreen screenManager;
    public ChatScreen chatScreen;
    public BlogScreen blogScreen;
    public ConnectScreen friendsScreen;

    public BaseChannel currentChannel;
    public User selectedUser;
    public OpenChannel _openChannel;

    public List<User> nearByPeople;
    public List<GroupChannel> friendsGroupChannel;
    public List<User> friendsUserList;
    public List<BaseMessage> chatHistrory;
    public List<BaseMessage> blogHistrory;

    public Color owner;
    public Color friend;

    public List<Sprite> avatar;
    public Sprite user;


    public string userName = "";

    // Use this for initialization
    public void StartConnection()
    {
        ConnectUser();

        SendBirdClient.ChannelHandler channelHandler = new SendBirdClient.ChannelHandler();
        channelHandler.OnMessageReceived = (BaseChannel channel, BaseMessage message) =>
        {

            Debug.Log("Check: " + currentChannel.Url.Equals(channel.Url));

            if (currentChannel.Url.Equals(channel.Url))
            {
                if (message is UserMessage)
                {
                    if (channel is GroupChannel)
                    {
                        chatScreen.AddChatMessage(message);
                        //Debug.Log("Data " + ((UserMessage)message).Data + "   Message  :" + ((UserMessage)message).Message +
                        //    "   Sender  :" + ((UserMessage)message).Sender.Nickname);
                    }
                    //else
                    //{
                    //    //Debug.Log("Data " + ((UserMessage)message).Data + "   Message  :" + ((UserMessage)message).Message +
                    //    //    "   Sender  :" + ((UserMessage)message).Sender.Nickname);
                    //    AddBlogMessage(message);
                    //}
                }

                if (message is AdminMessage)
                {
                    Debug.Log("   Message  :" + ((AdminMessage)message).Message);
                }
            }
            else if (_openChannel.Url.Equals(channel.Url))
            {
                if (message is UserMessage)
                {
                    if (channel is OpenChannel)
                    {
                        AddBlogMessage(message);
                    }

                }
            }

            SendBirdClient.AddChannelHandler("default", channelHandler);


        };
    }



    void Awake()
    {
        Instance = this;

        SendBirdClient.SetupUnityDispatcher(gameObject); // Set SendBird gameobject to DontDestroyOnLoad.
        StartCoroutine(SendBirdClient.StartUnityDispatcher); // Start a Unity dispatcher.

        SendBirdClient.Init("5311CDB0-4A1F-4DB2-8939-A26EC98290B5"); // SendBird Sample Application ID
        SendBirdClient.Log += (message) =>
        {
            Debug.Log(message);
        };
    }

    public void ConnectUser()
    {
        SendBirdClient.Connect(userName, (user, e) =>
        {
            if (e != null)
            {
                Debug.Log(e.Code + ": " + e.Message);
                return;
            }

            SendBirdClient.UpdateCurrentUserInfo(userName, null, (e1) =>
            {
                if (e1 != null)
                {
                    Debug.Log(e.Code + ": " + e.Message);
                    return;
                }
                GetListOpenGroup();
                PeopleNearYou();
                GetListOfFriends();
            });
        });
    }



    public void PeopleNearYou()
    {
        nearByPeople = new List<User>();
        mUserListQuery = SendBirdClient.CreateUserListQuery();
        mUserListQuery.Limit = 50;

        mUserListQuery.Next((list, e) =>
        {
            if (e != null)
            {
                Debug.Log(e.Code + ": " + e.Message);
                return;
            }

            foreach (User user in list)
            {
                if (!user.Nickname.Equals(userName))
                    nearByPeople.Add(user);
            }
            Debug.Log("Near by people : " + nearByPeople.Count);
            if (friendsScreen.isConnect)
                friendsScreen.RefreshNearBy();
        });

    }

    public void GetListOfFriends()
    {
        friendsGroupChannel = new List<GroupChannel>();
        friendsUserList = new List<User>();
        mGroupChannelListQuery = GroupChannel.CreateMyGroupChannelListQuery();
        mGroupChannelListQuery.Limit = 50;
        mGroupChannelListQuery.Next((list, e) =>
        {
            if (e != null)
            {
                Debug.Log(e.Code + ": " + e.Message);
                return;
            }
            foreach (GroupChannel groupChannel in list)
            {
                foreach (User user in groupChannel.Members)
                {
                    if (!user.Nickname.Equals(userName))
                    {
                        friendsGroupChannel.Add(groupChannel);
                        friendsUserList.Add(user);
                        var index = nearByPeople.FindIndex(i => i.Nickname == user.Nickname);
                        if (index >= 0)
                        {   // ensure item found
                            nearByPeople.RemoveAt(index);
                        }
                        break;
                    }

                }
            }


            Debug.Log("Number of friends " + friendsUserList.Count);
            if (friendsScreen.isConnect)
                friendsScreen.RefreshFriends();

        });
    }

    public void GetListOpenGroup()
    {
        string s = "";
        OpenChannelListQuery mChannelListQuery = OpenChannel.CreateOpenChannelListQuery();
        mChannelListQuery.Next((List<OpenChannel> channels, SendBirdException e) =>
        {
            if (e != null)
            {
                // Error.
                return;
            }
            Debug.Log(channels[0].Url);
            EnterChannel(channels[0].Url);
        });

    }

    public void EnterChannel(string CHANNEL_URL)
    {
        OpenChannel.GetChannel(CHANNEL_URL, (OpenChannel openChannel, SendBirdException e) =>
        {
            if (e != null)
            {
                // Error.
                return;
            }
            _openChannel = openChannel;
            _openChannel.Enter((SendBirdException e3) =>
            {
                if (e3 != null)
                {
                    // Error.
                    return;
                }

                GetPreviousOpenChannelData();
            });
        });
    }


    public void CreatePrivateChat()
    {
        AppManager.Instance.loading.SetActive(true);
        List<User> mUserList = new List<User>();
        mUserList.Add(selectedUser);

        GroupChannel.CreateChannel(mUserList, false, (channel, e) =>
        {
            if (e != null)
            {
                Debug.Log(e.Code + ": " + e.Message);
                AppManager.Instance.loading.SetActive(false);
                return;
            }
            currentChannel = (GroupChannel)channel;
            currentChannel.SendUserMessage("Hello I am using BeSober.", (message, e1) =>
            {
                if (e1 != null)
                {
                    Debug.Log(e.Code + ": " + e1.Message);
                    return;
                }

                GetListOfFriends();
                LoadPreviousChatHistory();
                AppManager.Instance.loading.SetActive(false);
            });

        });


    }

    public void GetPreviousOpenChannelData()
    {
        PreviousMessageListQuery mPrevMessageListQuery = _openChannel.CreatePreviousMessageListQuery();
        mPrevMessageListQuery.Load(30, true, (List<BaseMessage> messages, SendBirdException e) =>
        {
            if (e != null)
            {
                // Error.
                return;
            }
            blogHistrory = messages;
        });
    }

    public void AddBlogMessage(BaseMessage msg)
    {
        blogHistrory.Add(msg);
        blogScreen.AddBlogPost(msg);
    }

    public void LoadPreviousChatHistory()
    {
        chatHistrory = new List<BaseMessage>();
        foreach (GroupChannel groupChannel in friendsGroupChannel)
        {
            foreach (User user in groupChannel.Members)
            {
                if (!user.Nickname.Equals(userName) && user.Nickname.Equals(selectedUser.Nickname))
                {
                    currentChannel = groupChannel;
                    break;
                }
            }
        }
        PreviousMessageListQuery mPrevMessageListQuery = currentChannel.CreatePreviousMessageListQuery();
        mPrevMessageListQuery.Load(15, true, (List<BaseMessage> messages, SendBirdException e) =>
        {
            if (e != null)
            {
                // Error.
                return;
            }

            chatHistrory = messages;
            screenManager.ShowChat();
        });
    }

    public void SendPrivateMessage(string data)
    {
        Debug.Log("data" + (currentChannel != null));
        Debug.Log("data" + (currentChannel.IsGroupChannel()));
        if (currentChannel != null && currentChannel.IsGroupChannel())
        {
            Debug.Log("send message1");
            currentChannel.SendUserMessage(data, (UserMessage message, SendBirdException e) =>
            {
                if (e != null)
                {
                    Debug.Log(e.Code + ": " + e.Message);
                    return;
                }
                Debug.Log("send message" + ((UserMessage)message).Message);
                chatScreen.AddChatMessage(message);

            });
        }
    }

    public void SendBlogPost(string data)
    {

        if (_openChannel != null && _openChannel.IsOpenChannel())
        {
            _openChannel.SendUserMessage(data, (UserMessage message, SendBirdException e) =>
            {
                if (e != null)
                {
                    Debug.Log(e.Code + ": " + e.Message);
                    return;
                }

                blogScreen.AddBlogPost(message);

            });
        }
    }

    public void UnFriend()
    {
        GroupChannel group_Channel = null;
        foreach (GroupChannel groupChannel in friendsGroupChannel)
        {
            foreach (User user in groupChannel.Members)
            {
                if (!user.Nickname.Equals(userName) && user.Nickname.Equals(selectedUser.Nickname))
                {
                    group_Channel = groupChannel;
                    break;
                }
            }
        }

        var index = friendsUserList.FindIndex(i => i.Nickname == selectedUser.Nickname);
        if (index >= 0)
            friendsUserList.RemoveAt(index);
        friendsGroupChannel.Remove(group_Channel);

        group_Channel.Leave((SendBirdException e) =>
        {
            if (e != null)
            {
                // Error.
                return;
            }

            Debug.Log("UserLeft");
            PeopleNearYou();
            GetListOfFriends();
        });
    }

}
