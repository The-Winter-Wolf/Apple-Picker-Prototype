using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    // Шаблон для создания яблок
    public GameObject       applePrefab;
    // Скорость движения яблони
    public float            speed = 1f;
    // Расстояние, на котором должно изменяться направление движения яблони
    public float            leftAndRightEdge = 25f;
    // Вероятность случайного изменения направления движения
    public float            chanceToChangeDirections = 0.1f;
    // Частота создания экземпляров яблок
    public float            secondsBetweenAppleDrops = 1f;
    // Сложность игры
    public int              difficulty = 1;

    public static int       dif = 1;

    public static bool      gameIsPaused = false;

    private Vector2         camMax;

    private Text            pauseGT;

    void Awake() // Вызывается при создании экземпляра класса, т.е перед Start
    {
        // Получить ссылку на игровой объект Pause
        GameObject pauseGO = GameObject.Find("Pause");
        // Получить компонент Text этого игрового объекта
        pauseGT = pauseGO.GetComponent<Text>();
        // Установить начальное число очков равным 0
        pauseGT.enabled = false;   

        // Вычислить расстояние перемещения дерева от ширины камеры (для разных экранов устройств)
        Vector2 camMax = Camera.main.ViewportToWorldPoint(new Vector2 (1,1));
        leftAndRightEdge = camMax.x - 3;

        // Изменение уровня сложности при ручном изменении
        if (difficulty > dif) {           
            speed += (difficulty-dif);
            if (secondsBetweenAppleDrops > 0.4f) {
                secondsBetweenAppleDrops -= 0.05f*(difficulty-dif);
            }
            if (secondsBetweenAppleDrops <= 0.4f) {
                secondsBetweenAppleDrops = 0.4f; 
            }
            dif = difficulty;
        }
    }
    
    void Start() // Обновляется до появления первого кадра
    {
        Invoke("DropApple", 2f);
    }

    void DropApple() 
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            pauseGT.enabled = true; 
        }
        else
        {
            Time.timeScale = 1f;
            pauseGT.enabled = false; 
        }
    }

    void Update() // Обновляется каждый кадр (частота зависит от быстродействия компьютера)
    {
        // Выйти из приложения
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        // Пауза игры
        if (Input.GetKeyUp("space"))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }

        if (gameIsPaused)
        {
            return;
        }

        // Простое перемещение
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Изменение направления
        if (pos.x > leftAndRightEdge) {
            speed = -Mathf.Abs(speed);                              // Начать движение влево
        } else if (pos.x < -leftAndRightEdge) {
            speed = Mathf.Abs(speed);                               // Начать движение вправо
        }

        // Изменение уровня сложности
        if (difficulty < dif) {           
            if (speed > 0) { speed += 1;}
            else if (speed < 0) { speed -= 1;}
            if (secondsBetweenAppleDrops > 0.4f) {
                secondsBetweenAppleDrops -= 0.05f;
            }
            if (secondsBetweenAppleDrops <= 0.4f) {
                secondsBetweenAppleDrops = 0.4f; 
            }
            difficulty = dif;
        }

    }

    void FixedUpdate() // Обновляется с фиксированной периодичностью 50 кадров/сек (для движущихся физических объектов)
    {
        if ( Random.value < chanceToChangeDirections) {
            speed *= -1;                                            // Случайно изменить направление
        }  
    }
}
