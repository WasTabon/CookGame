# Итерация 8: Экономика и IAP

## 🎯 Структура валют

```
💎 Gems ←── In-App Purchase (реальные деньги)
   ↓
💰 Coins ←── Награды за заказы
   ↓
🎮 Бустеры + 🔥 Fire Boost
```


## 📊 Цены

### 💎 Gems (IAP)
| Пак | Гемов | Product ID |
|-----|-------|------------|
| Gem Pack | 50 | `com.yourgame.gems50` |

### 💎 → 💰 Обмен
| Гемы | Монеты |
|------|--------|
| 1 💎 | 50 💰 |
| 5 💎 | 250 💰 |
| 10 💎 | 500 💰 |

### 🔥 Fire Boost (за гемы)
| Длительность | Цена |
|--------------|------|
| 2 секунды | 1 💎 |
| 3 секунды | 2 💎 |
| 5 секунд | 3 💎 |

### 🎮 Бустеры (за монеты)
| Бустер | Цена | Эффект |
|--------|------|--------|
| Extra Turn | 200 💰 | +1 ход |
| Shield | 300 💰 | Блокирует overflow |
| Double Coins | 400 💰 | x2 награда |
| Double XP | 400 💰 | x2 опыт |


## 📁 Файлы

```
Assets/
├── Scripts/
│   ├── IAPManager.cs              ← НОВЫЙ (Unity IAP 5.1.1)
│   ├── GemShopPanel.cs            ← НОВЫЙ
│   ├── ShopItem.cs                ← НОВЫЙ (только coinPrice)
│   ├── ShopManager.cs             ← НОВЫЙ
│   ├── ShopPanel.cs               ← НОВЫЙ
│   ├── ShopItemSlot.cs            ← НОВЫЙ
│   ├── BoosterManager.cs          ← НОВЫЙ
│   ├── BoosterSelectionPanel.cs   ← НОВЫЙ
│   ├── FireBoostController.cs     ← ОБНОВЛЁН (за гемы)
│   ├── CookingManager.cs          ← ОБНОВЛЁН
│   └── MenuPanel.cs               ← ОБНОВЛЁН
│
└── Editor/
    ├── GemShopPanelCreator.cs     ← НОВЫЙ
    ├── ShopPanelCreator.cs        ← НОВЫЙ
    └── BoosterSelectionPanelCreator.cs ← НОВЫЙ
```


## ⚙️ Требования

### Unity IAP Package

1. Window → Package Manager
2. Найди "In App Purchasing"
3. Установи версию **5.1.1**
4. Включи в Services: Window → General → Services → In-App Purchasing


## 🔧 Настройка

### 1. Скопируй скрипты

- Scripts → `Assets/Scripts/`
- Editor → `Assets/Editor/`


### 2. Создай IAPManager

1. Создай пустой GameObject: **IAPManager**
2. Добавь компонент `IAPManager.cs`
3. Настрой:

| Поле | Значение |
|------|----------|
| Gem Pack Product Id | `com.yourgame.gems50` |
| Gems Per Purchase | 50 |


### 3. Создай BoosterManager

1. Создай пустой GameObject: **BoosterManager**
2. Добавь компонент `BoosterManager.cs`


### 4. Создай ShopManager

1. Создай пустой GameObject: **ShopManager**
2. Добавь компонент `ShopManager.cs`


### 5. Создай ShopItem ScriptableObjects

Right-click → Create → Probability Kitchen → Shop Item

| Name | Booster Type | Coin Price | Quantity |
|------|--------------|------------|----------|
| Extra Turn x1 | ExtraTurn | 200 | 1 |
| Shield x1 | Shield | 300 | 1 |
| Double Coins x1 | DoubleCoins | 400 | 1 |
| Double XP x1 | DoubleXP | 400 | 1 |

Добавь все в **ShopManager → All Items**


### 6. Создай GemShopPanel UI

1. Меню: **Probability Kitchen → Create GemShopPanel UI**
2. Добавь компонент `GemShopPanel.cs`
3. Присвой ссылки:

| Поле | GameObject |
|------|------------|
| Gems Text | .../GemsText |
| Coins Text | .../CoinsText |
| Buy Gems Button | .../BuyGemsButton |
| Buy Gems Button Text | .../BuyGemsButton/Text |
| Gem Pack Info Text | .../GemPackInfoText |
| Restore Button | .../RestoreButton |
| Exchange 1 Button | .../Exchange1Button |
| Exchange 5 Button | .../Exchange5Button |
| Exchange 10 Button | .../Exchange10Button |
| Exchange 1 Text | .../Exchange1Button/Text |
| Exchange 5 Text | .../Exchange5Button/Text |
| Exchange 10 Text | .../Exchange10Button/Text |
| Close Button | .../CloseButton |
| Status Text | .../StatusText |


### 7. Создай ShopPanel UI

1. Меню: **Probability Kitchen → Create ShopPanel UI (Boosters)**
2. Добавь компонент `ShopPanel.cs`
3. Присвой ссылки:

