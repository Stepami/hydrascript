# Курсовая работа по дисциплине "Конструирование компиляторов"

## "Разработка интерпретатора подмножества языка JavaScript"

За основу был взят стандарт [ECMA-262](https://www.ecma-international.org/publications-and-standards/standards/ecma-262/)

#### Реализованная грамматика

```
Script -> StatementList
StatementList -> StatementListItem*
StatementListItem -> Statement
                     Declaration
                     
Statement -> BlockStatement
             ExpressionStatement
             IfStatement
             WhileStatement
             ContinueStatement
             BreakStatement
             ReturnStatement
             
Declaration -> LexicalDeclaration
               FunctionDeclaration
               
BlockStatement -> '{' StatementList '}'

ExpressionStatement -> Expression
Expression -> AssignExpression
AssignExpression -> "Ident" '=' (AssignExpression | ConditionalExpression) | OrExpression
ConditionalExpression -> OrExpression '?' ConditionalExpression ':' ConditionalExpression
OrExpression -> AndExpression ('||' AndExpression)*
AndExpression -> EqExpression ('&&' EqExpression)*
EqExpression -> RelExpression (('=='|'!=') RelExpression)*
RelExpression -> AddExpression (('<'|'>'|'<='|'>=') AddExpression)*
AddExpression -> MulExpression (('+'|'-') MulExpression)*
MulExpression -> UnaryExpression (('*'|'/'|'%') UnaryExpression)*
UnaryExpression -> PrimaryExpression | ('-'|'!') UnaryExpression
PrimaryExpression -> "Ident" | Literal | '(' Expression ')' | CallExpression
CallExpression -> "Ident" '(' (Expression (',' Expression)*)? ')'
Literal -> "NullLiteral"
           "IntegerLiteral"
           "FloatLiteral"
           "StringLiteral"
           "BooleanLiteral"
                
IfStatement -> 'if' '(' Expression ')' Statement ('else' Statement)?

WhileStatement -> 'while' '(' Expression ')' Statement

ContinueStatement -> 'continue'

BreakStatement -> 'break'

ReturnStatement -> 'return' Expression?
                       
LexicalDeclaration -> LetOrConst "Ident" Initialization (',' "Ident" Initialization)*
Initialization -> Type
                  Initializer
LetOrConst -> 'let'
              'const'
Initializer -> '=' Expression

FunctionDeclaration -> 'function' "Ident" '(' FunctionParameters? ')' Type? BlockStatement
FunctionParameters -> ParameterDeclaration (',' ParameterDeclaration)*
ParameterDeclaration -> "Ident" ':' TypeIdentifier
Type -> ':' TypeIdentifier
TypeIdentifier -> PrimitiveType
PrimitiveType -> 'number'
                 'string'
                 'boolean'
```

#### Примеры дампа интерпретатора

##### 1
Исходник
```javascript
function fact(n: number): number {
    if (n < 2) {
        return n
    }
    return n * fact(n - 1)
}

function fib(n: number): number {
    if (n < 2) {
        return n
    }
    return fib(n - 1) + fib(n - 2)
}

let n: number

n = 6
print("fact(" + toString(n) + ") = " + toString(fact(6)) + "\n")

n = 9
print("fib(" + toString(n) + ") = " + toString(fib(9)) + "\n")
```
IR = Three-Address-Code
```
0: Goto 9
1: _t1 = n < 2
2: IfNot _t1 Goto 4
3: Return n
4: _t4 = n - 1
5: PushParameter n = _t4
6: _t6 = Call (1, fact), 1
7: _t7 = n * _t6
8: Return _t7
9: Goto 21
10: _t10 = n < 2
11: IfNot _t10 Goto 13
12: Return n
13: _t13 = n - 1
14: PushParameter n = _t13
15: _t15 = Call (10, fib), 1
16: _t16 = n - 2
17: PushParameter n = _t16
18: _t18 = Call (10, fib), 1
19: _t19 = _t15 + _t18
20: Return _t19
21: n = 0
22: n = 6
23: _t23 = n as string
24: _t24 = \"fact(\" + _t23
25: _t25 = _t24 + \") = \"
26: PushParameter n = 6
27: _t27 = Call (1, fact), 1
28: _t28 = _t27 as string
29: _t29 = _t25 + _t28
30: _t30 = _t29 + \"\\n\"
31: Print _t30
32: n = 9
33: _t33 = n as string
34: _t34 = \"fib(\" + _t33
35: _t35 = _t34 + \") = \"
36: PushParameter n = 9
37: _t37 = Call (10, fib), 1
38: _t38 = _t37 as string
39: _t39 = _t35 + _t38
40: _t40 = _t39 + \"\\n\"
41: Print _t40
42: End
```
AST

![](https://habrastorage.org/webt/jv/in/ab/jvinabkiybf76gtj3bshqujbzdu.png)

CFG

![](https://habrastorage.org/webt/ys/vv/ab/ysvvab9udcwxfcyymhj4g59bnko.png)

##### 2
Исходник
```javascript
function abs(x: number): number {
    if (x < 0)
        return -x
    return x
}

let x = -10
print(toString(abs(x)))
```
IR = Three-Address-Code
```
0: Goto 6
1: _t1 = x < 0
2: IfNot _t1 Goto 5
3: _t3 =  -x
4: Return _t3
5: Return x
6: x =  -10
7: PushParameter x = x
8: _t8 = Call (1, abs), 1
9: _t9 = _t8 as string
10: Print _t9
11: End
```
AST

![](https://habrastorage.org/webt/ju/zd/1h/juzd1hdtocyaa2_fgvl1kkkn9ko.png)

CFG

![](https://habrastorage.org/webt/8y/dx/ps/8ydxpstbdfunz-qkxgtom-qhdp0.png)

### Тесты
1
```javascript
if (1 < 9) {
    
} else {
    
}
```
```
0: End
```
2
```javascript
function f() {
    
}

f()
```
```
0: End
```
3
```javascript
let x = 0
while (x < 0) {
    
}
print(toString(x))
```
```
0: x = 0
1: _t1 = x as string
2: Print _t1
3: End
```
4
```javascript
function bar(){
    print("bar")
    return
    let a = 0
}

bar()
```
```
0: Goto 3
1: Print \"bar\"
2: Return
3: Call (1, bar), 0
4: End
```
5
```javascript
let a = 1 + 3 * 2 - 5 / 5
// a == 6
print(toString(a))
```
```
0: a0 = 3 * 2
1: a1 = 1 + a0
2: a2 = 5 / 5
3: a = a1 - a2
4: _t4 = a as string
5: Print _t4
6: End
```
6
```javascript
// prime factors
let n = 150

let factor = 2
while (factor * factor <= n) {
    while (n % factor == 0) {
        n = n / factor
        print(toString(factor) + "\n")
    }

    factor = factor + 1
}

if (n != 1) {
    print(toString(factor) + "\n")
}
```
```
0: n = 150
1: factor = 2
2: _t2 = factor * factor
3: _t3 = _t2 <= n
4: IfNot _t3 Goto 15
5: _t5 = n % factor
6: _t6 = _t5 == 0
7: IfNot _t6 Goto 13
8: n = n / factor
9: _t9 = factor as string
10: _t10 = _t9 + \"\\n\"
11: Print _t10
12: Goto 5
13: factor = factor + 1
14: Goto 2
15: _t15 = n != 1
16: IfNot _t15 Goto 20
17: _t17 = factor as string
18: _t18 = _t17 + \"\\n\"
19: Print _t18
20: End
```
7
```javascript
// square root

function abs(x: number): number {
    return x < 0 ? -x : x
}

function sqrt(num: number): number {
    let lastGuess: number, guess = num / 3

    lastGuess = guess
    guess = (num / guess + guess) / 2
    while (abs(lastGuess - guess) > 0.000000000000005)
    {
        lastGuess = guess
        guess = (num / guess + guess) / 2
    }

    return guess
}

print(toString(sqrt(3)))
```
```
0: Goto 7
1: _t1 = x < 0
2: IfNot _t1 Goto 5
3: _t =  -x
4: Goto 6
5: _t = x
6: Return _t
7: Goto 25
8: lastGuess = 0
9: guess = num / 3
10: lastGuess = guess
11: guess11 = num / guess
12: guess12 = guess11 + guess
13: guess = guess12 / 2
14: _t14 = lastGuess - guess
15: PushParameter x = _t14
16: _t16 = Call (1, abs), 1
17: _t17 = _t16 > 5E-15
18: IfNot _t17 Goto 24
19: lastGuess = guess
20: guess20 = num / guess
21: guess21 = guess20 + guess
22: guess = guess21 / 2
23: Goto 14
24: Return guess
25: PushParameter num = 3
26: _t26 = Call (8, sqrt), 1
27: _t27 = _t26 as string
28: Print _t27
29: End
```
8
```javascript
function prime(x: number): boolean {
    if (x % 1 != 0 || x <= 3 || x % 2 == 0) {
        return false
    }

    let div = 3
    while (div * div <= x) {
        if (x % div == 0) {
            return false
        }
        div = div + 2
    }
    
    return true
}

print(toString(prime(13)))
```
```
0: Goto 21
1: _t1 = x % 1
2: _t2 = _t1 != 0
3: _t3 = x <= 3
4: _t4 = _t2 || _t3
5: _t5 = x % 2
6: _t6 = _t5 == 0
7: _t7 = _t4 || _t6
8: IfNot _t7 Goto 10
9: Return False
10: div = 3
11: _t11 = div * div
12: _t12 = _t11 <= x
13: IfNot _t12 Goto 20
14: _t14 = x % div
15: _t15 = _t14 == 0
16: IfNot _t15 Goto 18
17: Return False
18: div = div + 2
19: Goto 11
20: Return True
21: PushParameter x = 13
22: _t22 = Call (1, prime), 1
23: _t23 = _t22 as string
24: Print _t23
25: End
```
9
```javascript
function positive(x: number): boolean {
    return x > 0
}

function negative(x: number): boolean {
    return x < 0
}

let x = 0
if (positive(x)) {
    print("positive")
} else if (negative(x)) {
    print("negative")
} else {
    print("zero")
}
```
```
0: Goto 3
1: _t1 = x > 0
2: Return _t1
3: Goto 6
4: _t4 = x < 0
5: Return _t4
6: x = 0
7: PushParameter x = x
8: _t8 = Call (1, positive), 1
9: IfNot _t8 Goto 12
10: Print \"positive\"
11: Goto 18
12: PushParameter x = x
13: _t13 = Call (4, negative), 1
14: IfNot _t13 Goto 17
15: Print \"negative\"
16: Goto 18
17: Print \"zero\"
18: End
```
10
```javascript
let x = 1 > 0 ? 0 <= 1 ? 1 : 0 : -2 < 0 ? -1 : 0
print(toString(x))
```
```
0: _t0 = 1 > 0
1: IfNot _t0 Goto 8
2: _t2 = 0 <= 1
3: IfNot _t2 Goto 6
4: x = 1
5: Goto 7
6: x = 0
7: Goto 14
8: _t8 =  -2
9: _t9 = _t8 < 0
10: IfNot _t9 Goto 13
11: x =  -1
12: Goto 14
13: x = 0
14: _t14 = x as string
15: Print _t14
16: End
```
11
```javascript
function abs(x: number): number {
    return x < 0 ? -x : x
}

function gcd(a: number, b: number): number {
    let aa = abs(a), bb = abs(b)
    if (bb == 0)
        return aa
    return gcd(bb, aa % bb)
}

print(toString(gcd(1280, 720)))
```
```
0: Goto 7
1: _t1 = x < 0
2: IfNot _t1 Goto 5
3: _t =  -x
4: Goto 6
5: _t = x
6: Return _t
7: Goto 20
8: PushParameter x = a
9: aa = Call (1, abs), 1
10: PushParameter x = b
11: bb = Call (1, abs), 1
12: _t12 = bb == 0
13: IfNot _t12 Goto 15
14: Return aa
15: PushParameter a = bb
15: _t15 = aa % bb
17: PushParameter b = _t15
18: _t18 = Call (8, gcd), 2
19: Return _t18
20: PushParameter a = 1280
21: PushParameter b = 720
22: _t22 = Call (8, gcd), 2
23: _t23 = _t22 as string
24: Print _t23
25: End
```
12
```javascript
function abs(x: number): number {
    return x < 0 ? -x : x
}

function gcd(a: number, b: number): number {
    let aa = abs(a), bb = abs(b)
    if (bb == 0)
        return aa
    return gcd(bb, aa % bb)
}

function lcm(a: number, b: number): number {
    if (a == 0 || b == 0)
        return 0
    return abs(a * b) / gcd(a, b)
}

print(toString(lcm(1280, 720)))
```
```
0: Goto 7
1: _t1 = x < 0
2: IfNot _t1 Goto 5
3: _t =  -x
4: Goto 6
5: _t = x
6: Return _t
7: Goto 20
8: PushParameter x = a
9: aa = Call (1, abs), 1
10: PushParameter x = b
11: bb = Call (1, abs), 1
12: _t12 = bb == 0
13: IfNot _t12 Goto 15
14: Return aa
15: PushParameter a = bb
15: _t15 = aa % bb
17: PushParameter b = _t15
18: _t18 = Call (8, gcd), 2
19: Return _t18
20: Goto 34
21: _t21 = a == 0
22: _t22 = b == 0
23: _t23 = _t21 || _t22
24: IfNot _t23 Goto 26
25: Return 0
26: _t26 = a * b
27: PushParameter x = _t26
28: _t28 = Call (1, abs), 1
29: PushParameter a = a
30: PushParameter b = b
31: _t31 = Call (8, gcd), 2
32: _t32 = _t28 / _t31
33: Return _t32
34: PushParameter a = 1280
35: PushParameter b = 720
36: _t36 = Call (21, lcm), 2
37: _t37 = _t36 as string
38: Print _t37
39: End
```
13
```javascript
function floor(x: number): number{
    return x - (x % 1)
}

function fastPow(base: number, power: number): number {
    if (power == 0) {
        return 1
    }

    if (power % 2 == 0) {
        const multiplier = fastPow(base, power / 2)
        return multiplier * multiplier
    }
    
    const f = floor(power / 2)
    const multiplier = fastPow(base, f)
    return multiplier * multiplier * base
}

print(toString(fastPow(13, 3)))
```
```
0: Goto 4
1: _t1 = x % 1
2: _t2 = x - _t1
3: Return _t2
4: Goto 26
5: _t5 = power == 0
6: IfNot _t5 Goto 8
7: Return 1
8: _t8 = power % 2
9: _t9 = _t8 == 0
10: IfNot _t9 Goto 17
11: PushParameter base = base
11: _t11 = power / 2
13: PushParameter power = _t11
14: multiplier = Call (5, fastPow), 2
15: _t15 = multiplier * multiplier
16: Return _t15
17: _t17 = power / 2
18: PushParameter x = _t17
19: f = Call (1, floor), 1
20: PushParameter base = base
21: PushParameter power = f
22: multiplier = Call (5, fastPow), 2
23: _t23 = multiplier * multiplier
24: _t24 = _t23 * base
25: Return _t24
26: PushParameter base = 13
27: PushParameter power = 3
28: _t28 = Call (5, fastPow), 2
29: _t29 = _t28 as string
30: Print _t29
31: End
```
14
```javascript
function ceil(x: number): number {
    return x + (1 - x % 1)
}

print(toString(ceil(3.3)))
```
```
0: Goto 5
1: _t1 = x % 1
2: _t2 = 1 - _t1
3: _t3 = x + _t2
4: Return _t3
5: PushParameter x = 3.3
6: _t6 = Call (1, ceil), 1
7: _t7 = _t6 as string
8: Print _t7
9: End
```
15
```javascript
function search(item: number, range: number): number {
    let i = 1
    while (i <= range) {
        if (i == item) {
            return i * i
        }
        i = i + 1
    }
    return -1
}

print(toString(search(4, 10)))
```
```
0: Goto 12
1: i = 1
2: _t2 = i <= range
3: IfNot _t2 Goto 10
4: _t4 = i == item
5: IfNot _t4 Goto 8
6: _t6 = i * i
7: Return _t6
8: i = i + 1
9: Goto 2
10: _t10 =  -1
11: Return _t10
12: PushParameter item = 4
13: PushParameter range = 10
14: _t14 = Call (1, search), 2
15: _t15 = _t14 as string
16: Print _t15
17: End
```
### Источники:

1. [ECMA-262](https://www.ecma-international.org/publications-and-standards/standards/ecma-262/)
2. [DragonBook](https://suif.stanford.edu/dragonbook/)
3. [Stanford CS143 Lectures](https://web.stanford.edu/class/archive/cs/cs143/cs143.1128/)