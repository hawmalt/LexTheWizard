﻿/*
 * Author(s): Isaiah Mann 
 * Description: Wrapper class to store a list of AudioFiles
 * Has array-like properties
 * Dependencies: AudioFile
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioList {

	Dictionary<AudioClip, AudioFile> clipToFileDictionary;

	public AudioList (AudioFile[] files) {
		Audio = files;
		InitClipDictionary();
		SubscribeEvents();
	}
		
	// Destructor for Garbage Collection:
	~AudioList () {
		UnsubscribeEvents();
	}
		
	public AudioFile[] Audio;

	public AudioFile this[int index] {
		get {
			return Audio[index];
		}
	}

	public int Length {
		get {
			if (Audio == null) { 
				return 0;
			} else {
				return Audio.Length;
			}
		}
	}

	public AudioType GetAudioType (AudioClip clip) {
		return AudioUtil.AudioTypeFromString(clipToFileDictionary[clip].Type);
	}

	void ProcessAudioFileAccess (AudioFile file) {
		AddToClipDictionary(file);
	}

	void AddToClipDictionary (AudioFile file) {
		if (file.Clip != null && !clipToFileDictionary.ContainsKey(file.Clip)) {
			clipToFileDictionary.Add(file.Clip, file);
		}

	}

	public void SubscribeEvents () {
		if (clipToFileDictionary == null) {
			InitClipDictionary();
		}

		for (int i = 0; i < Audio.Length; i++) {
			Audio[i].OnClipRequest += ProcessAudioFileAccess;
		}
	}

	void UnsubscribeEvents () {
		for (int i = 0; i < Audio.Length; i++) {
			Audio[i].OnClipRequest -= ProcessAudioFileAccess;
		}
	}

	void HandleClipRequest (AudioFile file) {
		ProcessAudioFileAccess(file);
	}

	void InitClipDictionary () {
		clipToFileDictionary = new Dictionary<AudioClip, AudioFile>();
	}
		
}