| Поле | GameObject |
|------|------------|
| Title Text | .../TitleText |
| Coins Text | .../CoinsText |
| Items Container | .../ScrollView/Viewport/ItemsContainer |
| Item Slot Prefab | (создай prefab из ItemSlot) |
| Close Button | .../CloseButton |


### 8. Создай ShopItemSlot Prefab

1. Создай UI элемент или используй созданный
2. Добавь `ShopItemSlot.cs`
3. Присвой ссылки
4. Сохрани как Prefab
5. Присвой в ShopPanel → Item Slot Prefab


### 9. Создай BoosterSelectionPanel UI

1. Меню: **Probability Kitchen → Create BoosterSelectionPanel UI**
2. Добавь компонент `BoosterSelectionPanel.cs`
3. Присвой все ссылки


### 10. Обнови MenuPanel

Добавь кнопки и присвой:

| Поле | Описание |
|------|----------|
| Shop Button | Кнопка "🛒" для бустеров |
| Gem Shop Button | Кнопка "💎" для гемов |
| Shop Panel | ShopPanel |
| Gem Shop Panel | GemShopPanel |


### 11. Настрой FireBoostController

Проверь цены:

| Поле | Значение |
|------|----------|
| Gem Prices | [1, 2, 3] |
| Duration Options | [2, 3, 5] |


## 🍎 Настройка IAP для iOS

### App Store Connect:

1. Зайди в App Store Connect
2. Создай In-App Purchase (Consumable)
3. Product ID: `com.yourgame.gems50`
4. Установи цену ($0.99 или другую)


### В Unity:

1. Window → Services → In-App Purchasing → Configure
2. Добавь Product ID
3. Build & Run на устройстве для теста


## 🎮 Игровой флоу

```
MenuPanel
   ├── 💎 Button → GemShopPanel
   │      ├── Buy 50 💎 (IAP)
   │      ├── Exchange 💎 → 💰
   │      └── Restore Purchases
   │
   ├── 🛒 Button → ShopPanel (Boosters)
   │      └── Buy Extra Turn, Shield, etc. (💰)
   │
   └── Get Order → OrderPanel → Accept
          ↓
       BoosterSelectionPanel
          ↓ (Select & Start)
       CookingPanel
          └── 🔥 Fire Boost (💎)
```


## 🧪 Тестирование

### IAP (без реальной покупки):

```csharp
// В IAPManager.cs добавь для тестов:
public void TestAddGems()
{
    if (CurrencyManager.Instance != null)
    {
        CurrencyManager.Instance.AddGems(50);
    }
}
```

### Тесты:

1. **GemShopPanel:**
   - Открой 💎 Shop
   - ✅ Показаны гемы и монеты
   - Нажми обмен 1💎 → 50💰
   - ✅ Гемы списались, монеты добавились

2. **ShopPanel:**
   - Открой 🛒 Shop
   - ✅ Показаны бустеры с ценами в 💰
   - Купи Extra Turn
   - ✅ Монеты списались

3. **BoosterSelectionPanel:**
   - Начни заказ
   - ✅ Показаны доступные бустеры
   - Выбери Extra Turn
   - ✅ В игре +1 ход

4. **Fire Boost:**
   - Во время готовки нажми 🔥
   - ✅ Показаны варианты с ценами в 💎
   - Выбери 2s за 1💎
   - ✅ Гем списался, буст активировался


## 📋 Чек-лист

- [ ] Unity IAP 5.1.1 установлен
- [ ] IAPManager создан и настроен
- [ ] BoosterManager создан
- [ ] ShopManager создан с товарами
- [ ] ShopItem ScriptableObjects созданы
- [ ] GemShopPanel UI создан
- [ ] ShopPanel UI создан
- [ ] ShopItemSlot Prefab создан
- [ ] BoosterSelectionPanel UI создан
- [ ] MenuPanel обновлён
- [ ] FireBoostController настроен с ценами в гемах
- [ ] Тест: обмен гемов на монеты
- [ ] Тест: покупка бустеров за монеты
- [ ] Тест: Fire Boost за гемы


## 🐛 Возможные проблемы

**IAP не инициализируется:**
- Проверь Unity Services подключены
- Проверь Product ID совпадает с App Store Connect

**Кнопка покупки неактивна:**
- IAP работает только на реальном устройстве
- В Editor используй тестовый метод AddGems()

**Fire Boost не списывает гемы:**
- Проверь что CurrencyManager создан
- Проверь gemPrices[] в FireBoostController

**Бустеры не применяются:**
- Проверь что BoosterManager создан
- CookingManager должен вызывать ApplyStartingBoosters()


## 📊 Статус разработки

### ✅ Завершено:
- Итерация 1-7

### ✅ Итерация 8 (текущая):
- IAP для iOS (Unity IAP 5.1.1)
- GemShopPanel (покупка + обмен)
- ShopPanel (бустеры за монеты)
- Fire Boost за гемы
- Restore Purchases

### 🔄 Следующие:
- Итерация 9: Daily Rewards
- Итерация 10: Achievements
