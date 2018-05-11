using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysCube : MonoBehaviour
{
    private SimonSaysGameBoard simonGameBoard;

    private MeshRenderer meshRenderer;
    public Material InactiveMaterial;
    public Material ActiveMaterial;

    public float ActiveDurationSeconds = 1f;
    public float CooldownDurationSeconds = 0.2f;

    private int cubeIndex;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = InactiveMaterial;

        simonGameBoard = GetComponentInParent<SimonSaysGameBoard>();
    }

    public void SetActiveForPlayerSequence()
    {
        StartCoroutine(SetActiveRoutine(ActiveDurationSeconds / 3f, CooldownDurationSeconds / 3f));
    }

    public IEnumerator SetActiveRoutine()
    {
        meshRenderer.material = ActiveMaterial;

        yield return new WaitForSeconds(ActiveDurationSeconds);

        meshRenderer.material = InactiveMaterial;

        yield return new WaitForSeconds(CooldownDurationSeconds);
    }

    public IEnumerator SetActiveRoutine(float activeDuration, float cooldownDuration)
    {
        meshRenderer.material = ActiveMaterial;

        yield return new WaitForSeconds(activeDuration);

        meshRenderer.material = InactiveMaterial;

        yield return new WaitForSeconds(cooldownDuration);
    }

    private void SetInactive()
    {
        meshRenderer.material = InactiveMaterial;
    }

    public void PlayerSelect()
    {
        SetActiveForPlayerSequence();

        simonGameBoard.RegisterInput(cubeIndex);
    }

    public void SetIndex(int i)
    {
        cubeIndex = i;
    }
}
