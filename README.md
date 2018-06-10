# PayDebt

### Описание
Мобильное приложение под Android для контроля долгов и займов.

### Реализованные функции
*  Запись и удаление долгов
*  Возможность установки даты выплаты
*  Привязка к VK, отправка сообщения о взятии займа пользователю
*  Выбор валюты
*  История займов

### Требования
*   Android 5.0

### Структура решения
##### Слой инфраструктуры
Представлен [Infrastructure](Infrastructure) и [AndroidInfrastructure](PayDebt/AndroidInfrastructure).

[Infrastructure](Infrastructure)
*   [IEnumerableExtension](Infrastructure/IEnumerableExtension.cs), [TypeExtensions](Infrastructure/TypeExtensions.cs) - методы расширения
*   [ValueType](Infrastructure/ValueType.cs) - базовый класс для Value-типов
*   [Entity](Infrastructure/Entity.cs) - базовый класс для сущностей
*   [ScalarType](Infrastructure/ScalarType.cs) - Value-тип, имеющий единицы измерения
*   [StaticStorage](Infrastructure/StaticStorage.cs) - класс, предоставляющий наследникам возможность получить перечислений всех значений статических полей определенного типа
*   [IEntityStorageAccess](Infrastructure/IEntityStorageAccess.cs) - интерфейс хранилища сущностей

[AndroidInfrastructure](PayDebt/AndroidInfrastructure)
*   [TabsFragmentPagerAdapter](PayDebt/AndroidInfrastructure/TabsFragmentPagerAdapter.cs)
*   [ViewExtensions](PayDebt/AndroidInfrastructure/ViewExtensions.cs)
*   [VkRequestListener](PayDebt/AndroidInfrastructure/VkRequestListener.cs)


### Предметная область
Полностью представлена в проекте [ModelDebt](DebtModel):
*   [Contact](ModelDebt/Contact.cs) - контакт
*   [Currency](ModelDebt/Currency.cs) - валюта, Value-тип
*   [Currencies](ModelDebt/Currencies.cs) - хранилище валют, наследуется от `StaticStorage<Currency, Currencies>`
*   [Money](ModelDebt/Money.cs) - модель денег, представляет из себя скалярную величину(размерность - `Currency`, значение - `decimal`)
*   [Debt](ModelDebt/Debt.cs) - запись о долге, сущность
*   [IDebtStorageAccess](ModelDebt/IDebtStarageAccess.cs) - интерфейс хранилища долгов.
*	[Debts](ModelDebt/Debts.cs) - ???

### 


