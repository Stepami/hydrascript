let obj1 = {
    p: {
        s: {
            v: 1;
        };
    };
}

let obj2 = {
    k: 2;
}

obj2.k = obj1.p.s.v
print(obj2 as string)