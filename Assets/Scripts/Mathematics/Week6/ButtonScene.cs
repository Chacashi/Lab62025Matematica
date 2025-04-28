using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    Button myButton;


    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    private void Start()
    {
  
        myButton.onClick.AddListener(Restart);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
        
    }
}   
