using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;

    private Tile currentTile;
    private Stage stage;
    private Animator animator;

    private bool isMoving;
    private Tile pendingTarget;

    private void Start()
    {
        var mapObject = GameObject.FindWithTag("Map");
        stage = mapObject.GetComponent<Stage>();

        currentTile = stage.Map.startTile;
        transform.position = stage.GetTilePos(currentTile.id);

        stage.OnTileVisited(currentTile.id);

        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }


    private void Update()
    {
        if (currentTile == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var targetTileId = stage.ScreenPosTileId(Input.mousePosition, false);
            if (isMoving)
            {
                StopAllCoroutines();
                currentTile = pendingTarget;
                StartCoroutine(CoMovePlayer(targetTileId));
            }
            else
            {
                StartCoroutine(CoMovePlayer(targetTileId));
            }
        }
    }

    private IEnumerator CoMovePlayer(int targetTileId)
    {
        var tiles = stage.Map.PathFinding(currentTile, stage.Map.tiles[targetTileId]);
        if (tiles != null)
        {
            foreach (var tile in tiles)
            {
                isMoving = true;
                animator.speed = 1f;
                pendingTarget = tile;
                yield return CoMoveOneStep(tile);
                stage.OnTileVisited(currentTile.id);
                animator.speed = 0f;
                isMoving = false;
            }
        }
        if (tiles == null && isMoving)
        {
            yield return CoMoveOneStep(pendingTarget);
            stage.OnTileVisited(currentTile.id);
            animator.speed = 0f;
            isMoving = false;
        }
    }

    private IEnumerator CoMoveOneStep(Tile next)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = stage.GetTilePos(next.id);
        float moveDuration = Vector3.Distance(startPos, targetPos) / moveSpeed;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;

        }
        transform.position = targetPos;
        currentTile = next;
    }
}
