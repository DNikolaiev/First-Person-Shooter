using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioController : MonoBehaviour {
	public AudioClip[] windSounds;
	public AudioClip[] ambienceSounds;
	public AudioClip[] healthSounds;
	public AudioClip[] armorSounds;
	public AudioClip[] equipmentSounds;
    public AudioClip[] battleSounds;
    
	public  AudioSource wind;
	public  AudioSource ambience;
	public  AudioSource background;
	public  AudioSource weapon;
	public  AudioSource reload;
	public  AudioSource hitEffects;
	public  AudioSource enemy;
	public  AudioSource player;
	public  AudioSource heartBeat;
	public  AudioSource other;
	public  AudioSource explosion;

    private AudioClip[] currentSoundtracks;
    private bool battleStateChanged = false;
    private bool isChecking = false;

	[SerializeField] private float pauseTime;
	private float nextTime=0;
	// Use this for initialization

	public static AudioController instance=null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);


	}

	void Start()
	{
		PlayRandomSound (windSounds, wind);
		PlayRandomSound (ambienceSounds, ambience);
		pauseTime = 120;
		wind.loop = true;
		ambience.loop = true;

	}
    
	public IEnumerator ShiftBetweenAmbience( AudioClip[] toClip)
	{
		float volume = ambience.volume;
        while (ambience.volume > 0)
        {
            ambience.volume -= 0.2f;
            yield return new WaitForSeconds(0.3f);
        }
		if (ambience.volume == 0) {
            PlayRandomSound(toClip, ambience);
            while (ambience.volume < volume)
            {
                ambience.volume += 0.01f;
                yield return new WaitForSeconds(0.4f);
            }
        }
        yield return null;
	}




    void Update()
    {
        if (!isChecking)
            StartCoroutine("CheckBattleState");

        if (PlayerSettings.instance.isInBattle)
        { 
        currentSoundtracks = battleSounds;
        wind.clip = null;
        }
        else currentSoundtracks = ambienceSounds;

		if (Time.time > nextTime) {
			nextTime = Time.time + pauseTime;
			PlayRandomSound (windSounds, wind);
            StartCoroutine("ShiftBetweenAmbience", currentSoundtracks);
		}
        if (battleStateChanged)
        {
            StartCoroutine("ShiftBetweenAmbience", currentSoundtracks);
            battleStateChanged = false;
        }
        
	}
   private IEnumerator CheckBattleState()
    {
        isChecking = true;
        bool state = PlayerSettings.instance.isInBattle;
        yield return new WaitForSeconds(3f);
        if (PlayerSettings.instance.isInBattle == state)
        {
            battleStateChanged = false;
        }
        else
        {
            battleStateChanged = true;
        }
        isChecking = false;
        yield return null;
    }
	public void PlaySound(AudioClip clip, AudioSource source, float pitch=1, float volume=1)
	{
		source.clip = clip;
		source.pitch = pitch;
		source.volume = volume;
		source.Play ();
	}

	public void PlayRandomSound(AudioClip[] clips, AudioSource source, float pitch=1, float volume=1)
	{
		int randClip = Random.Range (0, clips.Length);
		PlaySound (clips [randClip], source,pitch,volume);
	}

	public void PlayLoopSound(AudioClip clip, AudioSource source, float pitch=1, float volume=1)
	{
		source.loop = true;
		PlaySound (clip, source, pitch, volume);
	}

	public void StopLoopSound(AudioSource source)
	{
		source.loop = false;
	}
}
