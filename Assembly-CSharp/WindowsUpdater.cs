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

	private static bool downloadError;

	private static string gamepath = Directory.GetCurrentDirectory();

	private static string url_latest = "https://raw.githubusercontent.com/HaZardousss/UberUpdates/master/Entry.txt";

	private static string url_auth = "https://raw.githubusercontent.com/HaZardousss/UberUpdates/master/Auth";

	private static string url_windows = "https://raw.githubusercontent.com/HaZardousss/UberUpdates/master/Windows";
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
			string downlink = url_windows + "/" + FileData[i][0].Replace("\\", "/");
			string downpath = gamepath + "\\Updates\\UberStrike\\" + FileData[i][0];
			yield return null;
			if (!File.Exists(lines))
			{
				AddToDownload(downlink, downpath, FileData[i][1], FileData[i][2]);
			}
			else if (!FileData[i][1].Equals(CalculateMD5(lines)))
			{
				AddToDownload(downlink, downpath, FileData[i][1], FileData[i][2]);
			}
		}
		if (DownloadList.Count == 0)
		{
			NoUpdate();
			yield break;
		}
		i = 0;
		downloadError = false;
		while (i < DownloadList.Count)
		{
			yield return null;
			yield return UnityRuntime.StartRoutine(WWWDownload(DownloadList[i][1], DownloadList[i][0], DownloadList[i][2], DownloadList[i][3], (i + 1).ToString() + " / " + DownloadList.Count.ToString()));
			if (downloadError)
			{
				break;
			}
			i++;
		}
		if (downloadError)
		{
			NoUpdate();
			ApplicationDataManager.LockApplication("Could not download update files from the server. Please try again later.");
			yield break;
		}
		Finished();
	}

	private static IEnumerator WWWDownload(string path, string link, string expectedMd5, string expectedSize, string index)
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

			// Do not trust the download. Recheck size and md5 hash to ensure the file is valid.
			byte[] bytes = www.bytes;
			bool ok = string.IsNullOrEmpty(www.error) && bytes != null && bytes.Length > 0;
			if (ok && long.TryParse(expectedSize, out long expected) && expected > 0 && bytes.Length != expected)
			{
				UnityEngine.Debug.LogError("size mismatch for " + link + " (expected=" + expected + ", actual=" + bytes.Length + ")");
				ok = false;
			}
			if (ok && !string.IsNullOrEmpty(expectedMd5))
			{
				string actualMd5;
				using (MD5 mD = MD5.Create())
				{
					actualMd5 = BitConverter.ToString(mD.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
				}
				if (!expectedMd5.Equals(actualMd5, StringComparison.OrdinalIgnoreCase))
				{
					UnityEngine.Debug.LogError("Server side Entry.txt md5 hash does not match for " + link + " (expected=" + expectedMd5 + ", actual=" + actualMd5 + ")");
					ok = false;
				}
			}
			if (!ok)
			{
				UnityEngine.Debug.LogError("Download failed or corrupted for " + link + " (error=" + www.error + ", bytes=" + ((bytes != null) ? bytes.Length : (-1)) + ", expected=" + expectedSize + ")");
				downloadError = true;
				yield break;
			}
			File.WriteAllBytes(path, bytes);
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

	private static void AddToDownload(string link, string downloadpath, string expectedMd5, string downloadsize)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(downloadpath));
		string[] item = new string[4]
		{
			link,
			downloadpath,
			expectedMd5,
			downloadsize
		};
		DownloadList.Add(item);
	}

	private static void Finished()
	{
		DeleteUnnecessary();
		AuthenticationManager._progress.Text = "Installing update...";
		CopyFiles(Path.Combine(gamepath, "Updates\\UberStrike"), gamepath);
<<<<<<< Updated upstream
		try
		{
			Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "UberStrike.exe")).WaitForExit();
		}
		catch (Exception e)
		{
			UnityEngine.Debug.LogError(e);
		}
		Application.Quit();
=======
>>>>>>> Stashed changes
	}

	private static void CopyFiles(string sourcePath, string destinationPath)
	{
		try
		{
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
			}
			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				try
				{
					File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
				}
				catch (Exception e)
				{
					UnityEngine.Debug.LogError(e);
				}
			}
		}
		catch (Exception e)
		{
			UnityEngine.Debug.LogError(e);
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
