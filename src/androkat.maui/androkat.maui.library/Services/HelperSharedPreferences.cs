using androkat.maui.library.Abstraction;

namespace androkat.maui.library.Services;

internal class HelperSharedPreferences : IHelperSharedPreferences
{
    public void Delete(string key)
    {
        throw new NotImplementedException();
    }

    public void PutSharedPreferencesLong(string key, long value)
    {
        throw new NotImplementedException();
    }

    public void PutSharedPreferencesInt(string key, int value)
    {
        throw new NotImplementedException();
    }

    public void PutSharedPreferencesBoolean(string key, bool val)
    {
        Preferences.Set(key, val);
    }

    public void PutSharedPreferencesstring(string key, string val)
    {
        throw new NotImplementedException();
    }

    public long GetSharedPreferencesLong(string key, long _default)
    {
        throw new NotImplementedException();
    }

    public string GetSharedPreferencesstring(string key, string _default)
    {
        var preferences = Preferences.Get(key, _default);
        return preferences;
    }

    public int GetSharedPreferencesInt(string key, int _default)
    {
        throw new NotImplementedException();
    }

    public int GetTextSize()
    {
        throw new NotImplementedException();
    }

    public bool GetSharedPreferencesBoolean(string key, bool _default)
    {
        var preferences = Preferences.Get(key, _default);
        return preferences;
    }
}
