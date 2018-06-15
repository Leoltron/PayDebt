# PayDebt

### Описание
Мобильное приложение под Android для контроля долгов и займов.

### Требования
*   Android 5.0

### Структура решения
##### Слой инфраструктуры
Представлен [Infrastructure](Infrastructure) и [AndroidInfrastructure](PayDebt/AndroidInfrastructure).

###### [Infrastructure](Infrastructure)
*   [EnumerableExtensions](Infrastructure/EnumerableExtensions.cs), [TypeExtensions](Infrastructure/TypeExtensions.cs) - методы расширения
*   [ValueType](Infrastructure/ValueType.cs) - базовый класс для Value-типов
*   [Entity](Infrastructure/Entity.cs) - базовый класс для сущностей
*   [ScalarType](Infrastructure/ScalarType.cs) - Value-тип, имеющий единицы измерения
*   [StaticStorage](Infrastructure/StaticStorage.cs) - класс, предоставляющий наследникам возможность получить перечисление всех значений статических полей определенного типа
*   [IEntityStorageAccess](Infrastructure/IEntityStorageAccess.cs) - интерфейс хранилища сущностей
*	[BytesExtensions](Infrastructure/BytesExtensions.cs) - расширения для сериализации

###### [AndroidInfrastructure](PayDebt/AndroidInfrastructure)
*   [TabsFragmentPagerAdapter](PayDebt/AndroidInfrastructure/TabsFragmentPagerAdapter.cs)
*   [ViewExtensions](PayDebt/AndroidInfrastructure/ViewExtensions.cs)
*   [VkRequestListener](PayDebt/AndroidInfrastructure/VkRequestListener.cs)
*	[VkFriends](PayDebt/AndroidInfrastructure/VkFriends.cs) - статический класс, позволяющий получить список друзей ВК


##### Предметная область
Представлена в проекте [ModelDebt](ModelDebt) и в папке [PayDebt/Model](PayDebt/Model):

###### [ModelDebt](ModelDebt):
*   [Contact](ModelDebt/Contact.cs), [VKContact](ModelDebt/VKContact.cs), [PhoneContact](ModelDebt/PhoneContact.cs) - базовый класс для контактов, а также его реализации
*   [Currency](ModelDebt/Currency.cs) - валюта, Value-тип
*   [Currencies](ModelDebt/Currencies.cs) - хранилище валют, наследуется от `StaticStorage<Currency, Currencies>`
*   [Money](ModelDebt/Money.cs) - модель денег, представляет из себя скалярную величину(размерность - `Currency`, значение - `decimal`)
*   [Debt](ModelDebt/Debt.cs) - запись о долге, сущность
*   [Debts](ModelDebt/Debts.cs) - информация о займах
*	[IContactProvider](ModelDebt/IContactProvider.cs) - интерфейс поставщика контактов
*	[IBaseContactPicker](ModelDebt/IBaseContactPicker.cs) - базовый интерфейс для работы с контактами
*	[BaseContactPicker](ModelDebt/BaseContactPicker.cs) - реализация `IBaseContactPicker`

###### [PayDebt/Model](PayDebt/Model):
*	[IAuth](PayDebt/Model/IAuth.cs) - интерфейс авторизации
*	[IContactPicker](PayDebt/Model/IContactPicker.cs) - интерфейс работы с контактами, наследуется от `IAuth` и `IBaseContactPicker`
*	[ContactPicker](PayDebt/Model/ContactPicker.cs) - реализация интерфейса `IContactPicker`
*	[ContactPickers](PayDebt/Model/ContactPickers.cs) - хранилище сборщиков контактов, наследуется от `StaticStorage<IContactPicker<Contact>, ContactPickers>`
*	[PhoneContactProvider](PayDebt/Model/PhoneContactProvider.cs) - реализация `IContactProvider<PhoneContact>`
*	[PhoneContactPicker](PayDebt/Model/PhoneContactPicker.cs) - наследник `ContactPicker<PhoneContact>`
*	[VKContactProvider](PayDebt/Model/VKContactProvider.cs) - реализация `IContactProvider<VkContact>`
*	[VkContactPicker](PayDebt/Model/VkContactPicker.cs) - наследник `ContactPicker<VkContact>`

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
*	Запись и удаление долгов
*	Возможность установки даты выплаты
*	Возможность выбора контакта из VK, отправка сообщения о взятии займа пользователю
*	Возможность выбора контакта из контактов телефона
*  	Выбор валюты
*  	История займов

##### Кто чем занимался
[Айдар](https://github.com/lowgear) - тестирование платформонезависимой части приложнения
[Александр](https://github.com/ashibaev) - разбиение на слои, выделение абстракций
[Леонид](https://github.com/Leoltron) - всё остальное

##### Точки расширения 
*	Легко добавлять новые валюты
*	Возможность выбора друзей из других источников
	Для этого необходимо реализовать:
	*	класс `MyContact : Contact`
	*	реализацию `IContactProvider<MyContact>`
	*	класс `MyContactPicker : ContactPicker<MyContact>`
	*	класс `MyContactPickerActivity : FriendPickerActivity<BaseContactPicker<MyContact>, MyContact>`
*	Хранилища данных

##### DI-контейнеры
Их до сих пор нет. Тем не менее, все зависимости могут быть внедрены в [одном месте](PayDebt/Application/CustomApplication.cs). Используется `StaticStorage` для внедрения зависимостей.

##### DDD
Явное разделение на области. 

![Граф зависимостей](https://cdn1.savepice.ru/uploads/2018/6/15/ccea3af1368864f45d7f64fc75ac0264-full.png)

##### Тесты
Тестирование уровня инфраструктуры и предметной области.
*	[InfrastructureTests](InfrastructureTests)
*	[DebtModelTests](DebtModelTests)
