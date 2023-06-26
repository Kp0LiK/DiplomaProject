using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Canvas _canvas;

    private void Awake()
    {
        _image.DOFade(1, 0f);
    }

    private async void Start()
    {
        await _image.DOFade(0, 0.5f).AsyncWaitForCompletion();
    }

    public async Task LoadSceneAsync(string scene)
    {
        _canvas.sortingOrder = 10;
        await _image.DOFade(1, 0.5f).AsyncWaitForCompletion();
        await SceneManager.LoadSceneAsync(scene);
        await _image.DOFade(0, 0.5f).AsyncWaitForCompletion();
        _canvas.sortingOrder = 0;
    }
}