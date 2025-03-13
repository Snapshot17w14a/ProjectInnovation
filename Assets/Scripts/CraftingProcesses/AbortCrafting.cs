using UnityEngine;
using UnityEngine.SceneManagement;

public class AbortCrafting : MonoBehaviour
{
    [SerializeField] private GameObject abortmenu;

   public void OpenAbortMenu()
   {
        abortmenu.SetActive(true);
   }

    public void CloseAbortMenu()
    {
        abortmenu.SetActive(false);
    }

    public void ConfirmAbortCrafting()
    {
        Destroy(GameObject.Find("CraftingManager(Clone)"));
        SceneManager.LoadScene("BattleScene");
    }
}
