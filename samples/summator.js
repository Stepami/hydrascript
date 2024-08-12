type summable = {
    x: number;
    y: number;
}

function sum(obj: summable): number {
    return obj.x + obj.y
}

let summator: summable = {
    x: 1;
    y: 2;
}

let s = summator.sum()
print(s as string)