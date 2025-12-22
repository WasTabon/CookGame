# Итерация 4: Jackpot System + Early Completion + Currency

## 🎯 Что нового в этой итерации

### 1. Jackpot System
- Каждые 5-10 роллов гарантированный джекпот
- Также срабатывает при 3 одинаковых ингредиентах
- 4 эффекта на выбор игрока:
  - **Meter Boost**: +10 ко всем метрам мгновенно
  - **Wild Multiplier**: x2 эффект следующего выбранного ингредиента
  - **Zone Shield**: защита одного метра от overflow (игрок выбирает какой)
  - **Triple Apply**: применить все 3 ингредиента сразу (рискованно!)

### 2. Early Completion (Serve Now)
- Кнопка досрочного завершения заказа
- Доступна после выбора минимум 1 ингредиента
- Расчёт награды по количеству метров в целевой зоне:
  - 3 метра = 100% (Perfect)
  - 2 метра = 60% (Good)
  - 1 метр = 30% (Okay)
  - 0 метров = 10% (Failed)
- Бонус за оставшиеся ходы: +5% за каждый (макс +25%)

### 3. Currency System
- **Coins** - основная валюта, награда за заказы
- **Gems** - премиум валюта (для будущих механик)
- Сохранение между сессиями (PlayerPrefs)


## 📁 Структура файлов

Скопируй все .cs файлы в `Assets/Scripts/`:

```
Assets/
├── Scripts/
│   ├── CurrencyManager.cs        ← НОВЫЙ
│   ├── CurrencyDisplay.cs        ← НОВЫЙ
│   ├── JackpotController.cs      ← НОВЫЙ
│   ├── ShieldController.cs       ← НОВЫЙ
│   ├── RewardCalculator.cs       ← НОВЫЙ
│   ├── CookingManager.cs         ← ОБНОВЛЁН
│   ├── CookingPanel.cs           ← ОБНОВЛЁН
│   ├── ResultPanel.cs            ← ОБНОВЛЁН
│   ├── MenuPanel.cs              ← ОБНОВЛЁН
│   ├── GameManager.cs            ← ОБНОВЛЁН
│   ├── UIManager.cs              ← ОБНОВЛЁН
│   ├── RecipeData.cs             ← ОБНОВЛЁН
│   ├── FireBoostController.cs
│   ├── OrderManager.cs
│   ├── OrderPanel.cs
│   ├── MeterController.cs
│   ├── IngredientSlot.cs
│   ├── IngredientData.cs
│   └── SafeAreaAdapter.cs
```


## 🔧 Настройка новых систем

### 1. Создай CurrencyManager

1. Создай пустой GameObject в сцене: **CurrencyManager**
2. Добавь компонент `CurrencyManager.cs`
3. Этот объект автоматически станет синглтоном и сохранится между сценами

### 2. Создай JackpotController

1. В CookingPanel создай дочерний пустой объект: **JackpotController**
2. Добавь компонент `JackpotController.cs`

**JackpotController UI элементы:**

**JackpotPanel** (Panel) - popup выбора эффекта:
- Position: центр экрана
- Size: 600x400
- Active: false (по умолчанию скрыт)
- Содержит 4 кнопки эффектов

