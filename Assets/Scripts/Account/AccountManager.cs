using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance;

    public Account Account { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Account = new Account();
    }
}
