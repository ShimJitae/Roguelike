using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinimapSetter : MonoBehaviour
{
    // 미니맵 아이콘으로 그릴 오브젝트의 태그
    private const string PlayerTag = "Player";
    private const string MonsterTag = "Monster";
    private const string GroundTag = "Ground";

    // 미니맵 아이콘 태그 및 레이어
    private const string MinimapIconTag = "MinimapIcon";
    private const string MinimapIconLayer = "MinimapIcon";

    // 미니맵 카메라에 그릴 오브젝트
    [Header("Icon Prefabs")]
    [SerializeField] private GameObject iconPlayer;
    [SerializeField] private GameObject iconMonster;
    [SerializeField] private GameObject iconGround;

    // Collider2D 크기에 맞춰 계산한 아이콘 스케일에 곱할 배율 (그냥 1배로 사용)
    [Header("Icon Scale")]
    [SerializeField] private float scaleMultiplier = 1f;

    // 각각의 태그의 오브젝트들을 담을 리스트
    [Header("Found Objects")]
    private List<GameObject> playerObjs = new List<GameObject>();
    private List<GameObject> monsterObjs = new List<GameObject>();
    private List<GameObject> groundObjs = new List<GameObject>();

    // 미니맵 아이콘의 레이어
    private int minimapIconLayer;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SetupMinimapIcons();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupMinimapIcons();
    }

    /*
    씬이 로드될 때, 태그로 미니맵 아이콘을 그릴 오브젝트들을 찾고,
    하위 오브젝트에 MinimapIconTag 를 가지고 있지 않은 오브젝트는 아이콘 오브젝트를 하위에 만들어준다.
    아이콘 오브젝트는 해당 오브젝트의 콜라이더 크기로 설정해준다.
    */
    public void SetupMinimapIcons()
    {
        minimapIconLayer = LayerMask.NameToLayer(MinimapIconLayer);

        if (minimapIconLayer == -1)
        {
            Debug.LogError(
                $"'{MinimapIconLayer}' 레이어가 없습니다.",
                this
            );
            return;
        }

        // 아이콘 그릴 오브젝트의 리스트들 초기화
        playerObjs.Clear();
        monsterObjs.Clear();
        groundObjs.Clear();

        // 태그로 찾은 오브젝트들을 리스트에 저장해준다.
        playerObjs.AddRange(GameObject.FindGameObjectsWithTag(PlayerTag));
        monsterObjs.AddRange(GameObject.FindGameObjectsWithTag(MonsterTag));
        groundObjs.AddRange(GameObject.FindGameObjectsWithTag(GroundTag));

        // 각각의 리스트들의 오브젝트에 아이콘을 만들어준다.
        CreateIcons(playerObjs, iconPlayer);
        CreateIcons(monsterObjs, iconMonster);
        CreateIcons(groundObjs, iconGround);
    }

    private void CreateIcons(
        List<GameObject> targets,
        GameObject iconPrefab
    )
    {
        if (iconPrefab == null)
        {
            Debug.LogError("미니맵 아이콘 프리팹이 없습니다.", this);
            return;
        }

        foreach (GameObject target in targets)
        {
            CreateIcon(target, iconPrefab);
        }
    }

    private void CreateIcon(GameObject target, GameObject iconPrefab)
    {
        // target이 null이거나, 이미 미니맵 아이콘을 가지고 있다면 CreateIcon을 종료
        if (target == null || HasMinimapIcon(target))
        {
            return;
        }

        // 2D 횡스크롤이므로 Collider2D 사용
        Collider2D targetCollider = target.GetComponent<Collider2D>();

        // 타겟이 콜라이더를 가지고 있지 않으면 CreateIcon을 종료
        if (targetCollider == null)
        {
            Debug.LogError($"'{target.name}'에 Collider2D가 없습니다.", target);
            return;
        }

        // 타겟이 콜라이더가 비활성화 되어 있으면 CreateIcon을 종료
        if (!targetCollider.enabled)
        {
            Debug.LogError($"'{target.name}'의 Collider2D가 비활성화되어 있습니다.", target);
            return;
        }

        // 아이콘 프리팹을 target의 자식으로 생성 
        GameObject icon = Instantiate(iconPrefab, target.transform, false);

        icon.name = $"MinimapIcon_{target.name}";

        /*
         Collider2D bounds의 월드 중심을 target의 로컬 좌표로 변환하여 아이콘을 배치
         Collider 중심이 오브젝트 피벗과 다른 경우도 보정
         */
        Vector3 colliderCenter = target.transform.InverseTransformPoint(targetCollider.bounds.center);
        icon.transform.localPosition = new Vector3(colliderCenter.x, colliderCenter.y, 0f);

        // 생성한 아이콘과 모든 하위 오브젝트의 레이어를 MinimapIcon으로 설정 (아이콘 오브젝트는 자식 오브젝트 없어서 지워도 ㄱㅊ을듯)
        SetLayerRecursively(icon, minimapIconLayer);
        /*
        아이콘의 기본 표시 크기가 1x1이라는 전제로,
        아이콘의 월드 크기가 Collider2D bounds 크기에 맞도록 localScale을 변경
        */
        ScaleIconToCollider(icon, target, targetCollider);
    }

    private void ScaleIconToCollider(GameObject icon, GameObject target, Collider2D targetCollider)
    {
        /*
        Collider2D를 감싸는 월드 좌표 기준 bounds 크기 가져오기
        Collider2D.bounds.size는 월드 좌표 기준 크기입니다.
        */
        Vector3 worldSize = targetCollider.bounds.size;
        /*
        부모의 월드 스케일 가져오기
        아이콘은 target의 자식임
        따라서 아이콘의 최종 크기에는 target의 스케일도 함께 적용
        lossyScale은 현재 오브젝트의 부모 계층까지 모두 반영한 월드 기준 스케일
        lossyScale : 부모 계층까지 모두 반영된 “월드 기준 최종 스케일”
        */
        Vector3 parentScale = target.transform.lossyScale;
        // 아이콘의 X와 Y는 새롭게 계산하지만, Z 스케일은 프리팹에 설정된 값을 유지하기 위해 가져옴
        Vector3 iconScale = icon.transform.localScale;

        icon.transform.localScale = new Vector3(
        // scaleMultiplier는 최종 아이콘 크기 배율 (기본적으로 1배율)
                                                worldSize.x / SafeScale(parentScale.x) * scaleMultiplier,
                                                worldSize.y / SafeScale(parentScale.y) * scaleMultiplier,
                                                iconScale.z
                                                );
    }

    // 해당 오브젝트의 하위 오브젝트가 MinimapIcon 태그를 가진 오브젝트가 있는지 여부를 확인
    private bool HasMinimapIcon(GameObject target)
    {
        Transform[] children = target.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            // 본인 오브젝트는 확인하지 않음
            if (child == target.transform)
            {
                continue;
            }

            // 자식의 태그가 MinimapIconTag이면 true 반환
            if (child.CompareTag(MinimapIconTag))
            {
                return true;
            }
        }

        // 미니맵 아이콘 오브젝트가 없으면 false 반환
        return false;
    }

    private void SetLayerRecursively(GameObject target, int layer)
    {
        target.layer = layer;

        foreach (Transform child in target.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    /*
    SafeScale()을 사용하는 이유
    다음 두 문제를 방지합니다.
    1. Scale이 0일 때 0으로 나누는 문제
    2. 캐릭터 반전으로 X Scale이 -1일 때 음수 크기로 계산되는 문제
    */
    private float SafeScale(float value)
    {
        return Mathf.Max(Mathf.Abs(value), 0.0001f);
    }
}
