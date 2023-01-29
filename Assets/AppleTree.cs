using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    static public int       dif = 1;

    private Vector2          camMax;

    void Awake() // Вызывается при создании экземпляра класса, т.е перед Start
    {
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

    void Update() // Обновляется каждый кадр (частота зависит от быстродействия компьютера)
    {
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
