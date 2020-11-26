using System;

public class Account
{
    public Guid Id { get; private set; }
    public string Pseudo { get; set; }
    public Account()
    {
        Id = Guid.NewGuid();
        Pseudo = $"Tombeur {UnityEngine.Random.Range(1000, 9999)}";;
    }
}
