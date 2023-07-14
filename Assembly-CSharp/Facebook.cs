// Decompiled with JetBrains decompiler
// Type: Facebook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facebook : P31RestKit
{
  public string accessToken;
  public string appAccessToken;
  private static Facebook _instance;

  public Facebook()
  {
    this._baseUrl = "https://graph.facebook.com/";
    this.forceJsonResponse = true;
  }

  public static Facebook instance
  {
    get
    {
      if (Facebook._instance == null)
        Facebook._instance = new Facebook();
      return Facebook._instance;
    }
  }

  protected virtual IEnumerator send(
    string path,
    HTTPVerb httpVerb,
    Dictionary<string, object> parameters,
    Action<string, object> onComplete)
  {
    if (parameters == null)
      parameters = new Dictionary<string, object>();
    if (!parameters.ContainsKey("access_token"))
      parameters.Add("access_token", (object) this.accessToken);
    return base.send(path, httpVerb, parameters, onComplete);
  }

  public void graphRequest(string path, Action<string, object> completionHandler) => this.get(path, (Dictionary<string, object>) null, completionHandler);

  public void graphRequest(string path, HTTPVerb verb, Action<string, object> completionHandler) => this.graphRequest(path, verb, (Dictionary<string, object>) null, completionHandler);

  public void graphRequest(
    string path,
    HTTPVerb verb,
    Dictionary<string, object> parameters,
    Action<string, object> completionHandler)
  {
    this.surrogateMonobehaviour.StartCoroutine(this.send(path, verb, parameters, completionHandler));
  }

  public void postMessage(string message, Action<string, object> completionHandler) => this.post("me/feed", new Dictionary<string, object>()
  {
    {
      nameof (message),
      (object) message
    }
  }, completionHandler);

  public void postMessageWithLink(
    string message,
    string link,
    string linkName,
    Action<string, object> completionHandler)
  {
    this.post("me/feed", new Dictionary<string, object>()
    {
      {
        nameof (message),
        (object) message
      },
      {
        nameof (link),
        (object) link
      },
      {
        "name",
        (object) linkName
      }
    }, completionHandler);
  }

  public void postMessageWithLinkAndLinkToImage(
    string message,
    string link,
    string linkName,
    string linkToImage,
    string caption,
    Action<string, object> completionHandler)
  {
    this.post("me/feed", new Dictionary<string, object>()
    {
      {
        nameof (message),
        (object) message
      },
      {
        nameof (link),
        (object) link
      },
      {
        "name",
        (object) linkName
      },
      {
        "picture",
        (object) linkToImage
      },
      {
        nameof (caption),
        (object) caption
      }
    }, completionHandler);
  }

  public void postImage(byte[] image, string message, Action<string, object> completionHandler) => this.post("me/photos", new Dictionary<string, object>()
  {
    {
      "picture",
      (object) image
    },
    {
      nameof (message),
      (object) message
    }
  }, completionHandler);

  public void postImageToAlbum(
    byte[] image,
    string caption,
    string albumId,
    Action<string, object> completionHandler)
  {
    Dictionary<string, object> dictionary = new Dictionary<string, object>()
    {
      {
        "picture",
        (object) image
      },
      {
        "message",
        (object) caption
      }
    };
    this.post(albumId, dictionary, completionHandler);
  }

  public void getFriends(Action<string, object> completionHandler) => this.get("me/friends", completionHandler);

  public void getAppAccessToken(string appId, string appSecret, Action<string> completionHandler) => this.get("oauth/access_token", new Dictionary<string, object>()
  {
    {
      "client_id",
      (object) appId
    },
    {
      "client_secret",
      (object) appSecret
    },
    {
      "grant_type",
      (object) "client_credentials"
    }
  }, (Action<string, object>) ((error, obj) =>
  {
    if (obj is string)
    {
      string str = obj as string;
      if (str.StartsWith("access_token="))
      {
        this.appAccessToken = str.Replace("access_token=", string.Empty);
        completionHandler(this.appAccessToken);
      }
      else
        completionHandler((string) null);
    }
    else
      completionHandler((string) null);
  }));

  public void postScore(string userId, int score, Action<bool> completionHandler)
  {
    if (this.appAccessToken == null)
    {
      Debug.Log((object) "you must first retrieve the app access token before posting a score");
      completionHandler(false);
    }
    else if (userId == null)
    {
      Debug.Log((object) "a valid userId is required to post a score");
      completionHandler(false);
    }
    else
      this.post(userId + "/scores", new Dictionary<string, object>()
      {
        {
          nameof (score),
          (object) score.ToString()
        },
        {
          "app_access_token",
          (object) this.appAccessToken
        },
        {
          "access_token",
          (object) this.appAccessToken
        }
      }, (Action<string, object>) ((error, obj) =>
      {
        if (error == null && obj is bool flag2)
          completionHandler(flag2);
        else
          completionHandler(false);
      }));
  }

  public void getScores(string userId, Action<string, object> onComplete) => this.get(userId + "/scores", onComplete);
}