**JackpotTitle** (TMP_Text):
- Text: "🎰 JACKPOT!"
- Font Size: 48
- Color: Gold (#FFD700)

**Effect Buttons** (4 штуки):
- MeterBoostButton: "+10 ALL"
- WildMultiplierButton: "x2 NEXT"
- ZoneShieldButton: "SHIELD"
- TripleApplyButton: "TRIPLE"

**FlashOverlay** (Image):
- Stretch на весь экран
- Color: Gold с alpha 0
- Raycast Target: false

### 3. Создай ShieldController

1. В CookingPanel создай дочерний пустой объект: **ShieldController**
2. Добавь компонент `ShieldController.cs`

**ShieldController UI элементы:**

**ShieldSelectionPanel** (Panel):
- Position: центр экрана
- Size: 500x200
- Active: false
- Содержит 3 кнопки выбора метра

**Shield Selection Buttons:**
- TasteShieldButton: "Protect Taste"
- StabilityShieldButton: "Protect Stability"
- MagicShieldButton: "Protect Magic"

**Shield Icons** (рядом с каждым метром):
- TasteShieldIcon: 🛡️ иконка
- StabilityShieldIcon: 🛡️ иконка
- MagicShieldIcon: 🛡️ иконка
- Все Active: false по умолчанию

### 4. Создай RewardCalculator

1. В GameManager создай дочерний пустой объект: **RewardCalculator**
2. Добавь компонент `RewardCalculator.cs`

### 5. Обнови CookingPanel UI

**Serve Now Button:**
- Position: рядом с Fire Boost (справа)
- Size: 180x80
- Text: "SERVE NOW"
- Color: меняется динамически

**Potential Reward Text:**
- Position: над кнопкой Serve Now
- Text: "Potential: 0 💰"

### 6. Обнови MenuPanel UI

**Currency Display:**
- CoinsText: "💰 0"
- GemsText: "💎 0"
- Position: верхний правый угол

### 7. Обнови ResultPanel UI

**Новые элементы:**

**GradeText** (TMP_Text):
- Text: "PERFECT" / "GOOD" / "OKAY" / "FAILED"
- Font Size: 36

**Meter Status Texts:**
- TasteStatusText: "✓ Taste" или "✗ Taste"
- StabilityStatusText: "✓ Stability" или "✗ Stability"
- MagicStatusText: "✓ Magic" или "✗ Magic"

**Reward Breakdown:**
- BaseRewardText: "Base: 100"
- MeterBonusText: "Meters (2/3): 60%"
- TurnsBonusText: "Turns bonus (2 left): +10"
- FinalRewardText: "85 💰" (анимированный счётчик)

### 8. Обнови RecipeData ScriptableObjects

В каждом рецепте добавь:
- **Base Reward**: 50-500 в зависимости от сложности

Рекомендуемые значения:
- Easy: 50
- Medium: 100
- Hard: 200
- Elite: 500


## 🔗 Подключение ссылок в Inspector

### GameManager:
- Jackpot Controller: → JackpotController
- Shield Controller: → ShieldController
- Reward Calculator: → RewardCalculator
- Currency Manager: → CurrencyManager

### CookingManager:
- Jackpot Controller: → JackpotController
- Shield Controller: → ShieldController
- Reward Calculator: → RewardCalculator

### UIManager:
- Jackpot Controller: → JackpotController
- Shield Controller: → ShieldController

### JackpotController:
- Jackpot Panel: → JackpotPanel
- Jackpot Title Text: → JackpotTitle
- Effect Buttons: Size = 4 (все 4 кнопки)
- Effect Button Texts: Size = 4
- Flash Overlay: → FlashOverlay
- Min Rolls For Jackpot: 5
- Max Rolls For Jackpot: 10

### ShieldController:
- Shield Selection Panel: → ShieldSelectionPanel
- Taste Shield Button: → TasteShieldButton
- Stability Shield Button: → StabilityShieldButton
- Magic Shield Button: → MagicShieldButton
- Taste Shield Icon: → TasteShieldIcon
- Stability Shield Icon: → StabilityShieldIcon
- Magic Shield Icon: → MagicShieldIcon

### CookingPanel:
- Serve Now Button: → ServeNowButton
- Serve Now Button Text: → ServeNowButtonText
- Serve Now Button Image: → ServeNowButtonImage
- Potential Reward Text: → PotentialRewardText

### MenuPanel:
- Coins Text: → CoinsText
- Gems Text: → GemsText

### ResultPanel:
- Grade Text: → GradeText
- Taste Status Text: → TasteStatusText
- Stability Status Text: → StabilityStatusText
- Magic Status Text: → MagicStatusText
- Base Reward Text: → BaseRewardText
- Meter Bonus Text: → MeterBonusText
- Turns Bonus Text: → TurnsBonusText
- Final Reward Text: → FinalRewardText


## 🧪 Как тестировать

### Jackpot System:

1. **Гарантированный джекпот:**
   - Начни готовку
   - Сделай 5-10 выборов ингредиентов
   - ✅ Должен появиться Jackpot popup

2. **Meter Boost:**
   - Выбери "Meter Boost" в jackpot
   - ✅ Все метры +10

3. **Wild Multiplier:**
   - Выбери "Wild Multiplier"
   - Выбери ингредиент
   - ✅ Эффект удвоен

4. **Zone Shield:**
   - Выбери "Zone Shield"
   - ✅ Появится выбор метра
   - Выбери метр
   - ✅ Иконка щита появится
   - Превысь максимум защищённого метра
   - ✅ Щит поглотит overflow

5. **Triple Apply:**
   - Выбери "Triple Apply"
   - ✅ Все 3 ингредиента применятся

### Early Completion:

1. **Кнопка недоступна в начале:**
   - ✅ Serve Now серая, неактивная

2. **После 1 ингредиента:**
   - Выбери ингредиент
   - ✅ Serve Now становится активной

3. **Цвет кнопки:**
   - ✅ Зелёная если 3 метра в зоне
   - ✅ Жёлтая если 2 метра
   - ✅ Оранжевая если 1 метр
   - ✅ Серая если 0 метров

4. **Досрочное завершение:**
   - Нажми Serve Now
   - ✅ Переход к Result Panel
   - ✅ Показана разбивка награды

### Currency System:

1. **Отображение:**
   - ✅ Coins показаны в меню
   - ✅ Gems показаны в меню

2. **Получение награды:**
   - Заверши заказ
   - ✅ Coins увеличились
   - ✅ Анимация счётчика в Result Panel

3. **Сохранение:**
   - Закрой игру
   - Открой снова
   - ✅ Coins сохранились


## 📋 Чек-лист

- [ ] Все скрипты скопированы в Assets/Scripts/
- [ ] CurrencyManager создан и настроен
- [ ] JackpotController создан и настроен
- [ ] ShieldController создан и настроен
- [ ] RewardCalculator создан и настроен
- [ ] Все UI элементы созданы
- [ ] Все ссылки подключены в Inspector
- [ ] RecipeData обновлены с baseReward
- [ ] Тест: Jackpot появляется каждые 5-10 роллов
- [ ] Тест: Serve Now работает
- [ ] Тест: Награды начисляются и сохраняются


## 🐛 Возможные проблемы

**Jackpot не появляется:**
→ Проверь что JackpotController подключен к CookingManager
→ Проверь minRollsForJackpot и maxRollsForJackpot

**Shield не блокирует overflow:**
→ Проверь что ShieldController подключен к CookingManager
→ Убедись что выбрал правильный метр для защиты

**Serve Now не работает:**
→ Кнопка активна только после 1+ ингредиента
→ Проверь что CookingPanel.cookingManager назначен

**Coins не сохраняются:**
→ Проверь что CurrencyManager создан в сцене
→ CurrencyManager использует PlayerPrefs

**Награда всегда 0:**
→ Проверь что baseReward > 0 в RecipeData
→ Проверь что RewardCalculator подключен к CookingManager


## 📊 Примеры расчёта награды

### Пример 1: Perfect Early Serve
- Рецепт: Magic Soup (baseReward = 100)
- Все 3 метра в зоне = 100%
- 3 хода осталось × 5% = +15%
- **Итого: 115 coins**

### Пример 2: Good Completion
- Рецепт: Fire Steak (baseReward = 200)
- 2 из 3 метров = 60%
- 0 ходов осталось = +0%
- **Итого: 120 coins**

### Пример 3: Panic Serve
- Рецепт: Dragon Pie (baseReward = 500)
- 1 метр в зоне = 30%
- 4 хода осталось × 5% = +20%
- **Итого: 250 coins**


## 📊 Статус разработки

### ✅ Итерация 1: Система заказов
### ✅ Итерация 2: Система готовки
### ✅ Итерация 3: Fire Boost
### ✅ Итерация 4: Jackpot + Early Completion + Currency (текущая)

### 🔄 Следующие итерации:
- **Итерация 5**: Улучшенные визуальные эффекты
- **Итерация 6**: Звуки и haptic feedback
- **Итерация 7**: Прогрессия и сохранение
- **Итерация 8**: Магазин и Gems


Готов к **Итерации 5: Visual Effects**? ✨
