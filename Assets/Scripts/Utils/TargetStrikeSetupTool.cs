#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TargetStrike.Core;
using TargetStrike.Player;
using TargetStrike.UI;

namespace TargetStrike.Editor
{
    public class TargetStrikeSetupTool : MonoBehaviour
    {
        private const string PlayerPrefabGuid = "6fd720100471c40eea62a411e90f0fd5";

        [MenuItem("Target Strike/Setup Current Scene")]
        public static void SetupScene()
        {
            SetupManagers();
            SetupUI();
            GameObject player = SetupPlayer();
            SetupWeapon(player);
            
            Debug.Log("[TARGET STRIKE] Scene Setup Complete! Weapon has been automatically attached.");
        }

        private static void SetupManagers()
        {
            if (Object.FindFirstObjectByType<GameManager>() == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }
            
            if (Object.FindFirstObjectByType<TimerManager>() == null)
            {
                GameObject tm = new GameObject("TimerManager");
                tm.AddComponent<TimerManager>();
            }
            
            if (Object.FindFirstObjectByType<LevelManager>() == null)
            {
                GameObject lm = new GameObject("LevelManager");
                lm.AddComponent<LevelManager>();
            }
        }

        private static void SetupUI()
        {
            if (Object.FindFirstObjectByType<UIManager>() != null) return;

            GameObject canvasObj = new GameObject("MainCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            UIManager uiManager = canvasObj.AddComponent<UIManager>();

            // Create HUD
            GameObject hud = new GameObject("HUD");
            hud.transform.SetParent(canvasObj.transform, false);

            uiManager.scoreText = CreateText(hud.transform, "ScoreText", "SCORE: 0", new Vector2(120, -60), new Vector2(0, 1));
            uiManager.timerText = CreateText(hud.transform, "TimerText", "00:00", new Vector2(0, -60), new Vector2(0.5f, 1));
            uiManager.ammoText = CreateText(hud.transform, "AmmoText", "30 / 90", new Vector2(-120, 60), new Vector2(1, 0));
            uiManager.targetCountText = CreateText(hud.transform, "TargetText", "TARGETS: 0/0", new Vector2(150, 60), new Vector2(0, 0));
            
            GameObject hm = new GameObject("HitMarker");
            hm.transform.SetParent(hud.transform, false);
            Image img = hm.AddComponent<Image>();
            img.color = new Color(1, 0, 0, 0.5f);
            hm.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            uiManager.hitMarker = hm;

            uiManager.winScreen = CreateScreen(canvasObj.transform, "WinScreen", "VICTORY", Color.green);
            uiManager.loseScreen = CreateScreen(canvasObj.transform, "LoseScreen", "GAME OVER", Color.red);
            
            uiManager.winScreen.SetActive(false);
            uiManager.loseScreen.SetActive(false);
        }

        private static GameObject CreateScreen(Transform parent, string name, string message, Color color)
        {
            GameObject screen = new GameObject(name);
            screen.transform.SetParent(parent, false);
            Image bg = screen.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform rect = screen.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            GameObject textObj = new GameObject("Message");
            textObj.transform.SetParent(screen.transform, false);
            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = message;
            tmp.fontSize = 60;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;

            return screen;
        }

        private static TextMeshProUGUI CreateText(Transform parent, string name, string text, Vector2 pos, Vector2 anchor)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 28;
            tmp.alignment = TextAlignmentOptions.Center;
            
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.anchoredPosition = pos;
            rect.sizeDelta = new Vector2(250, 60);
            
            return tmp;
        }

        private static GameObject SetupPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                string path = AssetDatabase.GUIDToAssetPath(PlayerPrefabGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                {
                    player = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    player.transform.position = Vector3.up;
                }
            }
            return player;
        }

        private static void SetupWeapon(GameObject player)
        {
            if (player == null) return;

            Camera cam = player.GetComponentInChildren<Camera>();
            if (cam == null) return;

            Transform container = cam.transform.Find("WeaponContainer");
            if (container == null)
            {
                GameObject go = new GameObject("WeaponContainer");
                go.transform.SetParent(cam.transform);
                go.transform.localPosition = new Vector3(0.2f, -0.3f, 0.4f);
                go.transform.localRotation = Quaternion.identity;
                container = go.transform;
            }

            if (container.childCount == 0)
            {
                string[] guids = AssetDatabase.FindAssets("P_LPSP_WEP_AR_01 t:Prefab");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    GameObject weaponPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    GameObject weapon = (GameObject)PrefabUtility.InstantiatePrefab(weaponPrefab, container);
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localRotation = Quaternion.identity;
                    
                    // CRITICAL: Destroy Infima's built-in Weapon script to prevent NullReferenceException
                    // We use our own PlayerShooting logic instead.
                    var infimaWeapon = weapon.GetComponent<InfimaGames.LowPolyShooterPack.Weapon>();
                    if (infimaWeapon != null) 
                    {
                        Object.DestroyImmediate(infimaWeapon);
                        Debug.Log("[SETUP TOOL] Permanently REMOVED Infima Weapon script to stop crashes.");
                    }
                    
                    var shooting = player.GetComponent<PlayerShooting>();
                    if (shooting != null) 
                    {
                        shooting.SetupWeapon(weapon);
                        // Ensure Hit Mask is set to Default/Everything
                        var serializedShooting = new SerializedObject(shooting);
                        var hitMaskProp = serializedShooting.FindProperty("hitMask");
                        if (hitMaskProp != null) hitMaskProp.intValue = -1; // -1 is Everything
                        serializedShooting.ApplyModifiedProperties();
                    }
                }
            }
        }
    }
}
#endif
