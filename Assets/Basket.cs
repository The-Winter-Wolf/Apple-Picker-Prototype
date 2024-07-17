using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text             scoreGT;

    void Start()
    {
        // Получить ссылку на игровой объект ScoreCounter
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        // Получить компонент Text этого игрового объекта
        scoreGT = scoreGO.GetComponent<Text>();
        // Установить начальное число очков равным 0
        scoreGT.text = "0";    
    }

    void Update()
    {
        if (AppleTree.gameIsPaused)
        {
            return;           
        }

        // Получить текущие координаты указтеля мыши на экране из Input
        Vector3 mousePos2D = Input.mousePosition;

        // Координата Z камеры определяет, какдалеко в трехмерном пространстве находится указатель мыши
        mousePos2D.z = -Camera.main.transform.position.z;

        // Преобразовать точку на двухмерной плоскости экрана в трехмерные координаты игры
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Переместить корзину вдоль оси Х в координату Х указателя мыши
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos; 
    }

    private void OnCollisionEnter(Collision coll) {
        // Отыскать яблоко, попавшее в эту корзину
        GameObject collidedWith = coll.gameObject;
        if (collidedWith.tag == "Apple") {
            Destroy(collidedWith);
            // Преобразовать текст scoreGT в целое число
            int score = int.Parse( scoreGT.text);
            // Добавить очки за пойманное яблоко
            score += 100;
            // Преобразовать число обратно в строку и вывести её на экран
            scoreGT.text = score.ToString();
            // Запомнить высшее достижение
            if (score > HighScore.score) {
                HighScore.score = score;
            }

            // Увеличить сложность за каждые 1000 очков
            if (score % 1000 == 0){
            AppleTree.dif += 1;
            Debug.Log (AppleTree.dif);
            }
        }
    }
}
