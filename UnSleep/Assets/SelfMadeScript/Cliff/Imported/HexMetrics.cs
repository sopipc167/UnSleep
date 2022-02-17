using UnityEngine;

public static class HexMetrics { //나중에 육각형 타일의 좌표를 쉽게 정희하기 위해서 육각 타일의 내접원을 정의하고, 육각타일의 여섯 방향 위치를 미리 정의함

	public const float outerRadius = 0.52f;

	public const float innerRadius = outerRadius * 0.866025404f;

	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)
	};
}