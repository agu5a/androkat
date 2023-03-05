namespace androkat.hu.Services;

internal class HelperSharedPreferences : IHelperSharedPreferences
{
    public void delete(string key)
    {
        throw new NotImplementedException();
        //throw new NotImplementedException();
        //preferences.Edit().Remove(key).Apply();
    }

    public void putSharedPreferencesLong(string key, long value)
    {
        throw new NotImplementedException();
        //var edit = preferences.Edit();
        //edit.PutLong(key, value);
        //edit.Apply();
    }

    public void putSharedPreferencesInt(string key, int value)
    {
        throw new NotImplementedException();
        //var edit = preferences.Edit();
        //edit.PutInt(key, value);
        //edit.Apply();
    }

    public void putSharedPreferencesBoolean(string key, bool val)
    {
        Preferences.Set(key, val);
    }

    public void putSharedPreferencesstring(string key, string val)
    {
        throw new NotImplementedException();
        //var edit = preferences.Edit();
        //edit.Putstring(key, val);
        //edit.Apply();
    }

    public long getSharedPreferencesLong(string key, long _default)
    {
        throw new NotImplementedException();
        //return preferences.GetLong(key, _default);
    }

    public string getSharedPreferencesstring(string key, string _default)
    {
        var preferences = Preferences.Get(key, _default);
        return preferences;
    }

    public int getSharedPreferencesInt(string key, int _default)
    {
        throw new NotImplementedException();
        //return preferences.GetInt(key, _default);
    }

    public int getTextSize()
    {
        throw new NotImplementedException();
        //return PreferenceManager.GetDefaultSharedPreferences(mContext).GetInt("charSize3", 0);
    }

    public bool getSharedPreferencesBoolean(string key, bool _default)
    {
        var preferences = Preferences.Get(key, _default);
        return preferences;
    }
}
