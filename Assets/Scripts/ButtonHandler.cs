using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Button jump;
    [SerializeField] private Button dash;
    [SerializeField] private Button fire;
    [SerializeField] private Player player;

    internal bool isJumpButtonClicked;
    internal bool isDashButtonClicked;
    internal bool isFireButtonClicked;
    // Start is called before the first frame update
    void Start()
    {
        jump.onClick.AddListener(() => isJumpButtonClicked = true);
        dash.onClick.AddListener(() => isDashButtonClicked = true);
        fire.onClick.AddListener(() => player.BasicAttack());
    }

    // Update is called once per frame
    void Update()
    {
           
    }


}
