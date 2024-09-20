using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SmartFPController
{
	public class MenuElements : MonoBehaviour
	{
		public enum EFirstPanel
		{
			Gameplay = 0,
			Audio = 1
		}

		public Slider lookSens;

		public Slider masterVol;

		public Slider musicVol;

		public Slider SFXVol;

		public Slider voiceVol;

		public Toggle invLookX;

		public Toggle invLookY;

		public EFirstPanel firstPanel;

		public GameObject gameplayPanel;

		public GameObject audioPanel;

		private SmartInputManager m_Input;

		public void SetActive(bool value)
		{
			if (value)
			{
				m_Input.UnblockCursor();
			}
			else
			{
				m_Input.BlockCursor();
			}
			base.gameObject.SetActive(value);
		}

		internal void AwakeMENU(SmartInputManager input)
		{
			m_Input = input;
		}

		private void Start()
		{
			if (firstPanel == EFirstPanel.Audio)
			{
				gameplayPanel.SetActive(false);
			}
			else
			{
				audioPanel.SetActive(false);
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				invLookX.isOn = GameSettings.InvertLookX;
				invLookY.isOn = GameSettings.InvertLookY;
				lookSens.value = GameSettings.LookSensitivity;
				masterVol.value = GameSettings.MasterVolume;
				musicVol.value = GameSettings.MusicVolume;
				SFXVol.value = GameSettings.SFXVolume;
				voiceVol.value = GameSettings.VoiceVolume;
			}
		}

		public void SetInvLookXIsOn(bool value)
		{
			GameSettings.InvertLookX = value;
		}

		public void SetInvLookYIsOn(bool value)
		{
			GameSettings.InvertLookY = value;
		}

		public void SetPBodyIsOn(bool value)
		{
		}

		public void SetLookSens(float value)
		{
			GameSettings.LookSensitivity = value;
		}

		public void SetMasterVolume(float value)
		{
			GameSettings.MasterVolume = value;
		}

		public void SetMusicVolume(float value)
		{
			GameSettings.MusicVolume = value;
		}

		public void SetSFXVolume(float value)
		{
			GameSettings.SFXVolume = value;
		}

		public void SetVoiceVolume(float value)
		{
			GameSettings.VoiceVolume = value;
		}

		public void UnPause()
		{
			m_Input.Pause();
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void StartReloadScene()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene("Mama23");
		}
	}
}
