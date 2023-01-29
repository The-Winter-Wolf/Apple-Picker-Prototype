using System.Collections;
using System.Collections.Generic;
using UnityEngine;                  // Необходима для работы с Unity
using UnityEngine.UI;               // Необходима для работы с интерфейсом пользователя

public class HighScore : MonoBehaviour
{
    static public int       score = 1000;

    void Awake() // Вызывается при создании экземпляра класса, т.е перед Start
    {
        // Если значение HighScore уже существует в PlayerPrefs, прочитать его
        if (PlayerPrefs.HasKey("HighScore")) {
            score = PlayerPrefs.GetInt("HighScore");
        }
        // Сохранить высшее достижение HighScore в хранилище PlayerPrefs
        PlayerPrefs.SetInt("HighScore", score);
    }

    void Update() // Обновляется в каждый кадр
    {
        // Отобразить значение score на UI
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + score;

        // Обновить Highscore в PlayerPrefs, если необходимо
        if (score > PlayerPrefs.GetInt("Highscore")) {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
