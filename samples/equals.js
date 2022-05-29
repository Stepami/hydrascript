type withEquals = {
    prop: number;
    equals: (withEquals) => boolean;
}

let obj1 = {
    prop: 1;
    equals => (that: withEquals): boolean {
        return prop == (that.prop)
    };
}

let obj2 = {
    prop: 2;
    equals => (that: withEquals): boolean {
        return prop == (that.prop)
    };
}

let res = obj1.equals(obj2)
print(res as string)