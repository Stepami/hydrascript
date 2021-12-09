# Курсовая работа по дисциплине "Констрирование компиляторов"

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

### Источники:

1. [ECMA-262](https://www.ecma-international.org/publications-and-standards/standards/ecma-262/)
2. [DragonBook](https://suif.stanford.edu/dragonbook/)
3. [Stanford CS143 Lectures](https://web.stanford.edu/class/archive/cs/cs143/cs143.1128/)