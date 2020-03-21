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
    public ConnectScreen friendsScreen;

    public BaseChannel currentChannel;
    public User selectedUser;

    public List<User> nearByPeople;
    public List<GroupChannel> friendsGroupChannel;
    public List<User> friendsUserList;
    public List<BaseMessage> chatHistrory;

    public Color owner;
    public Color friend;

    public List<Sprite> avatar;


    public string userName = "";

    // Use this for initialization
    public void StartConnection()
    {
        ConnectUser();

        SendBirdClient.ChannelHandler channelHandler = new SendBirdClient.ChannelHandler();
        channelHandler.OnMessageReceived = (BaseChannel channel, BaseMessage message) =>
        {
            if (currentChannel.Url == channel.Url)
            {
                if (message is UserMessage)
                {
                    chatScreen.AddChatMessage(message);
                    //Debug.Log("Data " + ((UserMessage)message).Data + "   Message  :" + ((UserMessage)message).Message +
                    //    "   Sender  :" + ((UserMessage)message).Sender.Nickname);
                }

                if (message is AdminMessage)
                {
                    Debug.Log("   Message  :" + ((AdminMessage)message).Message);
                }
            }
        };

        SendBirdClient.AddChannelHandler("default", channelHandler);

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
            Debug.Log("Near by people : " + nearByPeople.Count);
        });
    }


    public void CreatePrivateChat()
    {
        List<User> mUserList = new List<User>();
        mUserList.Add(selectedUser);

        GroupChannel.CreateChannel(mUserList, false, (channel, e) =>
        {
            if (e != null)
            {
                Debug.Log(e.Code + ": " + e.Message);
                return;
            }
            currentChannel = channel;
            currentChannel.SendUserMessage("Hello I am using BeSober.", (message, e1) =>
            {
                if (e1 != null)
                {
                    Debug.Log(e.Code + ": " + e1.Message);
                    return;
                }

                GetListOfFriends();
                LoadPreviousChatHistory();
            });

        });


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

        if (currentChannel != null && currentChannel.IsGroupChannel())
        {
            currentChannel.SendUserMessage(data, (UserMessage message, SendBirdException e) =>
            {
                if (e != null)
                {
                    Debug.Log(e.Code + ": " + e.Message);
                    return;
                }

                chatScreen.AddChatMessage(message);

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
            friendsScreen.Refresh();
        });
    }

}
