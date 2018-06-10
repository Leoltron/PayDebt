# PayDebt

### ��������
��������� ���������� ��� Android ��� �������� ������ � ������.

### ������������� �������
*  ������ � �������� ������
*  ����������� ��������� ���� �������
*  �������� � VK, �������� ��������� � ������ ����� ������������
*  ����� ������
*  ������� ������

### ����������
*   Android 5.0

### ��������� �������
##### ���� ��������������
����������� [Infrastructure](Infrastructure) � [AndroidInfrastructure](PayDebt/AndroidInfrastructure).

[Infrastructure](Infrastructure)
*   [IEnumerableExtension](Infrastructure/IEnumerableExtension.cs), [TypeExtensions](Infrastructure/TypeExtensions.cs) - ������ ����������
*   [ValueType](Infrastructure/ValueType.cs) - ������� ����� ��� Value-�����
*   [Entity](Infrastructure/Entity.cs) - ������� ����� ��� ���������
*   [ScalarType](Infrastructure/ScalarType.cs) - Value-���, ������� ������� ���������
*   [StaticStorage](Infrastructure/StaticStorage.cs) - �����, ��������������� ����������� ����������� �������� ������������ ���� �������� ����������� ����� ������������� ����
*   [IEntityStorageAccess](Infrastructure/IEntityStorageAccess.cs) - ��������� ��������� ���������

[AndroidInfrastructure](PayDebt/AndroidInfrastructure)
*   [TabsFragmentPagerAdapter](PayDebt/AndroidInfrastructure/TabsFragmentPagerAdapter.cs)
*   [ViewExtensions](PayDebt/AndroidInfrastructure/ViewExtensions.cs)
*   [VkRequestListener](PayDebt/AndroidInfrastructure/VkRequestListener.cs)


### ���������� �������
��������� ������������ � ������� [ModelDebt](DebtModel):
*   [Contact](ModelDebt/Contact.cs) - �������
*   [Currency](ModelDebt/Currency.cs) - ������, Value-���
*   [Currencies](ModelDebt/Currencies.cs) - ��������� �����, ����������� �� `StaticStorage<Currency, Currencies>`
*   [Money](ModelDebt/Money.cs) - ������ �����, ������������ �� ���� ��������� ��������(����������� - `Currency`, �������� - `decimal`)
*   [Debt](ModelDebt/Debt.cs) - ������ � �����, ��������
*   [IDebtStorageAccess](ModelDebt/IDebtStarageAccess.cs) - ��������� ��������� ������.
*	[Debts](ModelDebt/Debts.cs) - ???

### 


