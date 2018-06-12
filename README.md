# PayDebt

### Описание
Мобильное приложение под Android для контроля долгов и займов.

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
*   [StaticStorage](Infrastructure/StaticStorage.cs) - класс, предоставляющий наследникам возможность получить перечисление всех значений статических полей определенного типа
*   [IEntityStorageAccess](Infrastructure/IEntityStorageAccess.cs) - интерфейс хранилища сущностей

[AndroidInfrastructure](PayDebt/AndroidInfrastructure)
*   [TabsFragmentPagerAdapter](PayDebt/AndroidInfrastructure/TabsFragmentPagerAdapter.cs)
*   [ViewExtensions](PayDebt/AndroidInfrastructure/ViewExtensions.cs)
*   [VkRequestListener](PayDebt/AndroidInfrastructure/VkRequestListener.cs)


##### Предметная область
Полностью представлена в проекте [ModelDebt](DebtModel):
*   [Contact](ModelDebt/Contact.cs) - контакт
*   [Currency](ModelDebt/Currency.cs) - валюта, Value-тип
*   [Currencies](ModelDebt/Currencies.cs) - хранилище валют, наследуется от `StaticStorage<Currency, Currencies>`
*   [Money](ModelDebt/Money.cs) - модель денег, представляет из себя скалярную величину(размерность - `Currency`, значение - `decimal`)
*   [Debt](ModelDebt/Debt.cs) - запись о долге, сущность
*   [Debts](ModelDebt/Debts.cs) - информация о займах


##### Слой приложения
В этом слое находятся все файлы, находящиеся непосредственно в [Application](PayDebt/Application):
*   Классы из [Activities](PayDebt/Application/Activities), каждый из которых отвечает за поведение приложения на определенном экране
*	Различные классы, задающие взаимодествие с виджетами, а также связывающие данные с их представлением([CurrencySpinner](PayDebt/Application/CurrencySpinner.cs), [DebtListFragment](PayDebt/Application/DebtListFragment), [DebtInfoAdapter](PayDebt/Application/DebtInfoAdapter.cs))
*   [SharedPrefDebt](PayDebt/Application/SharedPrefDebt.cs) - реализация `IEntityStorageAccess<int, Debt>`
*   [SharedPrefDebtExtensions](PayDebt/Application/SharedPrefDebtExtensions.cs) - методы расширения для `SharedPrefDebt`


##### GUI
Реализован с помощью AXML, находится в [Resources](PayDebt/Resources). Макеты можно найти в [layout](PayDebt/Resources/layout).





### План презентации

##### Суть проекта
Удобный способ записи долгов.

##### Реализованные функции
*  Запись и удаление долгов
*  Возможность установки даты выплаты
*  Привязка к VK, отправка сообщения о взятии займа пользователю
*  Выбор валюты
*  История займов

##### Кто чем занимался
__TODO__

##### Точки расширения 
*	Легко добавлять новые валюты
*	Возможность выбора друзей из других источников

##### DI-контейнеры
А их вроде и нет.

##### DDD
Явное разделение на области. Можно прикрепить граф зависимостей.

##### Тесты
Тестирование уровня инфраструктуры и предметной области.

