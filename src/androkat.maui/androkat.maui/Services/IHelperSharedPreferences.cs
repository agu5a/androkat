namespace androkat.hu.Services;

public interface IHelperSharedPreferences
{
    void delete(string key);

    void putSharedPreferencesLong(string key, long value);

    void putSharedPreferencesInt(string key, int value);

    void putSharedPreferencesBoolean(string key, bool val);

    void putSharedPreferencesstring(string key, string val);

    long getSharedPreferencesLong(string key, long _default);

    string getSharedPreferencesstring(string key, string _default);

    int getSharedPreferencesInt(string key, int _default);

    int getTextSize();

    bool getSharedPreferencesBoolean(string key, bool _default);
}
