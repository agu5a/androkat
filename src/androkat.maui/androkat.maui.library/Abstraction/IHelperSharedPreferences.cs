namespace androkat.maui.library.Abstraction;

public interface IHelperSharedPreferences
{
    void Delete(string key);

    void PutSharedPreferencesLong(string key, long value);

    void PutSharedPreferencesInt(string key, int value);

    void PutSharedPreferencesBoolean(string key, bool val);

    void PutSharedPreferencesstring(string key, string val);

    long GetSharedPreferencesLong(string key, long _default);

    string GetSharedPreferencesstring(string key, string _default);

    int GetSharedPreferencesInt(string key, int _default);

    int GetTextSize();

    bool GetSharedPreferencesBoolean(string key, bool _default);
}
