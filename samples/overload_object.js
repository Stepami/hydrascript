function func(obj:TestObj, x:number){
>>>x
}

function func(obj:TestObj, x:number, y:number){
    return x + y
}

type TestObj = {}

let tObj:TestObj = {}

tObj.func(123)

>>> tObj.func(-1,5)