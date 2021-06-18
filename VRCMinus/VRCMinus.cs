using System.Collections;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(VRCMinus), nameof(VRCMinus), "1.0.1", "Behemoth")]
[assembly: MelonGame("VRChat", "VRChat")]

public class VRCMinus : MelonMod {
    public override void OnApplicationStart()
        => MelonCoroutines.Start(WaitForUiManager());

    private readonly static string[] MainMenuDestroyTargets = {
        "Backdrop/Backdrop/Image",
        "Backdrop/Header/Tabs/ViewPort/Content/GalleryTab",
        "Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab",
        "Popups/SendInvitePopup/SendInviteMenu/SubscribeToAddPhotosButton",
        "Popups/RequestInvitePopup/RequestInviteMenu/SubscribeToAddPhotosButton",
        "Screens/Avatar/Vertical Scroll View/Viewport/Content/Favorite Avatar List/GetMoreFavorites",
        "Screens/UserInfo/AvatarImage/SupporterIcon",
        "Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/Supporter",
        "Screens/UserInfo/User Panel/Supporter",
        "Screens/UserInfo/User Panel/VRCPlusEarlyAdopter",
        "Screens/UserInfo/User Panel/VRCIcons",
        "Screens/UserInfo/SelfButtons/ChangeProfilePicButton",
    };

    private readonly static string[] QuickMenuDestroyTargets = {
        "ShortcutMenu/GalleryButton",
        "ShortcutMenu/HeaderContainer/VRCPlusBanner",
        "ShortcutMenu/UserIconButton",
        "ShortcutMenu/UserIconCameraButton",
        "ShortcutMenu/VRCPlusThankYou",
        "ShortcutMenu/VRCPlusMiniBanner",
        "QuickModeMenus/QuickModeInviteResponseMenu/SubscribeToAddPhotosButton",
        "QuickMenu_NewElements/_InfoBar/EarlyAccessText",
    };

    private readonly static string[] PlayerNameplateHideTargets = {
        "Icon/User Image",
        "Icon/Background",
        "Icon/Glow",
        "Icon/Pulse",
        "Main/Glow",
        "Main/Pulse",
    };

    private static IEnumerator WaitForUiManager() {
        /* Wait for VRCUiManager init */
        while (VRCUiManager.prop_VRCUiManager_0 == null)
            yield return new WaitForSeconds(1f);

        /* Destory all the shit on the quick menu */
        var quick_menu = GameObject.Find("UserInterface/QuickMenu").transform;
        foreach (var path in QuickMenuDestroyTargets)
            FindAndDestroy(quick_menu, path);

        /* Move the world info text to the left */
        var world_text = quick_menu.Find("QuickMenu_NewElements/_InfoBar/WorldText");
        world_text.localPosition = new Vector3(-420f, 93.85f);

        /* Destory all the shit on the main menu */
        var main_menu = GameObject.Find("UserInterface/MenuContent").transform;
        foreach (var path in MainMenuDestroyTargets)
            FindAndDestroy(main_menu, path);

        var social = main_menu.Find("Screens/Social/Vertical Scroll View/Viewport/Content").transform;

        /* Hide icon on the player picker */
        var picker = social.Find("OnlineFriends").GetComponent<UiUserList>().pickerPrefab.transform;
        var icon = picker.Find("Icons/GameObject/IconSupporter");
        icon.GetComponent<RawImage>().enabled = false;
        icon.SetAsLastSibling();

        /* Edit player template */
        while (SpawnManager.field_Private_Static_SpawnManager_0 == null)
            yield return new WaitForSeconds(1f);
        var target = SpawnManager.field_Private_Static_SpawnManager_0.field_Public_GameObject_0.transform.Find("Player Nameplate/Canvas/Nameplate/Contents");

        /* Hide annoying shit from the nameplate */
        foreach (var path in PlayerNameplateHideTargets)
            FindAndHide(target, path);
    }

    private static void FindAndDestroy(Transform parent, string path)
        => GameObject.Destroy(parent.Find(path)?.gameObject);

    private static void FindAndHide(Transform parent, string path) {
        var target = parent.Find(path);
        target.gameObject.active = false;
        target.localScale = Vector3.zero;
    }
}
