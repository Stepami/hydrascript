function g(a:number, b = 0){
    >>> "g(number, number)"
    >>> a
    >>> b
}

g(1)
g(1,2)

function f(a = 0, b = 1){
    >>> "f(number, number)"
    >>> a
    >>> b
}

function f(){
>>>"f()"
}

function f(a = 0){
>>> "f(number)"
    >>> a
}

f()
f(1)
f(1,2)