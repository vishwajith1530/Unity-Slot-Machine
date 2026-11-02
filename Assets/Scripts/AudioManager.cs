
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource effectsSource;
    private AudioSource spinSource;

    private AudioClip clickClip;
    private AudioClip leverClip;
    private AudioClip spinClip;
    private AudioClip stopClip;
    private AudioClip winClip;
    private AudioClip loseClip;
    private AudioClip warningClip;
    private AudioClip resetClip;
    private AudioClip selectClip;
    private AudioClip startClip;

    private const int SampleRate = 44100;

    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        effectsSource = sources[0];

        spinSource = sources.Length > 1
            ? sources[1]
            : gameObject.AddComponent<AudioSource>();

        effectsSource.playOnAwake = false;
        effectsSource.loop = false;
        effectsSource.spatialBlend = 0f;
        effectsSource.volume = 0.8f;

        spinSource.playOnAwake = false;
        spinSource.loop = true;
        spinSource.spatialBlend = 0f;
        spinSource.volume = 0.35f;

        clickClip = MakeTone("Click", 0.08f, 950f, 550f, 0.3f);
        leverClip = MakeTone("Lever", 0.25f, 180f, 650f, 0.45f);
        spinClip = MakeNoise("Spin", 0.7f, 0.12f);
        stopClip = MakeTone("ReelStop", 0.16f, 480f, 150f, 0.4f);
        winClip = MakeWin();
        loseClip = MakeTone("Lose", 0.5f, 330f, 130f, 0.35f);
        warningClip = MakeTone("Warning", 0.35f, 700f, 350f, 0.4f);
        resetClip = MakeTone("Reset", 0.35f, 400f, 850f, 0.4f);
        selectClip = MakeTone("Select", 0.12f, 600f, 1000f, 0.35f);
        startClip = MakeTone("Start", 0.5f, 300f, 750f, 0.4f);
    }

    public void PlayClick() => PlayEffect(clickClip);
    public void PlayLever() => PlayEffect(leverClip);
    public void PlayReelStop() => PlayEffect(stopClip);
    public void PlayWin() => PlayEffect(winClip);
    public void PlayLose() => PlayEffect(loseClip);
    public void PlayWarning() => PlayEffect(warningClip);
    public void PlayReset() => PlayEffect(resetClip);
    public void PlaySelect() => PlayEffect(selectClip);
    public void PlayStart() => PlayEffect(startClip);

    public void StartSpin()
    {
        if (spinSource != null && spinClip != null)
        {
            spinSource.clip = spinClip;
            spinSource.Play();
        }
    }

    public void StopSpin()
    {
        if (spinSource != null && spinSource.isPlaying)
            spinSource.Stop();
    }

    private void PlayEffect(AudioClip clip)
    {
        if (effectsSource != null && clip != null)
            effectsSource.PlayOneShot(clip);
    }

    private AudioClip MakeTone(
        string clipName,
        float duration,
        float startFrequency,
        float endFrequency,
        float volume)
    {
        int count = Mathf.CeilToInt(SampleRate * duration);
        float[] samples = new float[count];

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;
            float frequency = Mathf.Lerp(startFrequency, endFrequency, t);
            float envelope = Mathf.Sin(Mathf.PI * t);

            samples[i] =
                Mathf.Sin(2f * Mathf.PI * frequency * i / SampleRate)
                * envelope * volume;
        }

        return CreateClip(clipName, samples);
    }

    private AudioClip MakeNoise(
        string clipName,
        float duration,
        float volume)
    {
        int count = Mathf.CeilToInt(SampleRate * duration);
        float[] samples = new float[count];

        System.Random random = new System.Random(12345);

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;
            float fade = Mathf.Sin(Mathf.PI * t);
            float noise = (float)(random.NextDouble() * 2.0 - 1.0);

            samples[i] = noise * fade * volume;
        }

        return CreateClip(clipName, samples);
    }

    private AudioClip MakeWin()
    {
        float duration = 0.9f;
        int count = Mathf.CeilToInt(SampleRate * duration);
        float[] samples = new float[count];

        float[] notes = { 523.25f, 659.25f, 783.99f, 1046.5f };

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / SampleRate;
            int noteIndex = Mathf.Min(3, Mathf.FloorToInt(t / 0.2f));
            float localT = (t - noteIndex * 0.2f) / 0.2f;
            float envelope = Mathf.Sin(Mathf.PI * localT);

            samples[i] =
                Mathf.Sin(2f * Mathf.PI * notes[noteIndex] * t)
                * envelope * 0.35f;
        }

        return CreateClip("Win", samples);
    }

    private AudioClip CreateClip(string clipName, float[] samples)
    {
        AudioClip clip = AudioClip.Create(
            clipName,
            samples.Length,
            1,
            SampleRate,
            false
        );

        clip.SetData(samples, 0);

        return clip;
    }
}
