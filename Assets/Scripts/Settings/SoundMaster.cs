using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class SoundMaster : MonoBehaviour {
    [Header("Mixer & Groups")]
    public AudioMixer mixer;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    [Header("Persistent Audio Sources")]
    public AudioSource musicSource; // for looping background music
    public AudioSource sfxSource;   // for global SFX (PlayOneShot)

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Music Controls
    // -------------------------
    void Awake() {
        LoadVolume("master", masterSlider);
        LoadVolume("music", musicSlider);
        LoadVolume("sfx", sfxSlider);
    }

    void LoadVolume(string key, Slider slider) {
        if (PlayerPrefs.HasKey(key)) {
            float linear = PlayerPrefs.GetFloat(key);
            slider.value = linear;
            mixer.SetFloat(key, Mathf.Log10(linear) * 20f);
        }
    }
    void Start() {
        PlayMusic(musicSource.clip);
    }
    public void PlayMusic(AudioClip clip, bool loop = true) {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic() {
        musicSource.Stop();
    }

    // Global SFX (1-shot)
    // -------------------------
    public void PlaySFX(AudioClip clip, float volume = 1f) {
        sfxSource.PlayOneShot(clip, volume);
    }

    // Positional / 3D SFX
    // -------------------------
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float spatialBlend = 1f) {
        GameObject temp = new GameObject("TempSFX");
        temp.transform.position = position;

        AudioSource audio = temp.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.outputAudioMixerGroup = sfxGroup;
        audio.volume = volume;
        audio.spatialBlend = spatialBlend; // 0 = 2D, 1 = 3D
        audio.Play();

        Destroy(temp, clip.length);
    }

    // Volume Sliders
    // -------------------------

    public void SetMasterVolume() => SetVolume("master", masterSlider);
    public void SetMusicVolume() => SetVolume("music", musicSlider);
    public void SetSFXVolume() => SetVolume("sfx", sfxSlider);

    void SetVolume(string key, Slider slider) {
        float linear = slider.value;
        mixer.SetFloat(key, Mathf.Log10(linear) * 20f);
        PlayerPrefs.SetFloat(key, linear);
        PlayerPrefs.Save();
    }

    private float LinearToDb(float linear) => Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
}