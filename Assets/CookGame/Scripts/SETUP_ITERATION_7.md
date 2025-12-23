# Итерация 7: Прогрессия и сохранение

## 🎯 Что нового

### PlayerProgressManager
- Система уровней (1-50)
- Накопление опыта (XP)
- Streak бонусы за серию побед
- Сохранение прогресса в PlayerPrefs

### RecipeUnlockManager
- Разблокировка рецептов по уровню
- Фильтрация доступных рецептов

### LevelUpPopup
- Попап при повышении уровня
- Показ новых разблокированных рецептов
- Анимации и эффекты

### PlayerStatsPanel
- Полная статистика игрока
- Прогресс уровня
- Достижения и рекорды


## 📁 Файлы

```
Assets/
├── Scripts/
│   ├── PlayerProgressManager.cs   ← НОВЫЙ
│   ├── RecipeUnlockManager.cs     ← НОВЫЙ
│   ├── LevelUpPopup.cs            ← НОВЫЙ
│   ├── PlayerStatsPanel.cs        ← НОВЫЙ
│   ├── RecipeData.cs              ← ОБНОВЛЁН
│   ├── CookingManager.cs          ← ОБНОВЛЁН
│   ├── MenuPanel.cs               ← ОБНОВЛЁН
│   └── OrderManager.cs            ← ОБНОВЛЁН
│
└── Editor/
    ├── PlayerStatsPanelCreator.cs  ← НОВЫЙ
    ├── LevelUpPopupCreator.cs      ← НОВЫЙ
    └── MenuLevelDisplayCreator.cs  ← НОВЫЙ
```


## 🔧 Настройка

### 1. Скопируй скрипты

- Все `.cs` из Scripts → `Assets/Scripts/`
- Все `.cs` из Editor → `Assets/Editor/`


### 2. Создай PlayerProgressManager

1. Создай пустой GameObject: **PlayerProgressManager**
2. Добавь компонент `PlayerProgressManager.cs`
3. Настрой параметры:

| Параметр | Значение | Описание |
|----------|----------|----------|
| Base XP Per Level | 100 | XP для 1 уровня |
| XP Scaling Factor | 1.5 | Множитель на каждый уровень |
| Max Level | 50 | Максимальный уровень |
| XP Per Order | 10 | Базовый XP за заказ |
| XP Bonus Perfect | 25 | Бонус за PERFECT |
| XP Bonus Good | 15 | Бонус за GOOD |
| XP Bonus Okay | 5 | Бонус за OKAY |
| XP Bonus Streak | 5 | Бонус за каждый streak |


### 3. Создай RecipeUnlockManager

1. Создай пустой GameObject: **RecipeUnlockManager**
2. Добавь компонент `RecipeUnlockManager.cs`
3. Перетащи все RecipeData в список **All Recipes**


### 4. Обнови RecipeData ScriptableObjects

В каждом рецепте установи **Unlock Level**:

| Сложность | Unlock Level |
|-----------|--------------|
| Easy | 1-5 |
| Medium | 5-15 |
| Hard | 15-30 |
| Elite | 30-50 |

Пример:
- Simple Soup (Easy) → Level 1
- Magic Stew (Medium) → Level 5
- Fire Dragon Dish (Hard) → Level 15
- Divine Feast (Elite) → Level 35


## 🎨 Создание UI через Editor скрипты

После копирования Editor скриптов в меню Unity появится **Probability Kitchen**.


### 5. Создай PlayerStatsPanel UI

1. Меню: **Probability Kitchen → Create PlayerStatsPanel UI**
2. ✅ Автоматически создаётся полная панель
3. Добавь компонент `PlayerStatsPanel.cs` на созданный объект
4. Присвой ссылки в Inspector:

| Поле | GameObject |
|------|------------|
| Level Text | PlayerStatsPanel/Container/LevelText |
| XP Slider | PlayerStatsPanel/Container/XPSlider |
| XP Text | PlayerStatsPanel/Container/XPText |
| XP To Next Text | PlayerStatsPanel/Container/XPToNextText |
| Orders Completed Text | .../OrdersCompletedRow/OrdersCompletedText |
| Orders Failed Text | .../OrdersFailedRow/OrdersFailedText |
| Success Rate Text | .../SuccessRateRow/SuccessRateText |
| Perfect Orders Text | .../PerfectOrdersRow/PerfectOrdersText |
| Current Streak Text | .../CurrentStreakRow/CurrentStreakText |
| Highest Streak Text | .../HighestStreakRow/HighestStreakText |
| Jackpots Text | .../JackpotsRow/JackpotsText |
| Total XP Text | .../TotalXPRow/TotalXPText |
| Next Unlock Text | .../NextUnlockText |
| Levels Until Unlock Text | .../LevelsUntilUnlockText |
| Close Button | .../CloseButton |
| Reset Button | .../ResetButton |


### 6. Создай LevelUpPopup UI

1. Меню: **Probability Kitchen → Create LevelUpPopup UI**
2. ✅ Автоматически создаётся попап с золотой рамкой
3. Добавь компонент `LevelUpPopup.cs` на созданный объект
4. Присвой ссылки в Inspector:

