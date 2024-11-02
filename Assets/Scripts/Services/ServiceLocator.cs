internal class ServiceLocator
{
    private static ServiceLocator locator = null;

    public static ServiceLocator Instance
    {
        get
        {
           if (locator == null)
           {
               locator = new ServiceLocator();
           }
           return locator;
        }
    }

    private ServiceLocator()
    {
    }

    private CharactersData _charactersData = null;
    private PlayerModel _playerModel = null;
    public PlayerModel GetPlayerModel()
    {
        if (_playerModel == null)
        {
            _playerModel = new PlayerModel();
        }
        return _playerModel;
    }

    public CharactersData GetCharactersData()
    {
        if (_charactersData == null)
        {
            _charactersData = new CharactersData();
        }
        return _charactersData;
    }
}
