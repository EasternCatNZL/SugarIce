using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class MenuManagement : MonoBehaviour {

    public Image[] Buttons;
    public Sprite[] ButtonImages;
    private int CursorPosition = 0;

    public Image CreditScreen = null;
    private bool CreditsOn = false;

    public Image[] Cutscene;
    private int CutscenePosition = 0;
    private bool CutsceneOn = false;

    private GamePadState state;
    private GamePadState prevState;

    // Use this for initialization
    void Start () {
		if(Buttons.Length == 0)
        {
            Debug.LogError("There are no buttons for the menu manager");
        }
	}
	
	// Update is called once per frame
	void Update () {
        print(prevState.DPad.Down);
        prevState = state;
        state = GamePad.GetState(PlayerIndex.One);
        if (CutsceneOn && prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            CutscenePosition++;
            Cutscene[CutscenePosition - 1].color = new Color(CreditScreen.color.r, CreditScreen.color.g, CreditScreen.color.b, 0.0f);
            Cutscene[CutscenePosition].color = new Color(CreditScreen.color.r, CreditScreen.color.g, CreditScreen.color.b, 1.0f);

            if (CutscenePosition == Cutscene.Length - 1)
            {
                SceneManager.LoadScene(1);
            }
        }
        if (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed)
        {
            Buttons[CursorPosition].sprite = ButtonImages[CursorPosition * 2];
            CursorPosition += 1;
            if(CursorPosition == Buttons.Length)
            {
                CursorPosition = 0;
            }
            Buttons[CursorPosition].sprite = ButtonImages[CursorPosition * 2 + 1];
        }
        if (prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed)
        {
            Buttons[CursorPosition].sprite = ButtonImages[CursorPosition * 2];
            CursorPosition -= 1;
            if (CursorPosition < 0)
            {
                CursorPosition = Buttons.Length - 1;
            }
            Buttons[CursorPosition].sprite = ButtonImages[CursorPosition * 2 + 1];
        }
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            switch(CursorPosition)
            {
                case 0:
                    CutsceneOn = true;
                    Cutscene[CutscenePosition].color = new Color(CreditScreen.color.r, CreditScreen.color.g, CreditScreen.color.b, 1.0f);
                    break;
                case 1:
                    CreditScreen.color = new Color(CreditScreen.color.r, CreditScreen.color.g, CreditScreen.color.b, 1.0f);
                    CreditsOn = true;
                    break;
                case 2:
                    Application.Quit();
                    break;
                default: break;
            }
        }

        if (CreditsOn && prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            CreditScreen.color = new Color(CreditScreen.color.r, CreditScreen.color.g, CreditScreen.color.b, 0.0f);
            CreditsOn = false;
        }

    }
}
