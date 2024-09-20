using UnityEngine;
using UnityEngine.Audio;

namespace SmartFPController
{
	public class GameSettings : ScriptableObject
	{
		[SerializeField]
		private bool invertLookX;

		[SerializeField]
		private bool invertLookY;

		[Range(1f, 10f)]
		[SerializeField]
		private float lookSensitivity = 1f;

		[Range(-80f, 0f)]
		[SerializeField]
		private float masterVolume;

		[Range(-80f, 0f)]
		[SerializeField]
		private float musicVolume;

		[Range(-80f, 0f)]
		[SerializeField]
		private float sfxVolume;

		[Range(-80f, 0f)]
		[SerializeField]
		private float voiceVolume;

		[SerializeField]
		private AudioMixer masterMixer;

		[SerializeField]
		private AudioMixerGroup sfxOutput;

		[SerializeField]
		private AudioMixerGroup musicOutput;

		[SerializeField]
		private AudioMixerGroup voiceOutput;

		private static GameSettings instance;

		private static GameSettings m_Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<GameSettings>("GameSettings");
				}
				return instance;
			}
		}

		public static bool InvertLookX
		{
			get
			{
				return m_Instance.invertLookX;
			}
			set
			{
				m_Instance.invertLookX = value;
			}
		}

		public static bool InvertLookY
		{
			get
			{
				return m_Instance.invertLookY;
			}
			set
			{
				m_Instance.invertLookY = value;
			}
		}

		public static float InvertLookPosNegValue_X
		{
			get
			{
				return (!m_Instance.invertLookX) ? 1f : (-1f);
			}
		}

		public static float InvertLookPosNegValue_Y
		{
			get
			{
				return (!m_Instance.invertLookY) ? 1f : (-1f);
			}
		}

		public static float LookSensitivity
		{
			get
			{
				return m_Instance.lookSensitivity;
			}
			set
			{
				m_Instance.lookSensitivity = value;
			}
		}

		public static float GetLookSensitivityByInvert_X
		{
			get
			{
				return (!m_Instance.invertLookX) ? m_Instance.lookSensitivity : (0f - m_Instance.lookSensitivity);
			}
		}

		public static float GetLookSensitivityByInvert_Y
		{
			get
			{
				return (!m_Instance.invertLookY) ? m_Instance.lookSensitivity : (0f - m_Instance.lookSensitivity);
			}
		}

		public static float MasterVolume
		{
			get
			{
				return m_Instance.masterVolume;
			}
			set
			{
				if (m_Instance.masterVolume != value)
				{
					m_Instance.masterVolume = value;
					SetVolumeByType(EVolumeType.Master, value);
				}
			}
		}

		public static float MusicVolume
		{
			get
			{
				return m_Instance.musicVolume;
			}
			set
			{
				if (m_Instance.musicVolume != value)
				{
					m_Instance.musicVolume = value;
					SetVolumeByType(EVolumeType.Music, value);
				}
			}
		}

		public static float SFXVolume
		{
			get
			{
				return m_Instance.sfxVolume;
			}
			set
			{
				if (m_Instance.sfxVolume != value)
				{
					m_Instance.sfxVolume = value;
					SetVolumeByType(EVolumeType.SFX, value);
				}
			}
		}

		public static float VoiceVolume
		{
			get
			{
				return m_Instance.voiceVolume;
			}
			set
			{
				if (m_Instance.voiceVolume != value)
				{
					m_Instance.voiceVolume = value;
					SetVolumeByType(EVolumeType.Voice, value);
				}
			}
		}

		public static AudioMixer MasterMixer
		{
			get
			{
				return m_Instance.masterMixer;
			}
		}

		public static AudioMixerGroup MusicOutput
		{
			get
			{
				return m_Instance.musicOutput;
			}
		}

		public static AudioMixerGroup SFXOutput
		{
			get
			{
				return m_Instance.sfxOutput;
			}
		}

		public static AudioMixerGroup VoiceOutput
		{
			get
			{
				return m_Instance.voiceOutput;
			}
		}

		public static void SetVolumeByType(EVolumeType volumeType, float value)
		{
			m_Instance.masterMixer.SetFloat(string.Concat(volumeType, "Volume"), value);
		}

		internal static void UpdateMixerVolumes()
		{
			SetVolumeByType(EVolumeType.Master, m_Instance.masterVolume);
			SetVolumeByType(EVolumeType.Music, m_Instance.musicVolume);
			SetVolumeByType(EVolumeType.SFX, m_Instance.sfxVolume);
			SetVolumeByType(EVolumeType.Voice, m_Instance.voiceVolume);
		}
	}
}
