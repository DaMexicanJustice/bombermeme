using System;
using System.Collections.Generic;
using Irc;
using UnityEngine;
using UnityEngine.UI;

public class TwitchEvents : MonoBehaviour
{
	public InputField UsernameText;
	public InputField TokenText;
	public InputField ChannelText;

	public Text ChatText;
	public InputField MessageText;

	public GameObject gameMaster;
	private GameEvents ge;

	void Start()
	{
		//Subscribe for events
		TwitchIrc.Instance.OnChannelMessage += OnChannelMessage;
		TwitchIrc.Instance.OnUserLeft += OnUserLeft;
		TwitchIrc.Instance.OnUserJoined += OnUserJoined;
		TwitchIrc.Instance.OnServerMessage += OnServerMessage;
		TwitchIrc.Instance.OnExceptionThrown += OnExceptionThrown;

		ge = gameMaster.GetComponent<GameEvents> ();
	}

	public void Connect()
	{
		TwitchIrc.Instance.Username = UsernameText.text;
		TwitchIrc.Instance.OauthToken = TokenText.text;
		TwitchIrc.Instance.Channel = ChannelText.text;

		TwitchIrc.Instance.Connect();
	}

	//Send message
	public void MessageSend(String msg)
	{
		/*
		if (String.IsNullOrEmpty(MessageText.text))
			return;

		TwitchIrc.Instance.Message(MessageText.text);
		ChatText.text += "<b>" + TwitchIrc.Instance.Username + "</b>: " + MessageText.text +"\n";
		MessageText.text = ""; */
		TwitchIrc.Instance.Message(msg);

	}

	//Open URL
	public void GoUrl(string url)
	{
		Application.OpenURL(url);
	}

	//Receive message from server
	void OnServerMessage(string message)
	{
		ChatText.text += "<b>SERVER:</b> " + message + "\n";
		Debug.Log(message);
	}

	// ALL THE COMMANDS WE WANT IN OUR GAME GOES HERE. EVALUATE IN A SWITCH STATEMENT AND COMMUNICATE THE EVENT TO GAMEMASTER
	void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs)
	{

		String msg = channelMessageArgs.Message;

		switch (msg) {
		case "!bomb":
			ge.StartBombEvent ();
			break;
		case "!kappa":
			ge.StartKappaEvent ();
			break;
		case "!upgrademusic":
			ge.UpgradeMusic ();
			break;
		case "!swiftrage":
			ge.StartSwiftRageEvent ();
			break;
		case "!acid":
			ge.StartLightEvent ();
			break;
		case "!rotate":
			ge.StartRotateEvent ();
			break;
		case "!shake":
			ge.StartShakeEvent ();
			break;
		case "!mirror":
			ge.StartMirrorEvent ();
			break;
		case "!cheer":
			ge.StartCheerEvent ();
			break;
		case "!commands":
			MessageSend ("!bomb, !kappa, !upgrademusic, !swiftrage, !acid, !rotate, !mirror, !shake, !cheer");
			break;
		default:
			break;
		}


		ChatText.text += "<b>" + channelMessageArgs.From + ":</b> " + channelMessageArgs.Message + "\n";
		//Debug.Log("MESSAGE: " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
	}

	//Get the name of the user who joined to channel 
	void OnUserJoined(UserJoinedEventArgs userJoinedArgs)
	{
		ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userJoinedArgs.User + "\n";
		Debug.Log("USER JOINED: " + userJoinedArgs.User);
	}


	//Get the name of the user who left the channel.
	void OnUserLeft(UserLeftEventArgs userLeftArgs)
	{
		ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userLeftArgs.User + "\n";
		Debug.Log("USER JOINED: " + userLeftArgs.User);
	}

	//Receive exeption if something goes wrong
	private void OnExceptionThrown(Exception exeption)
	{
		Debug.Log(exeption);
	}

}
