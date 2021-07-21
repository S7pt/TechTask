using UnityEngine;

public class RoadScript : MonoBehaviour
{
    private static RoadScript _roadInstance;
    private float _offset = 0.15f;

    private void Awake()
    {
        if (_roadInstance != null && _roadInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.root);
            _roadInstance = this;
        }
    }

    private void Start()
    {
        PlayerScript.SubscribeToSizeChange(SetScale);
    }

    public void SetScale(Vector3 scaleOfPlayer)
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y,scaleOfPlayer.z-_offset);
    }

}
