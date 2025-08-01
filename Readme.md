# HydraScript

![logo](hydrascript-logo.jpg)

## "Расширенное подмножество ЯП JavaScript"

### Скачать

Файл интерпретатора собирается в рамках релиза на три платформы:
- Windows (x64)
- MacOS (arm64 Apple Silicon)
- Linux (x64)

Скачать нужную версию можно со страницы соответствующего релиза

[Последний релиз доступен по этой ссылке](https://github.com/Stepami/hydrascript/releases/latest)

### Вводная информация

За основу был взят стандарт [ECMA-262](https://www.ecma-international.org/publications-and-standards/standards/ecma-262/)

[Лексическая структура](src/Domain/HydraScript.Domain.Constants/TokenTypes.cs)

[Грамматика](src/Domain/HydraScript.Domain.FrontEnd/Parser/grammar.txt)

[Рабочие примеры](tests/HydraScript.IntegrationTests/Samples)

### Цели проекта
1. Частично реализовать JavaScript с объектами и статической структурной типизацией, избавившись от таких понятий, как `constructor`, `class`, `interface`
2. Публично реверс-инжинирить современный статический анализ (вывод типов, форвард рефы, ошибки выполнения на стадии компиляции)
3. Упростить понимание области конструирования компиляторов за счёт исходного кода проекта - собрать понятные реализации алгоритмов и типовых задач в репозитории (Lexer, Parser, CFG, SSA, DCE, etc.)

### Конструкции языка

#### Типизация
В языке структурная статическая сильная типизация.

Есть 5 примитивных типов:
1. number
2. boolean
3. string
4. null
5. void

Остальные типы делятся на группы:
- NullableType (тип, который допускает значение ```null```)
- ObjectType (тип объекта, является NullableType)
- ArrayType (списковый тип)

##### Значения по умолчанию

| Тип          | Значение |
|--------------|----------|
| number       | 0        |
| boolean      | false    |
| string       | ""       |
| NullableType | null     |
| ArrayType    | []       |
##### type alias
Можно создать свой type alias по типу того, как это сделано в С++

```
type int = number
type maybeInt = int?
type ints = int[]
type point = {
    x: int;
    y: int;
}
type composite = {
    p: point;
    arr: ints;
}
```
#### Объявление переменных
```
let i = 1 // интерпретатор выведет тип из выражения
let j: number // запишет значение по умолчанию в переменную
let k: number = 1 // полностью явное объявление
```

#### Объекты
```
let v2d = {
    x: 3;
    y: 4;
}
```
#### Списки
```
let array = [1, 2, 3]
let size = ~array // длина списка
array::1 // удаление элемента по индексу
array = array ++ [5, 7] // конкатенация списков
```
#### Операторы
| Оператор         | Вид      | Типы операндов         | Тип операции   |
|------------------|----------|------------------------|----------------|
| +                | бинарный | оба number, оба string | number, string |
| *, -, /, %       | бинарный | number                 | number         |
| &#124;&#124;, && | бинарный | boolean                | boolean        |
| !=, ==           | бинарный | равный с двух сторон   | boolean        |
| <=, >=, >, <     | бинарный | number                 | boolean        |
| !                | унарный  | boolean                | boolean        |
| -                | унарный  | number                 | number         |
| ++               | бинарный | []                     | []             |
| ::               | бинарный | [] и number            | void           |
| ~                | унарный  | []                     | number         |

#### Ветвление
```
if (1 == 1) {
    // ...
} else if (2 == 2) {
    // ...
}
else {
    // ...
}
// в общем как в Си подобных языках
// главное, чтобы выражение условия
// возвращало boolean
```
Также есть тернарный оператор
```
let x = 1 > 0 ? 0 <= 1 ? 1 : 0 : -2 < 0 ? -1 : 0
```
#### Цикл
```
while (cond) {
    // ...
    continue
    // ...
    break
}
```
#### Функции
```
// объявление
function add(a: number, b: number): number {
    return a + b
}
// вызов
let c = add(1, 2)
```
#### Методы
```
// сделаны подобно Go - привязка по имени типа

// шаг 1. Объявить type alias
type Point2 = {
    x: number;
    y: number;
}

// шаг 2. Объявить переменную этого типа
let v2d: Point2 = {
    x: 3;
    y: 4;
}

// шаг 3. Указать первым параметром функции - объект типа
function lengthSquared(obj: Point2) {
    let x = obj.x
    let y = obj.y
    return x * x + y * y
}
```
#### Операции доступа
```
// объекты
let x = v2d.x
let s = v2d.lengthSquared()
// массивы
let l = array[2]
```
#### Приведение типов
```
let s = v2d as string
```
#### Печать на экран
```
let obj = {}
>>>obj
>>>"Hello, World!"
```

### Требования

До версии **2.3.0** для запуска интерпретатора требовалась [установка .NET Runtime](https://dotnet.microsoft.com/ru-ru/download/dotnet)

Таблица соответствий hydrascript и dotnet:

| hydrascript                                                       | dotnet |
|-------------------------------------------------------------------|--------|
| <ul><li>1.0.0</li><li>1.0.2</li></ul>                             | .NET 5 |
| <ul><li>1.1.2</li><li>1.1.3</li><li>1.1.4</li><li>1.1.5</li></ul> | .NET 6 |
| <ul><li>1.2.5</li><li>1.2.6</li><ul>                              | .NET 7 |
| 2.0.0                                                             | .NET 8 |
| <ul><li>2.1.0</li><li>2.1.1</li><li>2.2.0</li></ul>               | .NET 9 |

### Сборка
Необходим .NET SDK 9.0.202 (поддержка SLNX)

После клонирования репозитория идём в папку проекта `HydraScript`.

Там выполняем команду:
```dotnet publish ./src/HydraScript/HydraScript.csproj -r <RUNTIME_IDENTIFIER> -o <OUTPUT_DIRECTORY>```

Список идентификаторов рантайма лежит [тут](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#windows-rids)

### Запуск

Простой:
```
HydraScript file.js
```

С выводом дебаг инфы (токены, ast, инструкции):
```
HydraScript file.js --dump
```

### Источники:

1. Курсы "Конструирование Компиляторов" и "Генерация Оптимального Кода" кафедры ИУ-9 МГТУ им. Н.Э. Баумана [@bmstu-iu9](https://www.github.com/bmstu-iu9)
2. [ECMA-262](https://www.ecma-international.org/publications-and-standards/standards/ecma-262/)
3. [DragonBook](https://suif.stanford.edu/dragonbook/)
4. [Stanford CS143 Lectures](https://web.stanford.edu/class/archive/cs/cs143/cs143.1128/)
5. [Simple Virtual Machine](https://github.com/parrt/simple-virtual-machine)
6. Ахо А., Ульман Дж. Теория синтаксического анализа, перевода и компиляции
7. Свердлов С.З. Языки программирования и методы трансляции
