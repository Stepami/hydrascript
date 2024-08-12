type withEquals = {
    prop: number;
}

function equals(obj: withEquals, that: withEquals) {
    return obj.prop == that.prop
}

let obj1: withEquals = {
    prop: 1;
}

let obj2: withEquals = {
    prop: 2;
}

let res = obj1.equals(obj2)
print(res as string)