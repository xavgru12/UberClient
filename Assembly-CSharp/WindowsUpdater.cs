using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

internal class WindowsUpdater
{
	private static string Auth;

	private static string Read;

	private static string Entry;

	private static List<string[]> FileData = new List<string[]>();

	private static List<string[]> DownloadList = new List<string[]>();

	private static string gamepath = Directory.GetCurrentDirectory();

	private static string url_latest = "https://raw.githubusercontent.com/HaZardousss/UberUpdates/master/Entry.txt";

	private static string url_auth = "https://raw.githubusercontent.com/HaZardousss/UberUpdates/master/Auth";

	private static int currentdownloadcount = 0;

	public static IEnumerator Updater()
	{
		if (Directory.Exists(gamepath + "\\Updates"))
		{
			Directory.Delete(gamepath + "\\Updates", recursive: true);
		}
		AuthenticationManager._progress.Text = "Checking Updates";
		AuthenticationManager._progress.Progress = 0f;
		if (File.Exists(gamepath + "\\devx"))
		{
			NoUpdate();
			yield break;
		}
		yield return null;
		yield return UnityRuntime.StartRoutine(WWWRead(url_auth));
		Auth = Read;
		if (Auth.ToLower().Contains("qa"))
		{
			ApplicationDataManager.LockApplication("Servers are down for update. Please wait.");
			yield break;
		}
		yield return UnityRuntime.StartRoutine(WWWRead(url_latest));
		Entry = Read;
		if (Entry == null || Auth == null)
		{
			ApplicationDataManager.LockApplication("Could not connect to update servers.\nCheck your Internet connection or turn off Firewall if the issue persists");
			yield break;
		}
		string[] array = Entry.Split(new string[1]
		{
			"\n"
		}, StringSplitOptions.RemoveEmptyEntries);
		foreach (string lines in array)
		{
			yield return null;
			string[] item = lines.Split(' ');
			FileData.Add(item);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		int i;
		for (i = 0; i < FileData.Count; i++)
		{
			yield return null;
			string lines = gamepath + "\\" + FileData[i][0];
			string downlink = "https://raw.githubusercontent.com/HaZardousss/UberUpdates/master/Windows/" + FileData[i][0].Replace("\\", "/");
			string downpath = gamepath + "\\Updates\\UberStrike\\" + FileData[i][0];
			yield return null;
			if (!File.Exists(lines))
			{
				AddToDownload(downlink, downpath, FileData[i][2]);
			}
			else if (!FileData[i][1].Equals(CalculateMD5(lines)))
			{
				AddToDownload(downlink, downpath, FileData[i][2]);
			}
		}
		if (DownloadList.Count == 0)
		{
			NoUpdate();
			yield break;
		}
		PopupSystem.Show(AuthenticationManager._progress);
		i = 0;
		while (i < DownloadList.Count)
		{
			yield return null;
			yield return UnityRuntime.StartRoutine(WWWDownload(DownloadList[currentdownloadcount][1], DownloadList[currentdownloadcount][0], (i + 1).ToString() + " / " + DownloadList.Count.ToString()));
			i++;
			currentdownloadcount++;
		}
		Finished();
		while (true)
		{
			yield return new WaitForSeconds(5f);
		}
	}

	private static IEnumerator WWWDownload(string path, string link, string index)
	{
		using (WWW www = new WWW(link))
		{
			while (!www.isDone)
			{
				AuthenticationManager._progress.Progress = www.progress * 1.2f;
				AuthenticationManager._progress.Text = "Downloading File " + index;
				yield return null;
			}
			yield return new WaitForSeconds(0.1f);
			File.WriteAllBytes(path, www.bytes);
		}
		yield return new WaitForEndOfFrame();
	}

	private static IEnumerator WWWRead(string link)
	{
		using (WWW www = new WWW(link))
		{
			while (!www.isDone)
			{
				yield return null;
			}
			Read = www.text;
		}
	}

	private static string CalculateMD5(string filename)
	{
		using (MD5 mD = MD5.Create())
		{
			using (FileStream inputStream = File.OpenRead(filename))
			{
				byte[] value = mD.ComputeHash(inputStream);
				return BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
			}
		}
	}

	private static void AddToDownload(string link, string downloadpath, string downloadsize)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(downloadpath));
		string[] item = new string[3]
		{
			link,
			downloadpath,
			downloadsize
		};
		DownloadList.Add(item);
	}

	private static void Finished()
	{
		DeleteUnnecessary();
		CopyFiles(Path.Combine(Directory.GetCurrentDirectory(), "Updates\\Uberstrike"), Directory.GetCurrentDirectory());
		Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "UberStrike.exe")).WaitForExit();
		Application.Quit();
	}

	private static void CopyFiles(string SourcePath,string DestinationPath)
	{
		foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",SearchOption.AllDirectories))
		{
			try
			{
				if(!Directory.Exists(dirPath.Replace(SourcePath, DestinationPath)))
					Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
			}
			catch(Exception e)
			{
				UnityEngine.Debug.LogError(e);
			}
		}
		foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",SearchOption.AllDirectories))
		{
			try
			{
				string dest = newPath.Replace(SourcePath, DestinationPath);
				UnityEngine.Debug.LogError($"Path = {dest}");
				if(File.Exists(dest))
				{
					UnityEngine.Debug.LogError($"File exists at {dest} trying to move it to temp");
					File.Move(dest, dest + ".bkp");
					UnityEngine.Debug.LogError($"Moved successfully");
					File.Delete(dest + ".bkp");
					UnityEngine.Debug.LogError($"Temp file deleted");					
				}
				File.Copy(newPath, dest, true);
				UnityEngine.Debug.LogError("Copied successfully");
			}
			catch(Exception e)
			{
				UnityEngine.Debug.LogError(e);
			}
		}
	}

	private static int DeleteUnnecessary()
	{
		DeleteFile("UberStrike_Data\\level12");
		DeleteFile("version.txt");
		DeleteFile("UberEyeEngine.exe");
		DeleteFile("UberStrike_Data\\version.txt");
		DeleteFile("UberStrike_Data\\Plugins\\CommEye.exe");
		DeleteFile("UberEye.exe");
		DeleteFile("Scs.dll");
		DeleteFile("Uber.eye");
		DeleteFile("Scs.xml");
		return 0;
	}

	private static void NoUpdate()
	{
		try
		{
			if (Directory.Exists(gamepath + "\\Updates"))
			{
				Directory.Delete(gamepath + "\\Updates", recursive: true);
			}
		}
		catch(Exception e) { UnityEngine.Debug.LogError(e); }
	}

	private static int DeleteFolder(string path)
	{
		path = Directory.GetCurrentDirectory() + "\\" + path;
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
		return 0;
	}

	private static int DeleteFile(string path)
	{
		try
		{
			path = Directory.GetCurrentDirectory() + "\\" + path;
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch (Exception e) { UnityEngine.Debug.LogError(e); }
		return 0;
	}
}