| Поле | GameObject |
|------|------------|
| Popup Panel | LevelUpPopup/PopupPanel |
| Level Text | .../InnerPanel/LevelText |
| Congrats Text | .../InnerPanel/CongratsText |
| Unlocks Text | .../InnerPanel/UnlocksText |
| Continue Button | .../InnerPanel/ContinueButton |
| Confetti Particles | (опционально) |


### 7. Добавь Level Display в MenuPanel

1. Выбери **MenuPanel** в Hierarchy
2. Меню: **Probability Kitchen → Create Menu Level Display UI**
3. ✅ Автоматически создаётся блок с уровнем и XP
4. В компоненте `MenuPanel.cs` присвой ссылки:

| Поле | GameObject |
|------|------------|
| Level Text | MenuPanel/LevelDisplay/LevelText |
| XP Slider | MenuPanel/LevelDisplay/XPSlider |
| XP Text | MenuPanel/LevelDisplay/XPText |
| Streak Text | MenuPanel/LevelDisplay/StreakText |
| Stats Button | (создай кнопку вручную) |
| Stats Panel | PlayerStatsPanel |


### 8. Создай Stats Button в MenuPanel

1. В MenuPanel создай Button: **StatsButton**
2. Текст: "📊" или "STATS"
3. Позиция: рядом с другими кнопками
4. Присвой в MenuPanel → Stats Button


## 📊 Система XP

### Награды за заказ:

| Результат | XP |
|-----------|-----|
| Базово | +10 |
| + PERFECT | +25 |
| + GOOD | +15 |
| + OKAY | +5 |
| + Streak (за каждый) | +5 |

### Пример:
- PERFECT с streak 3 = 10 + 25 + 5×2 = **45 XP**

### Формула XP для уровня:
```
XP_needed = BaseXP × (ScalingFactor ^ (Level - 1))
```

Пример с Base=100, Scaling=1.5:
- Level 1: 100 XP
- Level 2: 150 XP
- Level 3: 225 XP
- Level 5: 506 XP
- Level 10: 3844 XP


## 📈 Streak система

- PERFECT или GOOD → streak +1
- OKAY или FAILED → streak = 0
- Streak > 1 → бонус XP за каждый уровень streak
- Highest streak сохраняется


## 🧪 Тестирование

1. **Базовый прогресс:**
   - Заверши заказ PERFECT → XP увеличился
   - Смотри MenuPanel → уровень и XP бар

2. **Level Up:**
   - Набери достаточно XP
   - ✅ Появился LevelUpPopup
   - ✅ Показаны новые рецепты (если есть)

3. **Streak:**
   - Сделай 3 PERFECT подряд
   - ✅ StreakText показывает "🔥 3"
   - ✅ Бонус XP увеличивается

4. **Разблокировка:**
   - На Level 1 доступны только Easy рецепты
   - Повысь уровень → появляются новые

5. **Статистика:**
   - Нажми Stats Button
   - ✅ Открылась PlayerStatsPanel
   - ✅ Все данные корректны

6. **Сохранение:**
   - Закрой игру
   - Открой снова
   - ✅ Уровень и статистика сохранились


## 📋 Чек-лист

- [ ] Скрипты скопированы в Assets/Scripts/
- [ ] Editor скрипты скопированы в Assets/Editor/
- [ ] PlayerProgressManager создан и настроен
- [ ] RecipeUnlockManager создан, рецепты добавлены
- [ ] RecipeData обновлены с unlockLevel
- [ ] PlayerStatsPanel UI создан через меню
- [ ] LevelUpPopup UI создан через меню
- [ ] Menu Level Display добавлен в MenuPanel
- [ ] Все ссылки присвоены в Inspector
- [ ] Тест: XP начисляется
- [ ] Тест: Level Up popup работает
- [ ] Тест: Статистика сохраняется


## ⚠️ Важно

- **Все менеджеры опциональны** - игра работает без них
- **RecipeData.unlockLevel = 1** по умолчанию (все доступны)
- **Streak сбрасывается** при OKAY и FAILED
- **LevelUpPopup поддерживает очередь** - если несколько level up сразу
- **Editor скрипты создают только визуал** - компоненты и ссылки добавляй вручную


## 🐛 Возможные проблемы

**Меню Probability Kitchen не появляется:**
- Убедись что Editor скрипты в папке Assets/Editor/
- Перезапусти Unity

**XP не начисляется:**
- Проверь что PlayerProgressManager создан
- CookingManager должен вызывать RecordOrderComplete()

**Рецепты не разблокируются:**
- Проверь что RecipeUnlockManager создан
- Проверь unlockLevel в RecipeData

**LevelUp popup не появляется:**
- Проверь что LevelUpPopup.cs добавлен
- Проверь что ссылки присвоены


## 📊 Статус разработки

### ✅ Завершено:
- Итерация 1: Система заказов
- Итерация 2: Система готовки
- Итерация 3: Fire Boost
- Итерация 4: Jackpot + Currency + Early Completion
- Итерация 5: Визуальные эффекты
- Итерация 6: Звуки и Haptic Feedback
- Итерация 7: Прогрессия и сохранение (текущая)

### 🔄 Следующие:
- Итерация 8: Магазин и Gems
- Итерация 9: Daily rewards
- Итерация 10: Achievements
