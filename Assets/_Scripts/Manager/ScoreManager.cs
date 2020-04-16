using System;

public class ScoreManager
{
    // readonly 키워드를 붙인 변수는 변수를 처음 선언할 때와,
    // 생성자를 실행할 때 외에는 절로 값을 변경할 수가 없습니다
    readonly int scorePerMissile;
    readonly int scorePerBuilding;

    int score;

    public Action<int> ScoreChanged;

    public ScoreManager(int scorePerMissile = 50, int scorePerBuilding = 5000)
    {
        this.scorePerMissile = scorePerMissile;
        this.scorePerBuilding = scorePerBuilding;
    }

    public void OnMissileDestroyed()
    {
        score += scorePerMissile;
        ScoreChanged?.Invoke(score);
    }
}
