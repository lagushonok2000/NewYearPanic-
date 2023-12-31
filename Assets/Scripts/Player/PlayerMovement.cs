using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CounterKristall _counterKristall;
    [SerializeField] private float  _playerSpeed;
    [SerializeField] private LevelsSO _config;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _soundKristall;
    [SerializeField] private AudioSource _soundAnimator;
    private Vector3 _move;
    private Vector3 _velocity;
    private const int _layerKristall = 6;
    private float g = -9.8f;
    private CharacterController _controller;

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().buildIndex == 0 )
        {
            _playerSpeed = _config.PlayerSpeed[SaveGame.Data.CurrentLevel];
        }
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _layerKristall)
        {
            int ID = other.GetComponent<Kristall>().ID;
            _counterKristall.AddKristall(ID);
            _soundKristall.Play();
            other.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        _move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _move.Normalize();
        _move *= _playerSpeed;

        if (_move == Vector3.zero)
        {
            _animator.SetBool("move", false);
            _soundAnimator.Play();
        }
        else
        {
            _animator.SetBool("move", true);
        }
   
        _velocity = new Vector3(0, g,0);
        _controller.Move((_move + _velocity) * Time.deltaTime);
    
        if (_move != Vector3.zero)
        {
            transform.forward = _move;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame.Data.CurrentIndexScene = SceneManager.GetActiveScene().buildIndex;
        SaveGame.SaveData();
    }
}