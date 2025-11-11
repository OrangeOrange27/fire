using TMPro;
using UnityEngine;


[DefaultExecutionOrder(-999)]
public class CpuFrameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private float _lastTime;

    private static readonly char[] Prefix = "CPU Frame Time: ".ToCharArray();
    private static readonly char[] Suffix = " ms".ToCharArray();

    private readonly char[] _buffer = new char[Prefix.Length + 6 + 1 + 2 + Suffix.Length];

    private void Awake()
    {
        _lastTime = Time.realtimeSinceStartup;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Measure CPU frame time ignoring v-sync
        float currentTime = Time.realtimeSinceStartup;
        float frameMs = (currentTime - _lastTime) * 1000f;
        _lastTime = currentTime;

        WriteToBuffer(frameMs);
        _text.SetCharArray(_buffer);
    }

    private void WriteToBuffer(float ms)
    {
        int pos = 0;

        // prefix
        for (int i = 0; i < Prefix.Length; i++)
            _buffer[pos++] = Prefix[i];

        // round to 2 decimals
        int centi = Mathf.RoundToInt(ms * 100f);
        int integer = centi / 100;
        int frac = Mathf.Abs(centi % 100);

        // integer digits
        pos = WriteInt(_buffer, pos, integer);

        // decimal point
        _buffer[pos++] = '.';

        // two decimal digits (always 2)
        _buffer[pos++] = (char)('0' + (frac / 10));
        _buffer[pos++] = (char)('0' + (frac % 10));

        // suffix
        for (int i = 0; i < Suffix.Length; i++)
            _buffer[pos++] = Suffix[i];

        // null-terminate (TMP ignores but keeps safety)
        for (int i = pos; i < _buffer.Length; i++)
            _buffer[i] = '\0';
    }

    private static int WriteInt(char[] buffer, int pos, int value)
    {
        if (value == 0)
        {
            buffer[pos++] = '0';
            return pos;
        }

        int temp = value;
        int digits = 0;
        while (temp > 0)
        {
            digits++;
            temp /= 10;
        }

        int start = pos + digits - 1;
        temp = value;
        for (int i = 0; i < digits; i++)
        {
            int d = temp % 10;
            buffer[start - i] = (char)('0' + d);
            temp /= 10;
        }

        return pos + digits;
    }
}