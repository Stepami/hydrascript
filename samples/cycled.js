type CycledType = {
    x: CycledType;
}
let obj: CycledType = {
    x: null;
}
obj.x = obj
>>>obj