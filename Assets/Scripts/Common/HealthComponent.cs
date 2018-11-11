using UnityEngine;


public class HealthComponent : MonoBehaviour
{
    public float CurrentHeath;
    public float InitialHealth;
    public float MaxHealth;
    
    // Use this for initialization
    void Start()
    {
        CurrentHeath = InitialHealth;
        
    }
}